using System.Xml.Serialization;
using EdFi.SampleDataGenerator.Core.Config;

namespace EdFi.SampleDataGenerator.Console.Config
{
    public class SchoolAttendanceProfile : ISchoolAttendanceProfile
    {
        [XmlAttribute]
        public double AverageAttendanceRate { get; set; }

        [XmlAttribute]
        public double AverageTardyRate { get; set; }
    }
}