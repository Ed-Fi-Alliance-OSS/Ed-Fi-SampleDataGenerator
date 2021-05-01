using System;
using EdFi.SampleDataGenerator.Core.Entities;

namespace EdFi.CalendarGenerator.Console
{
    public class SchoolCalendarDate
    {
        public DateTime Date { get; set; }
        public CalendarEventDescriptor Descriptor { get; set; }
    }
}