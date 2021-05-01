using CsvHelper.Configuration;
using EdFi.SampleDataGenerator.Core.Entities;
using EdFi.SampleDataGenerator.Core.Serialization.CsvHelper.Descriptors;
using EdFi.SampleDataGenerator.Core.Serialization.CsvHelper.Standards;

namespace EdFi.SampleDataGenerator.Core.Serialization.CsvHelper.EducationOrganization
{
    public sealed class LearningStandardLookupTypeCsvClassMap : CsvClassMap<LearningStandardLookupType>
    {
        public LearningStandardLookupTypeCsvClassMap()
        {
            Map(x => x.LearningStandardId);
            References<AcademicSubjectDescriptorReferenceTypeCsvClassMap>(x => x.AcademicSubject);
            References<ContentStandardCsvClassMap>(x => x.ContentStandard);
            References<GradeLevelDescriptorReferenceTypeCsvClassMap>(x => x.GradeLevel);
            References<LearningStandardIdentificationCodeCsvClassMap>(x => x.LearningStandardIdentificationCode);
        }
    }
}