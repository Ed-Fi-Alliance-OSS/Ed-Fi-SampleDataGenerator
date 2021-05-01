using EdFi.SampleDataGenerator.Core.Config;
using NUnit.Framework;

namespace EdFi.SampleDataGenerator.Core.UnitTests.Config
{
    [TestFixture]
    public class ImmigrantPopulationProfileValidatorTester : ValidatorTestBase<ImmigrantPopulationProfileValidator, IImmigrantPopulationProfile>
    {
        private static readonly ISampleDataGeneratorConfig TestSampleDataGeneratorConfig = new TestSampleDataGeneratorConfig
        {
            EthnicityMappings = TestEthnicityMapping.Defaults
        };

        private static readonly IStudentProfile TestStudentProfile = new TestStudentProfile
        {
            Name = "Test"
        };

        public ImmigrantPopulationProfileValidatorTester() : base(new ImmigrantPopulationProfileValidator(TestSampleDataGeneratorConfig, TestStudentProfile))
        {
        }

        [Test]
        public void ShouldPassValidProfile()
        {
            var profile = GetValidTestProfile();
            Validate(profile, true);
        }

        [Test]
        public void ShouldFailProfileWithNoCountriesOfOrigin()
        {
            var profile = GetValidTestProfile();
            profile.CountriesOfOrigin = null;

            Validate(profile, false);
        }

        private static TestImmigrantPopulationProfile GetValidTestProfile()
        {
            return new TestImmigrantPopulationProfile
            {
                CountriesOfOrigin = new[]
                {
                    new TestCountryOfOrigin
                    {
                        Frequency = 0.01,
                        Name = "Test",
                        Race = "White"
                    }
                }
            };
        }
    }

    [TestFixture]
    public class CountryOfOriginValidatorTester : ValidatorTestBase<CountryOfOriginValidator, ICountryOfOrigin>
    {
        private static readonly ISampleDataGeneratorConfig TestSampleDataGeneratorConfig = new TestSampleDataGeneratorConfig
        {
            EthnicityMappings = TestEthnicityMapping.Defaults
        };

        private static readonly IStudentProfile TestStudentProfile = new TestStudentProfile
        {
            Name = "Test"
        };

        public CountryOfOriginValidatorTester() : base(new CountryOfOriginValidator(TestSampleDataGeneratorConfig, TestStudentProfile))
        {
        }

        [Test]
        public void ShouldPassValidCountryOfOrigin()
        {
            var coo = GetValidCountryOfOrigin();
            Validate(coo, true);
        }

        [Test]
        public void ShouldFailWithEmtpyName()
        {
            var coo = GetValidCountryOfOrigin();
            coo.Name = "";

            Validate(coo, false);
        }

        [Test]
        public void ShouldFailWithInvalidFrequency()
        {
            var coo = GetValidCountryOfOrigin();
            coo.Frequency = 0;
            Validate(coo, false);

            coo.Frequency = 1.01;
            Validate(coo, false);
        }

        [Test]
        public void ShouldFailWithInvalidRace()
        {
            var coo = GetValidCountryOfOrigin();
            coo.Race = "Test";

            Validate(coo, false);
        }

        private static TestCountryOfOrigin GetValidCountryOfOrigin()
        {
            return new TestCountryOfOrigin
            {
                Name = "Test",
                Frequency = 0.01,
                Race = "White"
            };
        }
    }
}
