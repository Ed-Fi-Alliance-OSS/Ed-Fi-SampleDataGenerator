using System;
using System.Collections.Generic;
using System.Linq;
using EdFi.SampleDataGenerator.Core.Entities;

namespace EdFi.CalendarGenerator.Console
{
    public class GradingPeriodTemplate
    {
        public DateTime StartDate => CalendarDates.Min(d => d.Date);
        public DateTime EndDate => CalendarDates.Max(d => d.Date);

        public DateTime EndDateIncludingHolidays => CalendarDates.Concat(AdditionalHolidays).Max(d => d.Date);

        public int GradingPeriodNumber { get; set; }
        
        public List<SchoolCalendarDate> CalendarDates { get; set; } = new List<SchoolCalendarDate>();
        public List<SchoolCalendarDate> AdditionalHolidays { get; set; } = new List<SchoolCalendarDate>();

        public int TotalInstructionalDays => CalendarDates.Count(cd => cd.Descriptor == CalendarEventDescriptor.InstructionalDay);
    }
}