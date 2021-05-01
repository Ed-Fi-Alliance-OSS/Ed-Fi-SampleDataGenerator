using CsvHelper.Configuration;
using EdFi.SampleDataGenerator.Core.Entities;

namespace EdFi.SampleDataGenerator.Core.Serialization.CsvHelper.EducationOrganization
{
    public sealed class StateEducationAgencyLookupTypeCsvClassMap : CsvClassMap<StateEducationAgencyLookupType>
    {
        public StateEducationAgencyLookupTypeCsvClassMap()
        {
            Map(x => x.EducationOrganizationCategory);
            Map(x => x.NameOfInstitution);
            Map(x => x.StateEducationAgencyId);
            References<EducationOrganizationIdentificationCodeCsvClassMap>(x => x.EducationOrganizationIdentificationCode);
        }
    }
}