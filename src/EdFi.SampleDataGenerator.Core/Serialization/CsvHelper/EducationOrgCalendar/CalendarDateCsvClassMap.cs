using EdFi.SampleDataGenerator.Core.Entities;

namespace EdFi.SampleDataGenerator.Core.Serialization.CsvHelper.EducationOrgCalendar
{
    public sealed partial class CalendarDateCsvClassMap : ComplexObjectTypeCsvClassMap<CalendarDate>
    {
        public CalendarDateCsvClassMap()
        {
            Map(x => x.CalendarEvent);
            Map(x => x.Date);
            References<CalendarReferenceTypeCsvClassMap>(x => x.CalendarReference);
        }
    }
}