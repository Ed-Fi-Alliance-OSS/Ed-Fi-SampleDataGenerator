using CsvHelper.Configuration;
using EdFi.SampleDataGenerator.Core.Entities;

namespace EdFi.SampleDataGenerator.Core.Serialization.CsvHelper.EducationOrganization
{
    public sealed class LearningObjectiveIdentityTypeCsvClassMap : CsvClassMap<LearningObjectiveIdentityType>
    {
        public LearningObjectiveIdentityTypeCsvClassMap()
        {
            Map(x => x.LearningObjectiveId);
            Map(x => x.Namespace);
        }
    }
}