using System.Linq;
using EdFi.SampleDataGenerator.Core.Serialization.Output.Interchanges;

namespace EdFi.SampleDataGenerator.Core.Config.DataFiles
{
    public class DescriptorFileReaderService : IDescriptorFileReaderService
    {
        private readonly IDescriptorFileReader _descriptorFileReader;

        public DescriptorFileReaderService() : this(new DescriptorFileReader())
        {
        }

        public DescriptorFileReaderService(IDescriptorFileReader descriptorFileReader)
        {
            _descriptorFileReader = descriptorFileReader;
        }

        public DescriptorData[] Read(ISampleDataGeneratorConfig config)
        {
            return config.DataFileConfig.DescriptorFiles.Select(descriptorFileMapping => _descriptorFileReader.Read(descriptorFileMapping)).ToArray();
        }
    }
}