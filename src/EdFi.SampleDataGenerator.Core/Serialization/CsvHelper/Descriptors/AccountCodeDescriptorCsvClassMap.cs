using CsvHelper.Configuration;
using EdFi.SampleDataGenerator.Core.Entities;
using EdFi.SampleDataGenerator.Core.Serialization.CsvHelper.EducationOrganization;

namespace EdFi.SampleDataGenerator.Core.Serialization.CsvHelper.Descriptors
{
    public sealed partial class AccountCodeDescriptorCsvClassMap : CsvClassMap<AccountCode>
    {
        public AccountCodeDescriptorCsvClassMap()
        {
            Map(m => m.AccountClassification);
            Map(m => m.AccountCodeDescription);
            Map(m => m.AccountCodeNumber);
            Map(m => m.FiscalYear);
            References<EducationOrganizationReferenceTypeCsvClassMap>(x => x.EducationOrganizationReference);

            ExtensionMappings();
        }
    }
}
