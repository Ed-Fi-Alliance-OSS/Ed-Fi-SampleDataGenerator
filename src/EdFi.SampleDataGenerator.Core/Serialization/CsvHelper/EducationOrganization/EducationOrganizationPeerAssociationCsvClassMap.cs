using CsvHelper.Configuration;
using EdFi.SampleDataGenerator.Core.Entities;

namespace EdFi.SampleDataGenerator.Core.Serialization.CsvHelper.EducationOrganization
{
    public sealed partial class EducationOrganizationPeerAssociationCsvClassMap : CsvClassMap<EducationOrganizationPeerAssociation>
    {
        public EducationOrganizationPeerAssociationCsvClassMap()
        {
            References<EducationOrganizationReferenceTypeCsvClassMap>(x => x.EducationOrganizationReference);
            References<EducationOrganizationReferenceTypeCsvClassMap>(x => x.PeerEducationOrganizationReference);
            ExtensionMappings();
        }
    }
}