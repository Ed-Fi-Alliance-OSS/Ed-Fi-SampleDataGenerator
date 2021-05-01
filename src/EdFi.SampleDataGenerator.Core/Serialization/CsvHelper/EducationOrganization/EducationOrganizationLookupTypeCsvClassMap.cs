using CsvHelper.Configuration;
using EdFi.SampleDataGenerator.Core.Entities;

namespace EdFi.SampleDataGenerator.Core.Serialization.CsvHelper.EducationOrganization
{
    public sealed class EducationOrganizationLookupTypeCsvClassMap : CsvClassMap<EducationOrganizationLookupType>
    {
        public EducationOrganizationLookupTypeCsvClassMap()
        {
            Map(x => x.EducationOrganizationCategory);
            Map(x => x.EducationOrganizationId);
            Map(x => x.NameOfInstitution);
            References<EducationOrganizationIdentificationCodeCsvClassMap>(x => x.EducationOrganizationIdentificationCode);
        }
    }
}