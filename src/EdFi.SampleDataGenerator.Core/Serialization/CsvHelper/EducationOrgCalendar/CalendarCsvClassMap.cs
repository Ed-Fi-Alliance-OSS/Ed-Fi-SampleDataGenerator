using EdFi.SampleDataGenerator.Core.Entities;
using EdFi.SampleDataGenerator.Core.Serialization.CsvHelper.EducationOrganization;

namespace EdFi.SampleDataGenerator.Core.Serialization.CsvHelper.EducationOrgCalendar
{
    public sealed class CalendarCsvClassMap : ComplexObjectTypeCsvClassMap<Calendar>
    {
        public CalendarCsvClassMap()
        {
            Map(x => x.CalendarCode);
            Map(x => x.SchoolYear).ConvertEnumerationType();
            Map(x => x.CalendarType);
            References<SchoolReferenceTypeCsvClassMap>(x => x.SchoolReference);
        }
    }
}