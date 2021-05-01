using CsvHelper.Configuration;
using EdFi.SampleDataGenerator.Core.Entities;

namespace EdFi.SampleDataGenerator.Core.Serialization.CsvHelper.EducationOrganization
{
    public sealed partial class EducationOrganizationNetworkCsvClassMap : CsvClassMap<EducationOrganizationNetwork>
    {
        public EducationOrganizationNetworkCsvClassMap()
        {
            Map(x => x.EducationOrganizationNetworkId);
            Map(x => x.NetworkPurpose);
            Map(x => x.NameOfInstitution);
            Map(x => x.ShortNameOfInstitution);
            References<AddressCsvClassMap>(x => x.Address);
            ExtensionMappings();
        }
    }
}