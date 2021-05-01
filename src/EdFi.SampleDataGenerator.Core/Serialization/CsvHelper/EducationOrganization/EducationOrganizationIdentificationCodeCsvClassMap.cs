using CsvHelper.Configuration;
using EdFi.SampleDataGenerator.Core.Entities;

namespace EdFi.SampleDataGenerator.Core.Serialization.CsvHelper.EducationOrganization
{
    public sealed class EducationOrganizationIdentificationCodeCsvClassMap : CsvClassMap<EducationOrganizationIdentificationCode>
    {
        public EducationOrganizationIdentificationCodeCsvClassMap()
        {
            Map(x => x.IdentificationCode);
            Map(x => x.EducationOrganizationIdentificationSystem);
        }
    }
}