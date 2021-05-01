using EdFi.SampleDataGenerator.Core.Serialization.Output.Interchanges;

namespace EdFi.SampleDataGenerator.Core.Config.DataFiles
{
    public interface IDescriptorFileReaderService
    {
        DescriptorData[] Read(ISampleDataGeneratorConfig config);
    }
}