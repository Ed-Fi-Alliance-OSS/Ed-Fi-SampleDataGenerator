using EdFi.SampleDataGenerator.Core.Config;
using EdFi.SampleDataGenerator.Core.Entities;
using EdFi.SampleDataGenerator.Core.Helpers;

namespace EdFi.SampleDataGenerator.Core.UnitTests.Config
{
    public class TestGradeProfile : IGradeProfile
    {
        public string GradeName { get; set; }
        public IStudentPopulationProfile[] StudentPopulationProfiles { get; set; }
        public IGraduationPlanTemplateReference[] GraduationPlanTemplateReferences { get; set; }
        public IAssessmentParticipationConfiguration[] AssessmentParticipationConfigurations { get; set; }
        public int InitialStudentCount { get; set; }

        public static TestGradeProfile Default => GetGradeProfile(GradeLevelDescriptor.FirstGrade, 1);

        public static TestGradeProfile GetGradeProfile(GradeLevelDescriptor gradeLevel, int totalStudents)
        {
            return new TestGradeProfile
            {
                GradeName = gradeLevel.CodeValue,
                StudentPopulationProfiles = new IStudentPopulationProfile[]
                {
                    TestStudentPopulationProfile.Default
                },
                InitialStudentCount = totalStudents
            };
        }
    }
}