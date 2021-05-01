using EdFi.SampleDataGenerator.Core.Entities;

namespace EdFi.SampleDataGenerator.Core.Serialization.CsvHelper.EducationOrganization
{
    public sealed class LearningObjectiveReferenceTypeCsvClassMap : ReferenceTypeCsvClassMap<LearningObjectiveReferenceType>
    {
        public LearningObjectiveReferenceTypeCsvClassMap()
        {
            References<LearningObjectiveIdentityTypeCsvClassMap>(x => x.LearningObjectiveIdentity);
        }
    }
}