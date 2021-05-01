using System;
using Fclp;

namespace EdFi.MasterScheduleGenerator.Console
{
    public class CommandLineParser : FluentCommandLineParser<CommandLineOptions>
    {
        public CommandLineParser()
        {
            SetupHelp("?", "Help").Callback(text =>
            {
                System.Console.WriteLine(text);
                Environment.Exit(0);
            });

            Setup(a => a.SchoolYear)
                .As('y', "schoolYear")
                .WithDescription("School year to generate (in form 2016-2017)")
                .Required();

            Setup(a => a.SchoolFilePath)
                .As('s', "schoolFile")
                .WithDescription("Path to the School.csv file")
                .SetDefault(".\\School.csv");

            Setup(a => a.CourseFilePath)
                .As('c', "courseFile")
                .WithDescription("Path to the Course.csv file")
                .SetDefault(".\\Course.csv");

            Setup(a => a.ClassPeriodFilePath)
                .As('p', "classPeriodFile")
                .WithDescription("Path to the ClassPeriod.csv file")
                .SetDefault(".\\ClassPeriod.csv");

            Setup(a => a.LocationFilePath)
                .As('l', "locationFile")
                .WithDescription("Path to the Location file")
                .SetDefault(".\\Location.csv");

            Setup(a => a.OutputPath)
                .As('o', "outputPath")
                .WithDescription("Path where output files should be written")
                .SetDefault(".\\");

            Setup(a => a.SessionFilePath)
                .As('e', "sessionFile")
                .WithDescription("Path to the Session.csv file")
                .SetDefault(".\\Session.csv");
        }
    }
}
