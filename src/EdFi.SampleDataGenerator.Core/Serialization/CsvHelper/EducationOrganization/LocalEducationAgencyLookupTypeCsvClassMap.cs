using CsvHelper.Configuration;
using EdFi.SampleDataGenerator.Core.Entities;

namespace EdFi.SampleDataGenerator.Core.Serialization.CsvHelper.EducationOrganization
{
    public sealed class LocalEducationAgencyLookupTypeCsvClassMap : CsvClassMap<LocalEducationAgencyLookupType>
    {
        public LocalEducationAgencyLookupTypeCsvClassMap()
        {
            Map(x => x.EducationOrganizationCategory);
            Map(x => x.LocalEducationAgencyId);
            Map(x => x.NameOfInstitution);
            References<EducationOrganizationIdentificationCodeCsvClassMap>(x => x.EducationOrganizationIdentificationCode);
        }
    }
}