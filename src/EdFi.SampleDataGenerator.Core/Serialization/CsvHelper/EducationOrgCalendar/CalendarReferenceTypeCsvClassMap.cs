using EdFi.SampleDataGenerator.Core.Entities;

namespace EdFi.SampleDataGenerator.Core.Serialization.CsvHelper.EducationOrgCalendar
{
    public sealed class CalendarReferenceTypeCsvClassMap : ReferenceTypeCsvClassMap<CalendarReferenceType>
    {
        public CalendarReferenceTypeCsvClassMap()
        {
            References<CalendarIdentityTypeCsvClassMap>(x => x.CalendarIdentity);
        }
    }
}
