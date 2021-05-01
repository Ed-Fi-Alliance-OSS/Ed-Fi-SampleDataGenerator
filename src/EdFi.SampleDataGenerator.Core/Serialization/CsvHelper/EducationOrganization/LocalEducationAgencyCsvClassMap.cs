using EdFi.SampleDataGenerator.Core.Entities;

namespace EdFi.SampleDataGenerator.Core.Serialization.CsvHelper.EducationOrganization
{
    public sealed partial class LocalEducationAgencyCsvClassMap : EducationOrganizationCsvClassMap<LocalEducationAgency>
    {
        public LocalEducationAgencyCsvClassMap()
        {
            Map(x => x.CharterStatus);
            Map(x => x.LocalEducationAgencyCategory);
            Map(x => x.LocalEducationAgencyId);
            References<LocalEducationAgencyReferenceTypeCsvClassMap>(x => x.ParentLocalEducationAgencyReference);
            References<EducationServiceCenterReferenceTypeCsvClassMap>(x => x.EducationServiceCenterReference);
            References<StateEducationAgencyReferenceTypeCsvClassMap>(x => x.StateEducationAgencyReference);
            ExtensionMappings();
        }
    }
}