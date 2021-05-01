using EdFi.SampleDataGenerator.Core.Entities;

namespace EdFi.SampleDataGenerator.Core.Serialization.CsvHelper.EducationOrganization
{
    public sealed class LocalEducationAgencyReferenceTypeCsvClassMap : ReferenceTypeCsvClassMap<LocalEducationAgencyReferenceType>
    {
        public LocalEducationAgencyReferenceTypeCsvClassMap()
        {
            References<LocalEducationAgencyIdentityTypeCsvClassMap>(x => x.LocalEducationAgencyIdentity);
        }
    }
}