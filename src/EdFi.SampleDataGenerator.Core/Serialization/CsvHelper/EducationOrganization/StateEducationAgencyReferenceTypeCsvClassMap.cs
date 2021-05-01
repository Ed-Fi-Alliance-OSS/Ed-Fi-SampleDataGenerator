using EdFi.SampleDataGenerator.Core.Entities;

namespace EdFi.SampleDataGenerator.Core.Serialization.CsvHelper.EducationOrganization
{
    public sealed class StateEducationAgencyReferenceTypeCsvClassMap : ReferenceTypeCsvClassMap<StateEducationAgencyReferenceType>
    {
        public StateEducationAgencyReferenceTypeCsvClassMap()
        {
            References<StateEducationAgencyIdentityTypeCsvClassMap>(x => x.StateEducationAgencyIdentity);
        }
    }
}