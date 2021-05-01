using CsvHelper.Configuration;
using EdFi.SampleDataGenerator.Core.Entities;

namespace EdFi.SampleDataGenerator.Core.Serialization.CsvHelper.EducationOrganization
{
    public sealed class LocationIdentityTypeCsvClassMap : CsvClassMap<LocationIdentityType>
    {
        public LocationIdentityTypeCsvClassMap()
        {
            Map(x => x.ClassroomIdentificationCode);
            References<SchoolReferenceTypeCsvClassMap>(x => x.SchoolReference);
        }
    }
}