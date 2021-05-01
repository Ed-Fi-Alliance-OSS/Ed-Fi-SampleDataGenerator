using CsvHelper.Configuration;
using EdFi.SampleDataGenerator.Core.Entities;
using EdFi.SampleDataGenerator.Core.Serialization.CsvHelper.Descriptors;

namespace EdFi.SampleDataGenerator.Core.Serialization.CsvHelper.Standards
{
    public sealed partial class LearningStandardCsvClassMap : CsvClassMap<LearningStandard>
    {
        public LearningStandardCsvClassMap()
        {
            Map(x => x.LearningStandardId);
            Map(x => x.Description);
            Map(x => x.CourseTitle);
            Map(x => x.Namespace);
            Map(x => x.GradeLevel);
            Map(x => x.AcademicSubject);
            References<ContentStandardCsvClassMap>(x => x.ContentStandard);
            ExtensionMappings();
        }
    }
}
