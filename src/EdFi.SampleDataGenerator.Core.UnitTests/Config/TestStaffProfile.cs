using EdFi.SampleDataGenerator.Core.Config;

namespace EdFi.SampleDataGenerator.Core.UnitTests.Config
{
    public class TestStaffProfile : IStaffProfile
    {
        public IAttributeConfiguration StaffRaceConfiguration { get; set; }
        public IAttributeConfiguration StaffSexConfiguration { get; set; }

        public static TestStaffProfile Default => new TestStaffProfile
        {
            StaffRaceConfiguration = new TestAttributeConfiguration
            {
                Name = "Race",
                AttributeGeneratorConfigurationOptions = new[]
                {
                    new TestAttributeGeneratorConfigurationOption
                    {
                        Frequency = 1.00,
                        Value = "White"
                    }
                }
            },
            StaffSexConfiguration = new TestAttributeConfiguration()
            {
                Name = "Sex",
                AttributeGeneratorConfigurationOptions = new[]
                {
                    new TestAttributeGeneratorConfigurationOption
                    {
                        Frequency = 1.00,
                        Value = "Male"
                    }
                }
            }
        };
    }
}
