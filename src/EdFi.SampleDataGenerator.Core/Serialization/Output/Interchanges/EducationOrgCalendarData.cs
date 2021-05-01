using System.Collections.Generic;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common.Interchanges;
using EdFi.SampleDataGenerator.Core.Entities;

namespace EdFi.SampleDataGenerator.Core.Serialization.Output.Interchanges
{
    [InterchangeOutput(Interchange.EducationOrgCalendarInterchangeName, typeof(InterchangeEducationOrgCalendar), typeof(ComplexObjectType))]
    public partial class EducationOrgCalendarData
    {
        public List<Calendar> Calendar { get; set; } = new List<Calendar>();
        public List<CalendarDate> CalendarDates { get; set; } = new List<CalendarDate>();
        public List<GradingPeriod> GradingPeriods { get; set; } = new List<GradingPeriod>();
        public List<Session> Sessions { get; set; } = new List<Session>();
    }
}
