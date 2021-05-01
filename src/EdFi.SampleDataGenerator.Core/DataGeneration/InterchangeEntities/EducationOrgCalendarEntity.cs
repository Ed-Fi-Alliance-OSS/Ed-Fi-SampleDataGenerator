using System;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common.Entity;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common.Interchanges;
using EdFi.SampleDataGenerator.Core.Entities;

namespace EdFi.SampleDataGenerator.Core.DataGeneration.InterchangeEntities
{
    public sealed partial class EducationOrgCalendarEntity : Entity
    {
        private EducationOrgCalendarEntity(Type entityType) : base(entityType, Interchange.EducationOrgCalendar)
        {
        }

        public static readonly EducationOrgCalendarEntity Calendar = new EducationOrgCalendarEntity(typeof(Calendar));
        public static readonly EducationOrgCalendarEntity CalendarDate = new EducationOrgCalendarEntity(typeof(CalendarDate));
        public static readonly EducationOrgCalendarEntity GradingPeriod = new EducationOrgCalendarEntity(typeof(GradingPeriod));
        public static readonly EducationOrgCalendarEntity Session = new EducationOrgCalendarEntity(typeof(Session));
    }
}
