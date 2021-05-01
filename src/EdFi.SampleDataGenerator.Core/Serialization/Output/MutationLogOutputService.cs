using System.IO;
using EdFi.SampleDataGenerator.Core.Config;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common.Mutators;
using EdFi.SampleDataGenerator.Core.Serialization.CsvHelper;

namespace EdFi.SampleDataGenerator.Core.Serialization.Output
{
    public class MutationLogOutputService : BufferedOutputService<MutationLogEntry, MutationLogOutputConfiguration>
    {
        private readonly string _fileName;

        public MutationLogOutputService(string fileName)
        {
            _fileName = fileName;
        }

        protected override void WriteOutputToFile()
        {
            var outputFilePath = Path.Combine(CurrentConfiguration.SampleDataGeneratorConfig.OutputPath, _fileName);
            MappedCsvFileWriter.WriteEntityFile(outputFilePath, OutputBuffer);
        }
    }

    public class MutationLogOutputConfiguration
    {
        public ISampleDataGeneratorConfig SampleDataGeneratorConfig { get; set; }
    }
}
