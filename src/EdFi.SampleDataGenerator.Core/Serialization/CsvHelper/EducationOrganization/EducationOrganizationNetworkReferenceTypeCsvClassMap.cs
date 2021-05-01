using EdFi.SampleDataGenerator.Core.Entities;

namespace EdFi.SampleDataGenerator.Core.Serialization.CsvHelper.EducationOrganization
{
    public sealed class EducationOrganizationNetworkReferenceTypeCsvClassMap : ReferenceTypeCsvClassMap<EducationOrganizationNetworkReferenceType>
    {
        public EducationOrganizationNetworkReferenceTypeCsvClassMap()
        {
            References<EducationOrganizationNetworkIdentityTypeCsvClassMap>(x => x.EducationOrganizationNetworkIdentity);
        }
    }
}