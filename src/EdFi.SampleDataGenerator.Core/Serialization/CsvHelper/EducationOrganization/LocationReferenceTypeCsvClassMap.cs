using EdFi.SampleDataGenerator.Core.Entities;

namespace EdFi.SampleDataGenerator.Core.Serialization.CsvHelper.EducationOrganization
{
    public sealed class LocationReferenceTypeCsvClassMap : ReferenceTypeCsvClassMap<LocationReferenceType>
    {
        public LocationReferenceTypeCsvClassMap()
        {
            References<LocationIdentityTypeCsvClassMap>(x => x.LocationIdentity);
        }
    }
}
