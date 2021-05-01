using System.Linq;
using System.Xml.Serialization;
using EdFi.SampleDataGenerator.Console.Config.Staff;
using EdFi.SampleDataGenerator.Core.Config;

namespace EdFi.SampleDataGenerator.Console.Config
{
    public class SchoolProfile : ISchoolProfile
    {
        [XmlAttribute]
        public int SchoolId { get; set; }

        [XmlAttribute]
        public string SchoolName { get; set; }

        [XmlAttribute]
        public int CourseLoad { get; set; }

        [XmlElement("GradeProfile")]
        public GradeProfile[] GradeProfiles { get; set; }
        IGradeProfile[] ISchoolProfile.GradeProfiles => GradeProfiles;

        [XmlElement("StaffProfile")]
        public StaffProfile StaffProfile { get; set; }
        IStaffProfile ISchoolProfile.StaffProfile => StaffProfile;

        [XmlElement("DisciplineProfile")]
        public DisciplineProfile DisciplineProfile { get; set; }
        IDisciplineProfile ISchoolProfile.DisciplineProfile => DisciplineProfile;

        [XmlElement("SchoolAttendanceProfile")]
        public SchoolAttendanceProfile AttendanceProfile { get; set; }
        ISchoolAttendanceProfile ISchoolProfile.AttendanceProfile => AttendanceProfile;

        public int InitialStudentCount { get { return GradeProfiles.Sum(p => p.InitialStudentCount); } }
    }
}