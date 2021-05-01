using System;
using Fclp;

namespace EdFi.InterchangeXmlToCsv.Console
{
    public class CommandLineParser : FluentCommandLineParser<InterchangeXmlToCsvConfig>
    {
        public CommandLineParser()
        {
            SetupHelp("?", "Help").Callback(text =>
            {
                System.Console.WriteLine(text);
                Environment.Exit(0);
            });

            Setup(a => a.InputPath)
                .As('i', "inputPath")
                .WithDescription("Path to the input XML file or directory of XML files")
                .Required();

            Setup(a => a.Recurse)
                .As('r', "recurse")
                .WithDescription("If specified and inputPath is a folder, all XML files contained in inputPath and its subfolders will be processed")
                .SetDefault(false);

            Setup(a => a.OutputPath)
                .As('o', "outputPath")
                .WithDescription("Path where output CSV file(s) will be written")
                .Required();

            Setup(a => a.InterchangeName)
                .As('n', "interchangeName")
                .WithDescription("Name of Ed-Fi Interchange for which the file(s) contain data, e.g. Students, Descriptors, EducationOrganization")
                .Required();
        }
    }
}
