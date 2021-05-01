using EdFi.SampleDataGenerator.Core.Entities;

namespace EdFi.SampleDataGenerator.Core.Serialization.CsvHelper.EducationOrganization
{
    public sealed partial class CourseCsvClassMap : ComplexObjectTypeCsvClassMap<Course>
    {
        public CourseCsvClassMap()
        {
            Map(x => x.CourseCode);
            Map(x => x.CareerPathway);
            Map(x => x.CourseDefinedBy);
            Map(x => x.CourseDescription);
            Map(x => x.CourseGPAApplicability);
            Map(x => x.CourseLevelCharacteristic);
            Map(x => x.CourseTitle);
            Map(x => x.NumberOfParts);
            Map(x => x.AcademicSubject);
            Map(x => x.OfferedGradeLevel);
            Map(x => x.CompetencyLevel);
            References<CourseIdentificationCodeCsvClassMap>(x => x.CourseIdentificationCode);
            References<EducationOrganizationReferenceTypeCsvClassMap>(x => x.EducationOrganizationReference);
            References<LearningObjectiveReferenceTypeCsvClassMap>(x => x.LearningObjectiveReference);
            References<LearningStandardReferenceTypeCsvClassMap>(x => x.LearningStandardReference);
            References<CreditsCsvClassMap>(x => x.MinimumAvailableCredits);
            References<CreditsCsvClassMap>(x => x.MaximumAvailableCredits);
            ExtensionMappings();
        }
    }
}