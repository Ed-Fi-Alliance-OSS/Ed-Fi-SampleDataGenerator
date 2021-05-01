using System.Linq;
using EdFi.SampleDataGenerator.Core.Config;
using EdFi.SampleDataGenerator.Core.Config.SeedData;
using EdFi.SampleDataGenerator.Core.DataGeneration.Generators;
using EdFi.SampleDataGenerator.Core.Entities;

namespace EdFi.SampleDataGenerator.Core.Helpers
{
    public static class StudentDataGeneratorContextExtensions
    {
        public static StudentEducationOrganizationAssociation GetStudentEducationOrganization(this StudentDataGeneratorContext context)
        {
            return context.GeneratedStudentData.StudentEnrollmentData.StudentEducationOrganizationAssociation.First();
        }

        public static SeedRecord GetSeedRecord(this StudentDataGeneratorContext context, ISchoolProfile schoolProfile, IGradeProfile gradeProfile)
        {
            return new SeedRecord
            {
                SchoolId = schoolProfile.SchoolId,
                FirstName = context.Student.Name.FirstName,
                LastName = context.Student.Name.LastSurname,
                MiddleName = context.Student.Name.MiddleName,
                BirthDate = context.Student.BirthData.BirthDate,
                PerformanceIndex = context.StudentPerformanceProfile.PerformanceIndex,
                GradeLevel = gradeProfile.GetGradeLevel(),
                Gender = context.StudentCharacteristics.Sex,
                HispanicLatinoEthnicity = context.StudentCharacteristics.HispanicLatinoEthnicity,
                Race = context.StudentCharacteristics.Race
            };
        }
    }
}
