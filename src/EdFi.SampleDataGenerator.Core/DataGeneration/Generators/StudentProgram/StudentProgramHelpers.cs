using EdFi.SampleDataGenerator.Core.Config;

namespace EdFi.SampleDataGenerator.Core.DataGeneration.Generators.StudentProgram
{
    public static class StudentProgramHelpers
    {
        public static bool StudentBecameUnenrolledBeforeEndOfSchoolCalendar(StudentDataGeneratorContext context, StudentDataGeneratorConfig configuration)
        {
            var calendarEndDate = configuration.GlobalConfig.TimeConfig.SchoolCalendarConfig.EndDate;

            return context.EnrollmentDateRange.EndDate < calendarEndDate;
        }

        public static bool StudentBecameEnrolledDuringDataPeriod(StudentDataGeneratorContext context, IDataPeriod dataPeriod)
        {
            return dataPeriod.AsDateRange().Contains(context.EnrollmentDateRange.StartDate);
        }

        public static bool StudentBecameUnenrolledDuringDataPeriod(StudentDataGeneratorContext context, IDataPeriod dataPeriod)
        {
            return dataPeriod.AsDateRange().Contains(context.EnrollmentDateRange.EndDate);
        }
    }
}
