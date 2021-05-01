using System.IO;

namespace EdFi.SampleDataGenerator.Core.Serialization.Output
{
    public class InterchangeFileOutputService : IInterchangeFileOutputService
    {
        private readonly IInterchangeSerializationService _interchangeSerializationService;

        public InterchangeFileOutputService() : this(new InterchangeXmlSerializationService())
        {
        }

        public InterchangeFileOutputService(IInterchangeSerializationService interchangeSerializationService)
        {
            _interchangeSerializationService = interchangeSerializationService;
        }

        public void WriteOutputToFile<TInterchangeEntity>(string outputFilePath, TInterchangeEntity interchangeEntity)
        {
            if (string.IsNullOrEmpty(outputFilePath) || interchangeEntity == null) return;

            using (var fileStream = new FileStream(outputFilePath, FileMode.Create))
            {
                _interchangeSerializationService.WriteToOutput(interchangeEntity, fileStream);
            }
        }

        public void WriteManifestToFile(string outputFilePath, Manifest manifest)
        {
            manifest.Save(outputFilePath);
        }
    }
}