using CsvHelper.Configuration;
using EdFi.SampleDataGenerator.Core.Entities;

namespace EdFi.SampleDataGenerator.Core.Serialization.CsvHelper.EducationOrganization
{
    public sealed partial class EducationOrganizationNetworkAssociationCsvClassMap : CsvClassMap<EducationOrganizationNetworkAssociation>
    {
        public EducationOrganizationNetworkAssociationCsvClassMap()
        {
            References<EducationOrganizationNetworkReferenceTypeCsvClassMap>(x => x.EducationOrganizationNetworkReference);
            References<EducationOrganizationReferenceTypeCsvClassMap>(x => x.MemberEducationOrganizationReference);
            ExtensionMappings();
        }
    }
}