using CsvHelper.Configuration;
using EdFi.SampleDataGenerator.Core.Entities;
using EdFi.SampleDataGenerator.Core.Serialization.CsvHelper.EducationOrganization;

namespace EdFi.SampleDataGenerator.Core.Serialization.CsvHelper.EducationOrgCalendar
{
    public sealed class CalendarIdentityTypeCsvClassMap : CsvClassMap<CalendarIdentityType>
    {
        public CalendarIdentityTypeCsvClassMap()
        {
            Map(x => x.CalendarCode);
            Map(x => x.SchoolYear).ConvertEnumerationType();
            References<SchoolReferenceTypeCsvClassMap>(x => x.SchoolReference);
        }
    }
}
