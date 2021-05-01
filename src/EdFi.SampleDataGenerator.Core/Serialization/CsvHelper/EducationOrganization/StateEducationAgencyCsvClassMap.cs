using EdFi.SampleDataGenerator.Core.Entities;

namespace EdFi.SampleDataGenerator.Core.Serialization.CsvHelper.EducationOrganization
{
    public sealed partial class StateEducationAgencyCsvClassMap : EducationOrganizationCsvClassMap<StateEducationAgency>
    {
        public StateEducationAgencyCsvClassMap()
        {
            ExtensionMappings();
        }
    }
}
