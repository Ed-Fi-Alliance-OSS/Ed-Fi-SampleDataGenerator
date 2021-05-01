using System.Collections.Generic;
using System.Linq;
using EdFi.SampleDataGenerator.Core.Config;
using EdFi.SampleDataGenerator.Core.Entities;

namespace EdFi.SampleDataGenerator.Core.UnitTests.Config
{
    public class TestSchoolProfile : ISchoolProfile
    {
        public int SchoolId { get; set; }
        public string SchoolName { get; set; }
        public IGradeProfile[] GradeProfiles { get; set; }
        public IStaffProfile StaffProfile { get; set; }
        public IDisciplineProfile DisciplineProfile { get; set; }
        public ISchoolAttendanceProfile AttendanceProfile { get; set; }
        public int CourseLoad { get; set; }
        public int InitialStudentCount { get; set; }

        public static TestSchoolProfile Default => GetSchoolProfile("Eastwood Elementary School", 123456001, new[] {GradeLevelDescriptor.FirstGrade}, 1);

        public static TestSchoolProfile GetSchoolProfile(string schoolName, int schoolId, IEnumerable<GradeLevelDescriptor> gradeLevels, int studentsPerGrade)
        {
            var gradeProfiles = gradeLevels.Select(gl => TestGradeProfile.GetGradeProfile(gl, studentsPerGrade)).ToArray();

            return new TestSchoolProfile
            {
                SchoolId = schoolId,
                SchoolName = schoolName,
                GradeProfiles = gradeProfiles,
                StaffProfile = new TestStaffProfile
                {
                    StaffRaceConfiguration = new TestAttributeConfiguration
                    {
                        Name = "StaffRaceConfiguration",
                        AttributeGeneratorConfigurationOptions = new[]
                        {
                            Option("White", 0.49),
                            Option("Hispanic", 0.29),
                            Option("Black", 0.16),
                            Option("Asian", 0.06)
                        }
                    },
                    StaffSexConfiguration = new TestAttributeConfiguration
                    {
                        Name = "StaffSex",
                        AttributeGeneratorConfigurationOptions = new[]
                        {
                            Option("Male", 0.24),
                            Option("Female", 0.76)
                        }
                    }
                },
                DisciplineProfile = TestDisciplineProfile.Default,
                //AttendanceProfile = ?,
                CourseLoad = 8,
                InitialStudentCount = gradeProfiles.Length * studentsPerGrade,
            };
        }

        private static IAttributeGeneratorConfigurationOption Option(string value, double frequency)
        {
            return new TestAttributeGeneratorConfigurationOption
            {
                Value = value,
                Frequency = frequency
            };
        }
    }
}