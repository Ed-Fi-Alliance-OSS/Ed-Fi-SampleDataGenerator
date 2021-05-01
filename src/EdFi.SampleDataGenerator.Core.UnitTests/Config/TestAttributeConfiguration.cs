using EdFi.SampleDataGenerator.Core.Config;

namespace EdFi.SampleDataGenerator.Core.UnitTests.Config
{
    public class TestAttributeConfiguration : IAttributeConfiguration
    {
        public string Name { get; set; }
        public IAttributeGeneratorConfigurationOption[] AttributeGeneratorConfigurationOptions { get; set; }
    }

    public class TestAttributeGeneratorConfigurationOption : IAttributeGeneratorConfigurationOption
    {
        public string Value { get; set; }
        public double Frequency { get; set; }
    }
}
