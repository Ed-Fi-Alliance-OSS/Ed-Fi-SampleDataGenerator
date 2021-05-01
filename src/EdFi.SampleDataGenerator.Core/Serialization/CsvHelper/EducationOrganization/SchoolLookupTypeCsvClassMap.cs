using CsvHelper.Configuration;
using EdFi.SampleDataGenerator.Core.Entities;

namespace EdFi.SampleDataGenerator.Core.Serialization.CsvHelper.EducationOrganization
{
    public sealed class SchoolLookupTypeCsvClassMap : CsvClassMap<SchoolLookupType>
    {
        public SchoolLookupTypeCsvClassMap()
        {
            Map(x => x.EducationOrganizationCategory);
            Map(x => x.NameOfInstitution);
            Map(x => x.SchoolId);
            References<EducationOrganizationIdentificationCodeCsvClassMap>(x => x.EducationOrganizationIdentificationCode);
        }
    }
}