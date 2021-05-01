using EdFi.SampleDataGenerator.Core.Config;

namespace EdFi.SampleDataGenerator.Core.UnitTests.Config
{
    public class TestStudentProfile : IStudentProfile
    {
        public string Name { get; set; }
        public IAttributeConfiguration RaceConfiguration { get; set; }
        public IAttributeConfiguration SexConfiguration { get; set; }
        public IAttributeConfiguration EconomicDisadvantageConfiguration { get; set; }
        public IAttributeConfiguration HomelessStatusConfiguration { get; set; }
        public IImmigrantPopulationProfile ImmigrantPopulationProfile { get; set; }

        public static TestStudentProfile Default => new TestStudentProfile
        {
            RaceConfiguration = new TestAttributeConfiguration
            {
                Name = "Race",
                AttributeGeneratorConfigurationOptions = new IAttributeGeneratorConfigurationOption[]
                {
                    new TestAttributeGeneratorConfigurationOption
                    {
                        Frequency = 100,
                        Value = "White"
                    }
                }
            },
            SexConfiguration = new TestAttributeConfiguration
            {
                Name = "Sex",
                AttributeGeneratorConfigurationOptions = new IAttributeGeneratorConfigurationOption[]
                {
                    new TestAttributeGeneratorConfigurationOption
                    {
                        Frequency = 100,
                        Value = "Female"
                    }
                }
            },
            EconomicDisadvantageConfiguration = new TestAttributeConfiguration()
            {
                Name = "EconomicDisadvantage",
                AttributeGeneratorConfigurationOptions = new IAttributeGeneratorConfigurationOption[]
                {
                    new TestAttributeGeneratorConfigurationOption
                    {
                        Frequency = 10,
                        Value = "White"
                    }
                }
            },
            Name = "Test Student Profile"
        };
    }
}