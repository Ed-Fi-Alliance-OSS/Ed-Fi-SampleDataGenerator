using System.Collections.Generic;
using System.Linq;
using EdFi.SampleDataGenerator.Core.Date;
using EdFi.SampleDataGenerator.Core.Entities;

namespace EdFi.SampleDataGenerator.Core.Helpers
{
    public static class CalendarDateHelpers
    {
        public static IEnumerable<CalendarDate> GetInstructionalDays(this IEnumerable<CalendarDate> calendarDates)
        {
            return calendarDates
                .Where(cd => cd.CalendarEvent.Contains(CalendarEventDescriptor.InstructionalDay.GetStructuredCodeValue())) 
                .OrderBy(x => x.Date);
        }

        public static IEnumerable<CalendarDate> GetInstructionalDays(this IEnumerable<CalendarDate> calendarDates, DateRange dateRange)
        {
            return calendarDates
                .WithinDateRange(dateRange)
                .Where(cd => cd.CalendarEvent.Contains(CalendarEventDescriptor.InstructionalDay.GetStructuredCodeValue()))
                .OrderBy(x => x.Date);
        }

        public static IEnumerable<CalendarDate> WithinDateRange(this IEnumerable<CalendarDate> calendarDates, DateRange dateRange)
        {
            return dateRange == null ? Enumerable.Empty<CalendarDate>() : calendarDates.Where(cd => dateRange.Contains(cd.Date));
        }
    }
}
