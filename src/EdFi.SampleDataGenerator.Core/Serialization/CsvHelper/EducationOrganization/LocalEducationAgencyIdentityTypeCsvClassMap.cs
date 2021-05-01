using CsvHelper.Configuration;
using EdFi.SampleDataGenerator.Core.Entities;

namespace EdFi.SampleDataGenerator.Core.Serialization.CsvHelper.EducationOrganization
{
    public sealed class LocalEducationAgencyIdentityTypeCsvClassMap : CsvClassMap<LocalEducationAgencyIdentityType>
    {
        public LocalEducationAgencyIdentityTypeCsvClassMap()
        {
            Map(x => x.LocalEducationAgencyId);
        }
    }
}