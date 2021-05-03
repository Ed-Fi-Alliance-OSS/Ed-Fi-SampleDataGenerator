using CsvHelper.Configuration;
using EdFi.SampleDataGenerator.Core.Entities;

namespace EdFi.SampleDataGenerator.Core.Serialization.CsvHelper.Standards
{
    public sealed partial class LearningObjectiveCsvClassMap : CsvClassMap<LearningObjective>
    {
        public LearningObjectiveCsvClassMap()
        {
            Map(x => x.Objective);
            Map(x => x.Description);
            Map(x => x.Nomenclature);
            Map(x => x.SuccessCriteria);
            Map(x => x.Namespace);
            References<ContentStandardCsvClassMap>(x => x.ContentStandard);
            Map(x => x.AcademicSubject);
            Map(x => x.ObjectiveGradeLevel);
            References<LearningStandardCsvClassMap>(x => x.LearningStandardReference);
            ExtensionMappings();
        }
    }
}
