using EdFi.SampleDataGenerator.Core.Serialization.Output.Interchanges;

namespace EdFi.SampleDataGenerator.Core.Config.DataFiles
{
    public interface IDescriptorFileReader
    {
        DescriptorData Read(IDescriptorFileMapping fileMapping);
    }
}