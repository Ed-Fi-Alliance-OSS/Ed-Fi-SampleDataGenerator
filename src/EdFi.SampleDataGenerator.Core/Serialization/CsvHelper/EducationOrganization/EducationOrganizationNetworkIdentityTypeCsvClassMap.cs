using CsvHelper.Configuration;
using EdFi.SampleDataGenerator.Core.Entities;

namespace EdFi.SampleDataGenerator.Core.Serialization.CsvHelper.EducationOrganization
{
    public sealed class EducationOrganizationNetworkIdentityTypeCsvClassMap : CsvClassMap<EducationOrganizationNetworkIdentityType>
    {
        public EducationOrganizationNetworkIdentityTypeCsvClassMap()
        {
            Map(x => x.EducationOrganizationNetworkId);
        }
    }
}