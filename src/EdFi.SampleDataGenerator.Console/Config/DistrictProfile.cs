using System.Linq;
using EdFi.SampleDataGenerator.Core.Config;
using System.Xml.Serialization;

namespace EdFi.SampleDataGenerator.Console.Config
{
    public class DistrictProfile : IDistrictProfile
    {
        [XmlAttribute]
        public string DistrictName { get; set; }

        [XmlElement("HighPerformingStudentPercentile")]
        public double? HighPerformingStudentPercentile { get; set; }
        double? IDistrictProfile.HighPerformingStudentPercentile => HighPerformingStudentPercentile;

        [XmlElement("SchoolProfile")]
        public SchoolProfile[] SchoolProfiles { get; set; }
        ISchoolProfile[] IDistrictProfile.SchoolProfiles => SchoolProfiles;

        [XmlElement("LocationInfo")]
        public LocationInfo LocationInfo { get; set; }
        ILocationInfo IDistrictProfile.LocationInfo => LocationInfo;

        public int InitialStudentCount { get { return SchoolProfiles.Sum(p => p.InitialStudentCount); } }
    }
}