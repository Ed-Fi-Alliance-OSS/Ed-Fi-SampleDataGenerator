using EdFi.SampleDataGenerator.Core.Entities;

namespace EdFi.SampleDataGenerator.Core.Serialization.CsvHelper.EducationOrganization
{
    public sealed class LearningStandardReferenceTypeCsvClassMap : ReferenceTypeCsvClassMap<LearningStandardReferenceType>
    {
        public LearningStandardReferenceTypeCsvClassMap()
        {
            References<LearningStandardIdentityTypeCsvClassMap>(x => x.LearningStandardIdentity);
        }
    }
}