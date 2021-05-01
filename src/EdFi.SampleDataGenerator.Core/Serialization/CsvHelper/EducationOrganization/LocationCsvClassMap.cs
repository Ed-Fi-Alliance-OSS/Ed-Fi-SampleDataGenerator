using EdFi.SampleDataGenerator.Core.Entities;

namespace EdFi.SampleDataGenerator.Core.Serialization.CsvHelper.EducationOrganization
{
    public sealed partial class LocationCsvClassMap : ComplexObjectTypeCsvClassMap<Location>
    {
        public LocationCsvClassMap()
        {
            Map(x => x.ClassroomIdentificationCode);
            Map(x => x.MaximumNumberOfSeats);
            Map(x => x.OptimalNumberOfSeats);
            References<SchoolReferenceTypeCsvClassMap>(x => x.SchoolReference);
            ExtensionMappings();
        }
    }
}