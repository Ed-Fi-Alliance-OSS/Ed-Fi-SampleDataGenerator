using CsvHelper.Configuration;
using EdFi.SampleDataGenerator.Core.Entities;

namespace EdFi.SampleDataGenerator.Core.Serialization.CsvHelper.EducationOrganization
{
    public sealed class SchoolIdentityTypeCsvClassMap : CsvClassMap<SchoolIdentityType>
    {
        public SchoolIdentityTypeCsvClassMap()
        {
            Map(x => x.SchoolId);
        }
    }
}