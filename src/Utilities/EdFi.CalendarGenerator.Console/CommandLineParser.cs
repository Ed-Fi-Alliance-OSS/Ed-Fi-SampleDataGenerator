using System;
using Fclp;

namespace EdFi.CalendarGenerator.Console
{
    public class CommandLineParser : FluentCommandLineParser<CalendarGeneratorConfig>
    {
        public CommandLineParser()
        {
            SetupHelp("?", "Help").Callback(text =>
            {
                System.Console.WriteLine(text);
                Environment.Exit(0);
            });

            Setup(a => a.TermType)
                .As('t', "termType")
                .WithDescription("Type of term used by school (e.g. semester)")
                .SetDefault(TermType.Semester);

            Setup(a => a.GradingPeriodLengthInWeeks)
                .As('g', "gradingPeriodLength")
                .WithDescription("Length (in weeks) of grading period.  Must be either 6 or 9")
                .Required();

            Setup(a => a.SchoolYearStartDate)
                .As('s', "schoolStartDate")
                .WithDescription("Date on which school year begins")
                .Required();

            Setup(a => a.SchoolIds)
                .As('i', "schoolId")
                .WithDescription("Id(s) of schools for which you want calendar data to be generated");

            Setup(a => a.SchoolFilePath)
                .As('f', "schoolFile")
                .WithDescription("Path to School.csv file that contains target School Ids");

            Setup(a => a.TeacherWorkdaysPerGradingPeriod)
                .As('w', "workDays")
                .WithDescription("Number of teacher-only work days per grading period")
                .SetDefault(0);

            Setup(a => a.BadWeatherDaysPerGradingPeriod)
                .As('b', "badWeatherDays")
                .WithDescription("Number of bad weather days per grading period")
                .SetDefault(0);

            Setup(a => a.OutputPath)
                .As('o', "outputPath")
                .WithDescription("Path where output files will be stored")
                .SetDefault("");
        }
    }
}
