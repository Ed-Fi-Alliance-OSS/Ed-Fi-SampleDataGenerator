using System;
using System.IO;
using System.Linq;
using System.Reflection;
using EdFi.SampleDataGenerator.Core.Config;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common.Interchanges;
using EdFi.SampleDataGenerator.Core.Serialization.Output.Interchanges;

namespace EdFi.SampleDataGenerator.Core.Serialization.Output
{
    public class StudentDataOutputConfiguration
    {
        public ISchoolProfile SchoolProfile { get; set; }
        public ISampleDataGeneratorConfig SampleDataGeneratorConfig { get; set; }
        public IDataPeriod DataPeriod { get; set; }
        public IStudentPerformanceProfileOutput StudentPerformanceProfileOutput { get; set; }
    }

    public interface IStudentDataOutputService
    {
        void Configure(StudentDataOutputConfiguration configuration);
        void WriteToOutput(GeneratedStudentData record, double studentPerformanceIndex);
        void FlushOutput();
        void SaveManifest();
    }

    public class StudentDataOutputService : IStudentDataOutputService
    {
        private readonly StudentDataOutputBuffer _buffer = new StudentDataOutputBuffer();
        private StudentDataOutputConfiguration _configuration;
        private Manifest _manifest;
        private IStudentPerformanceProfileOutput _studentPerformanceProfileOutput = new StudentPerformanceProfileOutput();

        private readonly IInterchangeFileOutputService _interchangeFileOutputService;

        private int _writesToBuffer = 0;
        public int BatchId { get; private set; }
        public int BatchSize { get; private set; }

        public StudentDataOutputService() : this(new InterchangeFileOutputService())
        {
        }

        public StudentDataOutputService(IInterchangeFileOutputService interchangeFileOutputService)
        {
            _interchangeFileOutputService = interchangeFileOutputService;
        }

        public void Configure(StudentDataOutputConfiguration configuration)
        {
            _configuration = configuration;
            _manifest = new Manifest();
            BatchId = 1;
            BatchSize = GetBatchSize(configuration);
            _studentPerformanceProfileOutput = new StudentPerformanceProfileOutput();
        }

        private static readonly Lazy<PropertyInfo[]> GeneratedStudentDataMembers = new Lazy<PropertyInfo[]>
        (
            () => typeof (GeneratedStudentData).GetProperties(BindingFlags.Public | BindingFlags.Instance)
        );
        public void WriteToOutput(GeneratedStudentData record, double studentPerformanceIndex)
        {
            ++_writesToBuffer;

            ApplyActionToBufferMembers(buffer =>
            {
                var studentDataMember = GeneratedStudentDataMembers.Value.FirstOrDefault(m => m.PropertyType == buffer.SdgEntityType);
                if (studentDataMember == null) return;

                var valueToSaveToBuffer = studentDataMember.GetValue(record);
                buffer.CopyToCollection(valueToSaveToBuffer);
            });

            if(_configuration.SampleDataGeneratorConfig.CreatePerformanceFile)
                _studentPerformanceProfileOutput.Add(record.StudentData.Student.StudentUniqueId, studentPerformanceIndex);

            if (BatchSize > 0 && _writesToBuffer >= BatchSize)
            {
                FlushOutput();
            }
        }

        public void FlushOutput()
        {
            ApplyActionToBufferMembers(buffer =>
            {
                var outputInfo = buffer.GetType().GetInterchangeOutputInfo();
                WriteOutputToFile(outputInfo.Interchange, buffer.ConvertToEdFiInterchange());
                buffer.Clear();
            });

            FlushOutStudentPerformanceOutput();

            _writesToBuffer = 0;
            ++BatchId;
        }

        private void ApplyActionToBufferMembers(Action<ISdgEntityOutputCollection> bufferAction)
        {
            var bufferMembers = _buffer.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);

            foreach (var member in bufferMembers)
            {
                var collection = member.GetValue(_buffer) as ISdgEntityOutputCollection;
                if (collection != null)
                {
                    bufferAction(collection);
                }
            }
        }

        private void FlushOutStudentPerformanceOutput()
        {
            if (!_configuration.SampleDataGeneratorConfig.CreatePerformanceFile) return;

            var fileName = GetOutputFilePath("StudentPerformance");
            var filePath = Path.Combine(_configuration.SampleDataGeneratorConfig.OutputPath, fileName);

            _studentPerformanceProfileOutput.FlushOutput(filePath);
        }

        public void SaveManifest()
        {
            if (_configuration.SampleDataGeneratorConfig.OutputMode != OutputMode.Standard) return;

            var fileName = $"Manifest-{_configuration.SchoolProfile.SchoolName}-{_configuration.DataPeriod.Name}.xml";
            var manifestFilePath = Path.Combine(_configuration.SampleDataGeneratorConfig.OutputPath, fileName);

            _interchangeFileOutputService.WriteManifestToFile(manifestFilePath, _manifest);
        }

        private static int GetBatchSize(StudentDataOutputConfiguration configuration)
        {
            return configuration.SampleDataGeneratorConfig.BatchSize.HasValue &&
                   configuration.SampleDataGeneratorConfig.BatchSize.Value > 0
                        ? Math.Min(configuration.SchoolProfile.InitialStudentCount, configuration.SampleDataGeneratorConfig.BatchSize.Value)
                        : configuration.SchoolProfile.InitialStudentCount;
        }

        public string GetOutputFilePath(Interchange interchange)
        {
            return GetOutputFilePath(interchange.Name);
        }
        public string GetOutputFilePath(string name)
        {
            var includeBatchId = BatchSize > 0;

            var fileName = includeBatchId
                ? $"{name}-{_configuration.SchoolProfile.SchoolName}-{_configuration.DataPeriod.Name}-{BatchId:D4}.xml"
                : $"{name}-{_configuration.SchoolProfile.SchoolName}-{_configuration.DataPeriod.Name}.xml";

            return fileName;
        }

        private string FullyQualifyPath(string fileName)
        {
            return Path.Combine(_configuration.SampleDataGeneratorConfig.OutputPath, fileName);
        }

        private void WriteOutputToFile<TInterchangeItem>(Interchange interchange, TInterchangeItem interchangeItemToOutput)
        {
            if (interchangeItemToOutput == null) return;

            var outputFilePath = GetOutputFilePath(interchange);

            _interchangeFileOutputService.WriteOutputToFile(FullyQualifyPath(outputFilePath), interchangeItemToOutput);

            _manifest.Add(interchange, outputFilePath);
        }
    }
}