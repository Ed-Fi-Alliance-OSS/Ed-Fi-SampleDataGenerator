using EdFi.SampleDataGenerator.Core.Config;
using NUnit.Framework;

namespace EdFi.SampleDataGenerator.Core.UnitTests.Config
{
    [TestFixture]
    public class LocationInfoValidatorTester : ValidatorTestBase<LocationInfoValidator, ILocationInfo>
    {
        public LocationInfoValidatorTester() : base(new LocationInfoValidator("test district"))
        {
        }

        [Test]
        public void ShouldPassValidLocationInfo()
        {
            var locationInfo = GetValidLocationInfo();
            Validate(locationInfo, true);;
        }

        [Test]
        public void ShouldFailWithEmptyState()
        {
            var locationInfo = GetValidLocationInfo();
            locationInfo.State = "";

            Validate(locationInfo, false);
        }

        [Test]
        public void ShouldAllowStateWithImproperCapitalization()
        {
            var locationInfo = GetValidLocationInfo();
            locationInfo.State = "tx";

            Validate(locationInfo, true);
        }

        [Test]
        public void ShouldFailInvalidState()
        {
            var locationInfo = GetValidLocationInfo();
            locationInfo.State = "XT";

            Validate(locationInfo, false);
        }

        [Test]
        public void ShouldFailWithEmptyCities()
        {
            var locationInfo = GetValidLocationInfo();
            locationInfo.Cities = new ICity[] {};

            Validate(locationInfo, false);
        }

        private static TestLocationInfo GetValidLocationInfo()
        {
            return new TestLocationInfo
            {
                Cities = new []
                {
                    new TestCity
                    {
                        AreaCodes = new[]
                        {
                            new TestAreaCode { Value = 123 }
                        },
                        Name = "Test City",
                        County = "Test County",
                        PostalCodes = new []
                        {
                            new TestPostalCode
                            {
                                Value = "123456"
                            }
                        }
                    }
                },
                State = "TX"
            };
        }
    }

    [TestFixture]
    public class CityValidatorTester : ValidatorTestBase<CityValidator, ICity>
    {
        public CityValidatorTester() : base(new CityValidator("test district"))
        {
        }

        [Test]
        public void ShouldPassValidCity()
        {
            var city = GetValidTestCity();
            Validate(city, true);
        }

        [Test]
        public void ShouldFailEmptyName()
        {
            var city = GetValidTestCity();
            city.Name = "";

            Validate(city, false);
        }

        [Test]
        public void ShouldFailEmtpyCounty()
        {
            var city = GetValidTestCity();
            city.County = "";

            Validate(city, false);
        }

        [Test]
        public void ShouldFailEmptyAreaCodes()
        {
            var city = GetValidTestCity();
            city.AreaCodes = new IAreaCode[] {};

            Validate(city, false);
        }

        [Test]
        public void ShouldFailEmptyPostalCodes()
        {
            var city = GetValidTestCity();
            city.PostalCodes = new IPostalCode[] {};

            Validate(city, false);
        }
        
        private static TestCity GetValidTestCity()
        {
            return new TestCity
            {
                AreaCodes = new[]
                {
                    new TestAreaCode {Value = 123}
                },
                Name = "Test City",
                County = "Test County",
                PostalCodes = new[]
                {
                    new TestPostalCode
                    {
                        Value = "123456"
                    }
                }
            };
        }
    }

    [TestFixture]
    public class AreaCodeValidatorTester : ValidatorTestBase<AreaCodeValidator, IAreaCode>
    {
        public AreaCodeValidatorTester() : base(new AreaCodeValidator("test city","test district"))
        {
        }

        [Test]
        public void ShouldPassValidAreaCode()
        {
            var areaCode = new TestAreaCode {Value = 123};
            Validate(areaCode, true);
        }

        [Test]
        public void ShouldFailAreaCodeGreaterThan999()
        {
            var areaCode = new TestAreaCode { Value = 1000 };
            Validate(areaCode, false);
        }

        [Test]
        public void ShouldFailAreaCodeLessThan0()
        {
            var areaCode = new TestAreaCode { Value = -1 };
            Validate(areaCode, false);
        }
    }

    [TestFixture]
    public class PostalCodeValidatorTester : ValidatorTestBase<PostalCodeValidator, IPostalCode>
    {
        public PostalCodeValidatorTester() : base(new PostalCodeValidator("",""))
        {
        }

        [Test]
        public void ShouldPassValidNumericPostalCode()
        {
            var postalCode = new TestPostalCode {Value = "12345"};
            Validate(postalCode, true);
        }

        [Test]
        public void ShouldPassValidTextPostalCode()
        {
            var postalCode = new TestPostalCode { Value = "ABC 123" };
            Validate(postalCode, true);
        }

        [Test]
        public void ShouldFailEmptyPostalCode()
        {
            var postalCode = new TestPostalCode { Value = "" };
            Validate(postalCode, false);
        }
    }
}
