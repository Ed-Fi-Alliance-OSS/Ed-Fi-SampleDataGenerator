using System;
using Fclp;

namespace EdFi.EducationOrganizationGenerator.Console
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

            Setup(a => a.CourseTemplateFilePath)
                .As('c', "courseTemplate")
                .WithDescription("Path to Course template file")
                .SetDefault(".\\Courses.csv");

            Setup(a => a.SchoolNamesFilePath)
                .As('s', "schoolNames")
                .WithDescription("Path to School names file")
                .SetDefault(".\\SchoolNames.csv");

            Setup(a => a.DistrictConfigFilePath)
                .As('d', "districtConfig")
                .WithDescription("Path to district config file (e.g. MyDistrict.xml)")
                .Required();

            Setup(a => a.OutputPath)
                .As('o', "outputPath")
                .WithDescription("Path where output files should be written")
                .SetDefault(".\\");

            Setup(a => a.StreetNamesFilePath)
                .As('t', "streetNames")
                .WithDescription("Path to the Street names file")
                .SetDefault(".\\StreetNames.csv");

            Setup(a => a.ConfigurationSnippetsFilePath)
                .As('p', "configSnippets")
                .WithDescription("Path to XML file containing SDG configuration file snippets")
                .SetDefault(".\\ConfigSnippets.xml");
        }
    }
}
