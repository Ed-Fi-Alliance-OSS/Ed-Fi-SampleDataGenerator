using EdFi.SampleDataGenerator.Core.Entities;

namespace EdFi.SampleDataGenerator.Core.Serialization.CsvHelper.EducationOrganization
{
    public abstract class EducationOrganizationCsvClassMap<TEducationOrganization> : ComplexObjectTypeCsvClassMap<TEducationOrganization>
        where TEducationOrganization: Entities.EducationOrganization
    {
        protected EducationOrganizationCsvClassMap()
        {
            Map(x => x.NameOfInstitution);
            Map(x => x.ShortNameOfInstitution);
            Map(x => x.EducationOrganizationCategory);
            Map(x => x.OperationalStatus);
            Map(x => x.WebSite);
            References<EducationOrganizationIdentificationCodeCsvClassMap>(x => x.EducationOrganizationIdentificationCode);
            References<AddressCsvClassMap>(x => x.Address);
            References<InstitutionTelephoneCsvClassMap>(x => x.InstitutionTelephone);
        }
    }
}