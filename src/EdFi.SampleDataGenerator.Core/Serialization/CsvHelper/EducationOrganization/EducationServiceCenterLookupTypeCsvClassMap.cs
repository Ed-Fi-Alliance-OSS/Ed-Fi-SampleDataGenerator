using CsvHelper.Configuration;
using EdFi.SampleDataGenerator.Core.Entities;

namespace EdFi.SampleDataGenerator.Core.Serialization.CsvHelper.EducationOrganization
{
    public sealed class EducationServiceCenterLookupTypeCsvClassMap : CsvClassMap<EducationServiceCenterLookupType>
    {
        public EducationServiceCenterLookupTypeCsvClassMap()
        {
            Map(x => x.EducationServiceCenterId);
            Map(x => x.NameOfInstitution);
            Map(x => x.EducationOrganizationCategory);
            References<EducationOrganizationIdentificationCodeCsvClassMap>(x => x.EducationOrganizationIdentificationCode);
        }
    }
}