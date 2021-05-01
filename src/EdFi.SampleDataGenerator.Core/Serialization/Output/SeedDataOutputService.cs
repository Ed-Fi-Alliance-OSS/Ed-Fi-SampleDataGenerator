using System.Linq;
using EdFi.SampleDataGenerator.Core.Config;
using EdFi.SampleDataGenerator.Core.Config.SeedData;
using log4net;

namespace EdFi.SampleDataGenerator.Core.Serialization.Output
{
    public class SeedDataOutputService : BufferedOutputService<SeedRecord, ISampleDataGeneratorConfig>
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(SeedDataOutputService));
        private readonly ISeedDataSerializationService _seedDataSerializationService;

        public SeedDataOutputService() : this(new SeedDataSerializationService())
        {
        }

        public SeedDataOutputService(ISeedDataSerializationService seedDataSerializationService)
        {
            _seedDataSerializationService = seedDataSerializationService;
        }

        protected override void WriteOutputToFile()
        {
            var seedRecords = OutputBuffer.ToList();
            _seedDataSerializationService.Write(CurrentConfiguration, seedRecords);

            _log.Info($"Total {seedRecords.Count} seed records generated.");
        }
    }
}
