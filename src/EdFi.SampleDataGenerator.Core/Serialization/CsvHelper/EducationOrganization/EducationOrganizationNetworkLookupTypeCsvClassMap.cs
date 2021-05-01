using CsvHelper.Configuration;
using EdFi.SampleDataGenerator.Core.Entities;

namespace EdFi.SampleDataGenerator.Core.Serialization.CsvHelper.EducationOrganization
{
    public sealed class EducationOrganizationNetworkLookupTypeCsvClassMap : CsvClassMap<EducationOrganizationNetworkLookupType>
    {
        public EducationOrganizationNetworkLookupTypeCsvClassMap()
        {
            Map(x => x.EducationOrganizationCategory);
            Map(x => x.EducationOrganizationNetworkId);
            Map(x => x.NameOfInstitution);
            References<EducationOrganizationIdentificationCodeCsvClassMap>(x => x.EducationOrganizationIdentificationCode);
        }
    }
}