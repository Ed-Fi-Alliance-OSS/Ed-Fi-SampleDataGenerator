using System.Collections.Generic;
using System.IO;
using EdFi.SampleDataGenerator.Core.Entities;
using EdFi.SampleDataGenerator.Core.Serialization.CsvHelper;

namespace EdFi.CalendarGenerator.Console
{
    public class CalendarOutputService
    {
        public const string GradingPeriodFileName = "GradingPeriod.csv";
        public const string SessionFileName = "Session.csv";
        public const string CalendDateFileName = "CalendarDate.csv";

        public void WriteGradingPeriodFile(CalendarGeneratorConfig config, IEnumerable<GradingPeriod> gradingPeriods)
        {
            var outputPath = Path.Combine(config.OutputPath, GradingPeriodFileName);
            MappedCsvFileWriter.WriteEntityFile(outputPath, gradingPeriods);
        }

        public void WriteSessionFile(CalendarGeneratorConfig config, IEnumerable<Session> sessions)
        {
            var outputPath = Path.Combine(config.OutputPath, SessionFileName);
            MappedCsvFileWriter.WriteEntityFile(outputPath, sessions);
        }

        public void WriteCalendarDateFile(CalendarGeneratorConfig config, IEnumerable<CalendarDate> calendarDates)
        {
            var outputPath = Path.Combine(config.OutputPath, CalendDateFileName);
            MappedCsvFileWriter.WriteEntityFile(outputPath, calendarDates);
        }
    }
}
