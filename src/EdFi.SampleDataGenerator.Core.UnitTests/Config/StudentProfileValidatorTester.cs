using EdFi.SampleDataGenerator.Core.Config;
using NUnit.Framework;

namespace EdFi.SampleDataGenerator.Core.UnitTests.Config
{
    [TestFixture]
    public class StudentProfileValidatorTester : ValidatorTestBase<StudentProfileValidator, IStudentProfile>
    {
        private static readonly ISampleDataGeneratorConfig TestConfig = new TestSampleDataGeneratorConfig
        {
            EthnicityMappings = TestEthnicityMapping.Defaults,
            GenderMappings = TestGenderMapping.Defaults
        };

        public StudentProfileValidatorTester() : base(new StudentProfileValidator(TestConfig))
        {
        }

        [Test]
        public void ShouldPassValidConfig()
        {
            var profile = GetValidTestStudentProfile();
            Validate(profile, true);
        }

        [Test]
        public void ShouldFailWithEmptyName()
        {
            var profile = GetValidTestStudentProfile();
            profile.Name = "";

            Validate(profile, false);
        }

        [Test]
        public void ShouldFailWithEmptyRaceAttributeConfigurations()
        {
            var profile = GetValidTestStudentProfile();
            profile.RaceConfiguration = null;

            Validate(profile, false);
        }

        [Test]
        public void ShouldFailWithEmptySexAttributeConfigurations()
        {
            var profile = GetValidTestStudentProfile();
            profile.SexConfiguration = null;

            Validate(profile, false);
        }

        [Test]
        public void ShouldPassWithEmptyEconomicDisadvantageConfig()
        {
            var profile = GetValidTestStudentProfile();
            profile.EconomicDisadvantageConfiguration = null;

            Validate(profile, true);
        }

        [Test]
        public void ShouldFailWithInvalidRaceInEconomicDisadvantageConfig()
        {
            var profile = GetValidTestStudentProfile();
            ((TestAttributeGeneratorConfigurationOption)profile.EconomicDisadvantageConfiguration.AttributeGeneratorConfigurationOptions[0]).Value = "fake";

            Validate(profile, false);
        }

        [Test]
        public void ShouldPassWithEmptyHomelessStatusConfig()
        {
            var profile = GetValidTestStudentProfile();
            profile.HomelessStatusConfiguration = null;

            Validate(profile, true);
        }

        [Test]
        public void ShouldFailWithInvalidRaceInHomelessStatusConfig()
        {
            var profile = GetValidTestStudentProfile();
            ((TestAttributeGeneratorConfigurationOption)profile.HomelessStatusConfiguration.AttributeGeneratorConfigurationOptions[0]).Value = "fake";

            Validate(profile, false);
        }

        private static TestStudentProfile GetValidTestStudentProfile()
        {
            return new TestStudentProfile
            {
                Name = "Test",
                RaceConfiguration = new TestAttributeConfiguration
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
                SexConfiguration = new TestAttributeConfiguration
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
                },
                EconomicDisadvantageConfiguration = new TestAttributeConfiguration
                {
                    Name = "EconomicDisadvantage",
                    AttributeGeneratorConfigurationOptions = new []
                    {
                        new TestAttributeGeneratorConfigurationOption
                        {
                            Frequency = 0.01,
                            Value = "White"
                        }
                    }
                },
                HomelessStatusConfiguration = new TestAttributeConfiguration
                {
                    Name = "HomelessStatus",
                    AttributeGeneratorConfigurationOptions = new[]
                    {
                        new TestAttributeGeneratorConfigurationOption
                        {
                            Frequency = 0.01,
                            Value = "White"
                        }
                    }
                }
            };
        }
    }
}
