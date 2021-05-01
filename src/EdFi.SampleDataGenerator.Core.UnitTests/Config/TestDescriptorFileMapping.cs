using EdFi.SampleDataGenerator.Core.Config;

namespace EdFi.SampleDataGenerator.Core.UnitTests.Config
{
    public class TestDescriptorFileMapping : IDescriptorFileMapping
    {
        public string FilePath { get; set; }
        public string DescriptorName { get; set; }
    }
}
