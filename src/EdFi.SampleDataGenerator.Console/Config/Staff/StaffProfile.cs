using System.Xml.Serialization;
using EdFi.SampleDataGenerator.Core.Config;

namespace EdFi.SampleDataGenerator.Console.Config.Staff
{
    public class StaffProfile : IStaffProfile
    {
        [XmlElement("StaffRaceConfiguration")]
        public RaceConfiguration StaffRaceConfiguration { get; set; }
        IAttributeConfiguration IStaffProfile.StaffRaceConfiguration => StaffRaceConfiguration;

        [XmlElement("StaffSexConfiguration")]
        public StaffSexConfiguration StaffSexConfiguration { get; set; }
        IAttributeConfiguration IStaffProfile.StaffSexConfiguration => StaffSexConfiguration;
    }
}
