using System;
using System.IO;
using System.Linq;
using EdFi.SampleDataGenerator.Core.Entities;
using EdFi.SampleDataGenerator.Core.Helpers;
using EdFi.SampleDataGenerator.Core.Serialization.CsvHelper;

namespace EdFi.CalendarGenerator.Console
{
    class Program
    {
        static int Main(string[] args)
        {
            PrintCopyrightMessageToConsole();

            var errorCode = 0;
            try
            {
                var config = ReadConfiguration(args);

                Directory.CreateDirectory(config.OutputPath);

                var termTemplates = GenerateTermTemplates(config);
                ValidateTemplate(termTemplates);

                WriteOutput(config, termTemplates);
            }
            catch (Exception e)
            {
                System.Console.WriteLine(e.Message);
                errorCode = -1;
            }

#if DEBUG
            System.Console.Write("Press any key to continue...");
            System.Console.ReadKey();
#endif

            return errorCode;
        }

        private static void PrintCopyrightMessageToConsole()
        {
            const string copyrightText =
                "\r\n" +
                "Sample Data Generator is Copyright \u00a9 2018 Ed-Fi Alliance, LLC\r\n" +
                "License info available at https://techdocs.ed-fi.org/display/SDG/Licensing \r\n";

            //Set encoding to UTF8 so copyright symbol in the above message prints correctly
            System.Console.OutputEncoding = System.Text.Encoding.UTF8;
            System.Console.WriteLine(copyrightText);
        }

        private static void ValidateTemplate(SchoolYearTemplate termTemplates)
        {
            var startYear = termTemplates.StartDate.Year;
            var expectedEndYear = startYear + 1;
            if (termTemplates.EndDate.Year != expectedEndYear)
            {
                var message = $@"The calendar generated with the input parameters does not result in a valid school year (e.g. {startYear}-{expectedEndYear}).  At present, this utility cannot generate such a calendar.
StartDate={termTemplates.StartDate.Date:D}
EndDate={termTemplates.EndDate.Date:D}
";
                throw new NotSupportedException(message);
            }
        }

        static CalendarGeneratorConfig ReadConfiguration(string[] args)
        {
            var config = ParseCommandLine(args);
            ValidateConfiguration(config);

            if (string.IsNullOrEmpty(config.SchoolFilePath)) return config;

            var schools = MappedCsvFileReader.ReadEntityFile<School>(config.SchoolFilePath);
            var schoolIds = schools.Select(s => s.SchoolId.ToString());

            config.SchoolIds = config.SchoolIds.SafeConcat(schoolIds).Distinct().ToList();

            return config;
        }

        static CalendarGeneratorConfig ParseCommandLine(string[] args)
        {
            var parser = new CommandLineParser();
            var parseResult = parser.Parse(args);

            if (parseResult.HasErrors)
            {
                throw new Exception(parseResult.ErrorText);
            }

            return parser.Object;
        }

        static void ValidateConfiguration(CalendarGeneratorConfig config)
        {
            if (config.GradingPeriodLengthInWeeks != 6 && config.GradingPeriodLengthInWeeks != 9)
            {
                throw new Exception("Grading Period length must be either 6 or 9 weeks");
            }

            if (config.SchoolYearStartDate.IsWeekendDay())
            {
                System.Console.WriteLine($"Warning: {config.SchoolYearStartDate:yyyy MMMM dd} is a {config.SchoolYearStartDate.DayOfWeek}.  Are you sure your school calendar starts on a weekend?");
            }

            if (string.IsNullOrEmpty(config.SchoolFilePath) && config.SchoolIds.Count == 0)
            {
                throw new Exception("No SchoolIds provided either via the Ids argument or a School.csv file");
            }

            if (!string.IsNullOrEmpty(config.SchoolFilePath) && config.SchoolIds?.Count > 0)
            {
                System.Console.Write($"Warning: School Ids provided both as a command line argument and via the School.csv file - these sets of Ids will be combined");
            }
        }

        static SchoolYearTemplate GenerateTermTemplates(CalendarGeneratorConfig config)
        {
            var generator = new SchoolYearTemplateGenerator();
            return generator.Generate(config);
        }

        static void WriteOutput(CalendarGeneratorConfig config, SchoolYearTemplate termTemplates)
        {
            var outputService = new CalendarOutputService();
            outputService.WriteGradingPeriodFile(config, CalendarTemplateMappingService.MapToGradingPeriods(config, termTemplates));
            outputService.WriteCalendarFile(config, CalendarTemplateMappingService.MapToCalendars(config, termTemplates));
            outputService.WriteCalendarDateFile(config, CalendarTemplateMappingService.MapToCalendarDates(config, termTemplates));
            outputService.WriteSessionFile(config, CalendarTemplateMappingService.MapToSessions(config, termTemplates));
        }
    }
}
