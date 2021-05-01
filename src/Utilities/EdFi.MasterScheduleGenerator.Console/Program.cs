using System;
using System.IO;
using EdFi.MasterScheduleGenerator.Console.Configuration;
using EdFi.MasterScheduleGenerator.Console.Generators;
using EdFi.SampleDataGenerator.Core.Serialization.Output.Interchanges;

namespace EdFi.MasterScheduleGenerator.Console
{
    class Program
    {
        static int Main(string[] args)
        {
            PrintCopyrightMessageToConsole();

            var errorCode = 0;
            try
            {
                var commandLineOptions = ParseCommandLine(args);

                Directory.CreateDirectory(commandLineOptions.OutputPath);

                var generatorConfig = MasterScheduleGeneratorConfigReader.Read(commandLineOptions);

                var data = new MasterScheduleData();
                var generator = new MasterScheduleInterchangeGenerator();

                generator.Configure(generatorConfig);
                generator.Generate(data);

                WriteOutput(commandLineOptions, data);
            }
            catch (Exception e)
            {
                System.Console.WriteLine(e);
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

        static CommandLineOptions ParseCommandLine(string[] args)
        {
            var parser = new CommandLineParser();
            var parseResult = parser.Parse(args);

            if (parseResult.HasErrors)
            {
                throw new Exception(parseResult.ErrorText);
            }

            return parser.Object;
        }

        static void WriteOutput(CommandLineOptions commandLineOptions, MasterScheduleData data)
        {
            Output.MasterScheduleOutputService.WriteOutputFiles(commandLineOptions.OutputPath, data);
        }
    }
}
