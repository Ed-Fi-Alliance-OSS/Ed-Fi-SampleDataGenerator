using System.Linq;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common.Entity;
using EdFi.SampleDataGenerator.Core.DataGeneration.InterchangeEntities;
using EdFi.SampleDataGenerator.Core.Entities;
using EdFi.SampleDataGenerator.Core.Helpers;
using EdFi.SampleDataGenerator.Core.Serialization.Output.Interchanges;

namespace EdFi.EducationOrganizationGenerator.Console.Generators
{
    public class CourseEntityGenerator : EducationOrganizationEntityGenerator
    {
        public CourseEntityGenerator(IRandomNumberGenerator randomNumberGenerator) : base(randomNumberGenerator)
        {
        }

        public override IEntity GeneratesEntity => EducationOrganizationEntity.Course;
        public override IEntity[] DependsOnEntities => new IEntity[] { EducationOrganizationEntity.School };

        protected override void GenerateCore(EducationOrganizationData context)
        {
            foreach (var school in context.Schools)
            {
                var schoolType = school.SchoolCategory.First();
                var schoolProfile = Configuration.DistrictProfile.SchoolProfiles.First(sp => sp.SchoolType == schoolType);
                var schoolGradeLevels = schoolProfile.GradeProfiles.Select(gp => gp.GradeLevel);

                var courseTemplates = Configuration.CourseTemplates.Where(t => t.OfferedGradeLevel.Any(gl => schoolGradeLevels.Contains(gl)));

                foreach (var courseTemplate in courseTemplates)
                {
                    var course = new Course
                    {
                        id = $"CRSE_{school.SchoolId}_{courseTemplate.CourseCode}",
                        EducationOrganizationReference = EdFiReferenceTypeHelpers.GetEducationOrganizationReference(school.SchoolId),
                        CourseCode = courseTemplate.CourseCode,
                        CareerPathway = courseTemplate.CareerPathway,
                        CourseDefinedBy = courseTemplate.CourseDefinedBy,
                        CourseDescription = courseTemplate.CourseDescription,
                        CourseGPAApplicability = courseTemplate.CourseGPAApplicability,
                        CourseLevelCharacteristic = courseTemplate.CourseLevelCharacteristic,
                        CourseTitle = courseTemplate.CourseTitle,
                        NumberOfParts = courseTemplate.NumberOfParts,
                        AcademicSubject = courseTemplate.AcademicSubject,
                        CompetencyLevel = courseTemplate.CompetencyLevel,
                        CourseIdentificationCode = courseTemplate.CourseIdentificationCode,
                        LearningObjectiveReference = courseTemplate.LearningObjectiveReference,
                        LearningStandardReference = courseTemplate.LearningStandardReference,
                        MinimumAvailableCredits = courseTemplate.MinimumAvailableCredits,
                        MaximumAvailableCredits = courseTemplate.MaximumAvailableCredits,
                        OfferedGradeLevel = courseTemplate.OfferedGradeLevel
                    };

                    context.Courses.Add(course);
                }
            }
        }
    }
}
