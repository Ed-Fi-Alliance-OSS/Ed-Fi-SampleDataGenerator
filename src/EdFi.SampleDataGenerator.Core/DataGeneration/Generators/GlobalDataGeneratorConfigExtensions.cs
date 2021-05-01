using System.Collections.Generic;
using System.Linq;
using EdFi.SampleDataGenerator.Core.Config;
using EdFi.SampleDataGenerator.Core.Date;
using EdFi.SampleDataGenerator.Core.Entities;
using EdFi.SampleDataGenerator.Core.Helpers;

namespace EdFi.SampleDataGenerator.Core.DataGeneration.Generators
{
    public static class GlobalDataGeneratorConfigExtensions
    {
        public static IEnumerable<CalendarDate> GetSchoolInstructionalDays(this GlobalDataGeneratorConfig configuration, SchoolReferenceType schoolReference)
        {
            return configuration.EducationOrgCalendarData.CalendarDates
                .GetInstructionalDays(configuration.GlobalConfig.TimeConfig.DataClockConfig.AsDateRange())
                .Where(cd => cd.CalendarReference.CalendarIdentity.SchoolReference.ReferencesSameSchoolAs(schoolReference));
        }

        public static DateRange GetGeneratorDateRange(this GlobalDataGeneratorConfig globalConfig)
        {
            return globalConfig.GlobalConfig.TimeConfig?.DataClockConfig?.AsDateRange();
        }

        public static List<Session> GetAvailableSessions(this GlobalDataGeneratorConfig globalConfig, ISchoolProfile schoolProfile)
        {
            var generatorDateRange = GetGeneratorDateRange(globalConfig);
            return globalConfig.EducationOrgCalendarData.Sessions
                .ForSchool(schoolProfile)
                .Where(s => generatorDateRange == null || generatorDateRange.Overlaps(new DateRange(s.BeginDate, s.EndDate)))
                .ToList();
        }
    }
}
