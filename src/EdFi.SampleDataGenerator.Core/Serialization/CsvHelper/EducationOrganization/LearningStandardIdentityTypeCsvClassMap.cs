using CsvHelper.Configuration;
using EdFi.SampleDataGenerator.Core.Entities;

namespace EdFi.SampleDataGenerator.Core.Serialization.CsvHelper.EducationOrganization
{
    public sealed class LearningStandardIdentityTypeCsvClassMap : CsvClassMap<LearningStandardIdentityType>
    {
        public LearningStandardIdentityTypeCsvClassMap()
        {
            Map(x => x.LearningStandardId);
        }
    }
}