using CsvHelper.Configuration;
using EdFi.SampleDataGenerator.Core.Entities;

namespace EdFi.SampleDataGenerator.Core.Serialization.CsvHelper.EducationOrganization
{
    public sealed class InstitutionTelephoneCsvClassMap : CsvClassMap<InstitutionTelephone>
    {
        public InstitutionTelephoneCsvClassMap()
        {
            Map(x => x.InstitutionTelephoneNumberType);
            Map(x => x.TelephoneNumber);
        }
    }
}