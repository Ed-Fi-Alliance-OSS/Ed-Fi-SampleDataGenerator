using EdFi.SampleDataGenerator.Core.Entities;

namespace EdFi.SampleDataGenerator.Core.Serialization.CsvHelper.EducationOrganization
{
    public sealed class EducationOrganizationReferenceTypeCsvClassMap : ReferenceTypeCsvClassMap<EducationOrganizationReferenceType>
    {
        public EducationOrganizationReferenceTypeCsvClassMap()
        {
            References<EducationOrganizationIdentityTypeCsvClassMap>(x => x.EducationOrganizationIdentity);
        }
    }
}