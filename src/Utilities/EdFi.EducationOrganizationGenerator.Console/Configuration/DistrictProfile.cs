using System.Xml.Serialization;
using EdFi.SampleDataGenerator.Core.Config;
using EdFi.SampleDataGenerator.Core.Entities;

namespace EdFi.EducationOrganizationGenerator.Console.Configuration
{
    public class DistrictProfile
    {
        [XmlAttribute]
        public string DistrictName { get; set; }

        [XmlAttribute]
        public string StateId { get; set; }

        [XmlElement("SchoolProfile")]
        public SchoolProfile[] SchoolProfiles { get; set; }

        [XmlElement("LocationInfo")]
        public LocationInfo LocationInfo { get; set; }
    }

    public class SchoolProfile
    {
        [XmlAttribute]
        public string SchoolType { get; set; }

        [XmlAttribute]
        public int Count { get; set; }

        [XmlElement("GradeProfile")]
        public GradeProfile[] GradeProfiles { get; set; }
    }

    public class GradeProfile
    {
        [XmlAttribute]
        public string GradeLevel { get; set; }

        [XmlAttribute]
        public int InitialStudentCount { get; set; }
    }


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
