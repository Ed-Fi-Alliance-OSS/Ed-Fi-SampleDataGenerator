using System.Collections.Generic;
using System.Linq;
using EdFi.SampleDataGenerator.Core.Config;
using EdFi.SampleDataGenerator.Core.Date;
using EdFi.SampleDataGenerator.Core.Entities;
using EdFi.SampleDataGenerator.Core.Helpers;

namespace EdFi.SampleDataGenerator.Core.DataGeneration.Generators
{
    public static class StudentDataGeneratorConfigExtensions
    {
        public static IEnumerable<CalendarDate> GetSchoolInstructionalDays(this StudentDataGeneratorConfig configuration)
        {
            return configuration.EducationOrgCalendarData.CalendarDates
                .GetInstructionalDays(configuration.GlobalConfig.TimeConfig.DataClockConfig.AsDateRange())
                .Where(cd => cd.CalendarReference.CalendarIdentity.SchoolReference.ReferencesSchool(configuration.SchoolProfile));
        }

        public static DateRange GetGeneratorDateRange(this StudentDataGeneratorConfig configuration)
        {
            return configuration.GlobalConfig.TimeConfig?.DataClockConfig?.AsDateRange();
        }

        public static List<Session> GetAvailableSessions(this StudentDataGeneratorConfig configuration)
        {
            var generatorDateRange = GetGeneratorDateRange(configuration);
            return configuration.EducationOrgCalendarData.Sessions
                .ForSchool(configuration.SchoolProfile)
                .Where(s => generatorDateRange == null || generatorDateRange.Overlaps(new DateRange(s.BeginDate, s.EndDate)))
                .ToList();
        }

        public static List<Session> GetAvailableSessions(this StudentDataGeneratorConfig configuration, IDataPeriod dataPeriod)
        {
            return configuration.EducationOrgCalendarData.Sessions
                .ForSchool(configuration.SchoolProfile)
                .Where(s => dataPeriod.AsDateRange().Overlaps(s.AsDateRange()))
                .ToList();
        }

        public static SchoolYearType GetCurrentSchoolYear(this StudentDataGeneratorConfig configuration)
        {
            return configuration.GlobalConfig.TimeConfig.SchoolCalendarConfig.SchoolYear();
        }
    }
}