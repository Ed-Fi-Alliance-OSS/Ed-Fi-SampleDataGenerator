using EdFi.SampleDataGenerator.Core.Config;

namespace EdFi.SampleDataGenerator.Core.UnitTests.Config
{
    public class TestImmigrantPopulationProfile : IImmigrantPopulationProfile
    {
        public ICountryOfOrigin[] CountriesOfOrigin { get; set; }
    }

    public class TestCountryOfOrigin : ICountryOfOrigin
    {
        public string Name { get; set; }
        public string Race { get; set; }
        public double Frequency { get; set; }
    }
}
