using CsvHelper.Configuration;
using EdFi.SampleDataGenerator.Core.Entities;

namespace EdFi.SampleDataGenerator.Core.Serialization.CsvHelper.EducationOrganization
{
    public sealed class EducationOrganizationIdentityTypeCsvClassMap : CsvClassMap<EducationOrganizationIdentityType>
    {
        public EducationOrganizationIdentityTypeCsvClassMap()
        {
            Map(x => x.EducationOrganizationId);
        }
    }
}