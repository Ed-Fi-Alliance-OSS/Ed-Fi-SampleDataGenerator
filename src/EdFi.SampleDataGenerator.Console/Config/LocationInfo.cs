using System.Xml.Serialization;
using EdFi.SampleDataGenerator.Core.Config;

namespace EdFi.SampleDataGenerator.Console.Config
{
    public class LocationInfo : ILocationInfo
    {
        [XmlAttribute]
        public string State { get; set; }

        [XmlElement("City")]
        public City[] Cities { get; set; }
        ICity[] ILocationInfo.Cities => Cities;
    }

    public class City : ICity
    {
        [XmlAttribute]
        public string Name { get; set; }

        [XmlAttribute]
        public string County { get; set; }

        [XmlElement("AreaCode")]
        public AreaCode[] AreaCodes { get; set; }
        IAreaCode[] ICity.AreaCodes => AreaCodes;

        [XmlElement("PostalCode")]
        public PostalCode[] PostalCodes { get; set; }
        IPostalCode[] ICity.PostalCodes => PostalCodes;
    }

    public class AreaCode : IAreaCode
    {
        [XmlAttribute]
        public int Value { get; set; }
    }

    public class PostalCode : IPostalCode
    {
        [XmlAttribute]
        public string Value { get; set; }
    }
}
