using EdFi.SampleDataGenerator.Core.Entities;
using EdFi.SampleDataGenerator.Core.Serialization.CsvHelper.Descriptors;
using EdFi.SampleDataGenerator.Core.Serialization.CsvHelper.Standards;

namespace EdFi.SampleDataGenerator.Core.Serialization.CsvHelper.AssessmentMetadata
{
    public sealed partial class AssessmentCsvClassMap : ComplexObjectTypeCsvClassMap<Assessment>
    {
        public AssessmentCsvClassMap()
        {
            Map(x => x.AssessmentTitle);
            Map(x => x.AssessmentVersion);
            Map(x => x.RevisionDate);
            Map(x => x.MaxRawScore);
            Map(x => x.AssessmentCategory);
            Map(x => x.AcademicSubject);
            Map(x => x.AssessedGradeLevel);
            Map(x => x.LowestAssessedGradeLevel);
            Map(x => x.Namespace);
            References<AssessmentIdentificationCodeCsvClassMap>(x => x.AssessmentIdentificationCode);
            References<AssessmentPerformanceLevelCsvClassMap>(x => x.AssessmentPerformanceLevel);
            References<ContentStandardCsvClassMap>(x => x.ContentStandard);
            ExtensionMappings();
        }
    }
}
