using System;
using System.Collections.Generic;
using EdFi.SampleDataGenerator.Core.Config;

namespace EdFi.SampleDataGenerator.Core.Serialization.Output
{
    public interface IStudentDataOutputCoordinator
    {
        void Configure(ISampleDataGeneratorConfig configuration);
        void WriteToOutput(GeneratedStudentData dataToOutput, ISchoolProfile schoolProfile, IDataPeriod dataPeriod, double performanceIndex);
        void FinalizeOutput(ISchoolProfile schoolProfile, IEnumerable<IDataPeriod> dataPeriods);
    }
    
    public class StudentDataOutputCoordinator : IStudentDataOutputCoordinator
    {
        private ISampleDataGeneratorConfig _configuration;
        private readonly Dictionary<string, IStudentDataOutputService> _studentDataOutputServicesByDataPeriodName = new Dictionary<string, IStudentDataOutputService>();

        private readonly Func<IStudentDataOutputService> _outputServiceFactory;

        public StudentDataOutputCoordinator() : this(() => new StudentDataOutputService())
        {
        }

        public StudentDataOutputCoordinator(Func<IStudentDataOutputService> outputServiceFactory)
        {
            _outputServiceFactory = outputServiceFactory;
        }

        public void Configure(ISampleDataGeneratorConfig configuration)
        {
            _configuration = configuration;
        }

        public void WriteToOutput(GeneratedStudentData dataToOutput, ISchoolProfile schoolProfile, IDataPeriod dataPeriod, double performanceIndex)
        {
            if (_configuration == null) throw new InvalidOperationException("Configure must be called before this method may be used");
            var outputService = GetOrCreateOutputService(schoolProfile, dataPeriod);
            outputService.WriteToOutput(dataToOutput, performanceIndex);
        }

        public void FinalizeOutput(ISchoolProfile schoolProfile, IEnumerable<IDataPeriod> dataPeriods)
        {
            foreach (var dataPeriod in dataPeriods)
            {
                FinalizeOutput(schoolProfile, dataPeriod);
            }
        }

        private void FinalizeOutput(ISchoolProfile schoolProfile, IDataPeriod dataPeriod)
        {
            if (_configuration == null) throw new InvalidOperationException("Configure must be called before this method may be used");

            var outputService = GetOrCreateOutputService(schoolProfile, dataPeriod);
            outputService.FlushOutput();
            outputService.SaveManifest();

            RemoveOutputService(schoolProfile, dataPeriod);
        }

        private IStudentDataOutputService GetOrCreateOutputService(ISchoolProfile schoolProfile, IDataPeriod dataPeriod)
        {
            IStudentDataOutputService result;

            var key = GetOutputFileKey(schoolProfile, dataPeriod);
            if (!_studentDataOutputServicesByDataPeriodName.TryGetValue(key, out result))
            {
                var configuration = new StudentDataOutputConfiguration
                {
                    SampleDataGeneratorConfig = _configuration,
                    DataPeriod = dataPeriod,
                    SchoolProfile = schoolProfile
                };

                result = _outputServiceFactory.Invoke();
                result.Configure(configuration);

                _studentDataOutputServicesByDataPeriodName[key] = result;
            }

            return result;
        }

        private void RemoveOutputService(ISchoolProfile schoolProfile, IDataPeriod dataPeriod)
        {
            var key = GetOutputFileKey(schoolProfile, dataPeriod);
            _studentDataOutputServicesByDataPeriodName.Remove(key);
        }

        private string GetOutputFileKey(ISchoolProfile schoolProfile, IDataPeriod dataPeriod)
        {
            return $"{schoolProfile.SchoolName}-{dataPeriod.Name}";
        }
    }
}