using System.Xml.Serialization;
using EdFi.SampleDataGenerator.Core.Config;

namespace EdFi.SampleDataGenerator.Console.Config
{
    public class ImmigrantPopulationProfile : IImmigrantPopulationProfile
    {
        [XmlElement("CountryOfOrigin")]
        public CountryOfOrigin[] CountriesOfOrigin { get; set; }
        ICountryOfOrigin[] IImmigrantPopulationProfile.CountriesOfOrigin => CountriesOfOrigin;
    }

    public class CountryOfOrigin : ICountryOfOrigin
    {
        [XmlAttribute]
        public string Name { get; set; }

        [XmlAttribute]
        public string Race { get; set; }

        [XmlAttribute]
        public double Frequency { get; set; }
    }
}
