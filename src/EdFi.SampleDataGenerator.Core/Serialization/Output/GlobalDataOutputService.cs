using System;
using System.IO;
using System.Reflection;
using EdFi.SampleDataGenerator.Core.Config;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common.Interchanges;
using EdFi.SampleDataGenerator.Core.Serialization.Output.Interchanges;

namespace EdFi.SampleDataGenerator.Core.Serialization.Output
{
    public class GlobalDataOutputConfiguration
    {
        public ISampleDataGeneratorConfig SampleDataGeneratorConfig { get; set; }
    }

    public class GlobalDataOutputService
    {
        private readonly IInterchangeFileOutputService _interchangeFileOutputService;
        private GlobalDataOutputConfiguration _currentConfiguration;

        public GlobalDataOutputService() : this(new InterchangeFileOutputService())
        {
        }

        public GlobalDataOutputService(IInterchangeFileOutputService interchangeFileOutputService)
        {
            _interchangeFileOutputService = interchangeFileOutputService;
        }

        public void Configure(GlobalDataOutputConfiguration configuration)
        {
            _currentConfiguration = configuration;
        }

        public void WriteToOutput(GlobalData record, IDataPeriod dataPeriod)
        {
            if (_currentConfiguration == null)
                throw new InvalidOperationException("The Configure method must be called prior to calling WriteToOutput");

            if (_currentConfiguration.SampleDataGeneratorConfig.OutputMode != OutputMode.Standard) return;

            var manifest = new Manifest();

            WriteOutputToFile(record, manifest, dataPeriod);
            WriteManifestToFile(manifest, dataPeriod);
        }

        private void WriteManifestToFile(Manifest manifest, IDataPeriod dataPeriod)
        {
            _interchangeFileOutputService.WriteManifestToFile(FullyQualifyPath($"ManifestGlobalData-{dataPeriod.Name}.xml"), manifest);
        }

        private void WriteOutputToFile(GlobalData record, Manifest manifest, IDataPeriod dataPeriod)
        {
            var globalDataMembers = typeof(GlobalData).GetProperties(BindingFlags.Instance | BindingFlags.Public);

            foreach (var globalDataMember in globalDataMembers)
            {
                var interchangeOutputInfo = globalDataMember.GetInterchangeOutputInfo();
                var interchangeItemToOutput = record.ConvertPropertyToEdFiInterchange(globalDataMember);
                if (interchangeItemToOutput == null) continue;

                WriteOutputToFile(interchangeOutputInfo.Interchange, interchangeItemToOutput, manifest, dataPeriod);
            }
        }

        private void WriteOutputToFile<TInterchangeItem>(Interchange interchange, TInterchangeItem interchangeItemToOutput, Manifest manifest, IDataPeriod dataPeriod)
        {
            if (interchangeItemToOutput == null) return;

            var fileName = $"{interchange.Name}-{dataPeriod.Name}.xml";

            _interchangeFileOutputService.WriteOutputToFile(FullyQualifyPath(fileName), interchangeItemToOutput);

            manifest.Add(interchange, fileName);
        }

        private string FullyQualifyPath(string filename)
        {
            return Path.Combine(_currentConfiguration.SampleDataGeneratorConfig.OutputPath, filename);
        }
    }
}