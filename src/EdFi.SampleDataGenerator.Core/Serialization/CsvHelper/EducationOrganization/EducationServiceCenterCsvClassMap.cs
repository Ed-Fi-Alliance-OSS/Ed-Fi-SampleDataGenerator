using EdFi.SampleDataGenerator.Core.Entities;

namespace EdFi.SampleDataGenerator.Core.Serialization.CsvHelper.EducationOrganization
{
    public sealed partial class EducationServiceCenterCsvClassMap : EducationOrganizationCsvClassMap<EducationServiceCenter>
    {
        public EducationServiceCenterCsvClassMap()
        {
            Map(x => x.EducationServiceCenterId);
            Map(x => x.EducationOrganizationCategory);
            Map(x => x.NameOfInstitution);
            Map(x => x.OperationalStatus);
            Map(x => x.ShortNameOfInstitution);
            Map(x => x.WebSite);

            References<StateEducationAgencyReferenceTypeCsvClassMap>(x => x.StateEducationAgencyReference);
            ExtensionMappings();
        }
    }
}