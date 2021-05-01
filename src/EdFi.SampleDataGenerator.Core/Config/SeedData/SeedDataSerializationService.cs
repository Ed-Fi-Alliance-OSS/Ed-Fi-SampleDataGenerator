using System.Collections.Generic;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common;
using EdFi.SampleDataGenerator.Core.Serialization.CsvHelper;

namespace EdFi.SampleDataGenerator.Core.Config.SeedData
{
    public interface ISeedDataSerializationService
    {
        List<SeedRecord> Read(ISampleDataGeneratorConfig config);
        void Write(ISampleDataGeneratorConfig config, IEnumerable<SeedRecord> seedRecords);
    }

    public class SeedDataSerializationService : ISeedDataSerializationService
    {
        public List<SeedRecord> Read(ISampleDataGeneratorConfig config)
        {
            if (config.OutputMode == OutputMode.Seed) return new List<SeedRecord>();
            return MappedCsvFileReader.ReadEntityFile<SeedRecord>(config.SeedFilePath);
        }

        public void Write(ISampleDataGeneratorConfig config, IEnumerable<SeedRecord> seedRecords)
        {
            MappedCsvFileWriter.WriteEntityFile(config.SeedFilePath, seedRecords);
        }
    }
}
