using EdFi.SampleDataGenerator.Core.Config;

namespace EdFi.SampleDataGenerator.Core.UnitTests.Config
{
    public class TestLocationInfo : ILocationInfo
    {
        public string State { get; set; }
        public ICity[] Cities { get; set; }

        public static TestLocationInfo Default = new TestLocationInfo
        {
            State = "TX",
            Cities = new ICity[]
            {
                TestCity.Default
            }
        };
    }

    public class TestCity : ICity
    {
        public string Name { get; set; }
        public string County { get; set; }
        public IAreaCode[] AreaCodes { get; set; }
        public IPostalCode[] PostalCodes { get; set;  }

        public static TestCity Default = new TestCity
        {
            Name = "Test City Name",
            AreaCodes = new IAreaCode[]
            {
                TestAreaCode.Default
            },
            County = "Test County",
            PostalCodes = new IPostalCode[] { TestPostalCode.Default }
        };
    }

    public class TestAreaCode : IAreaCode
    {
        public int Value { get; set; }

        public static TestAreaCode Default = new TestAreaCode
        {
            Value = 555
        };
    }

    public class TestPostalCode : IPostalCode
    {
        public string Value { get; set; }

        public static TestPostalCode Default = new TestPostalCode
        {
            Value = "78701"
        };
    }
}
