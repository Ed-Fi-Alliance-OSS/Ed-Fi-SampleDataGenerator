using System;
using System.IO;
using EdFi.EducationOrganizationGenerator.Console.Configuration;
using EdFi.EducationOrganizationGenerator.Console.Generators;
using EdFi.EducationOrganizationGenerator.Console.SampleDataGeneratorConfiguration;
using EdFi.SampleDataGenerator.Core.Config.Xml;
using EdFi.SampleDataGenerator.Core.Serialization.Output.Interchanges;

namespace EdFi.EducationOrganizationGenerator.Console
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

                var generatorConfig = EducationOrganizationGeneratorConfigReader.Read(commandLineOptions);
                var configurationSnippets = XmlConfigHelpers.ParseConfigFileToObject<ConfigurationSnippets>(commandLineOptions.ConfigurationSnippetsFilePath);

                var data = GenerateEducationOrganizationData(generatorConfig);
                var districtProfileConfig = MergeDistrictProfileConfigSnippets(configurationSnippets, generatorConfig, data);

                WriteEducationOrganizationOutputFiles(commandLineOptions, data);
                WriteDistrictInfoConfigurationOutputFile(commandLineOptions, generatorConfig.DistrictProfile, districtProfileConfig);
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

        private static EducationOrganizationData GenerateEducationOrganizationData(EducationOrganizationGeneratorConfig generatorConfig)
        {
            var data = new EducationOrganizationData();
            var generator = new EducationOrganizationInterchangeGenerator();
            generator.Configure(generatorConfig);
            generator.Generate(data);
            return data;
        }

        private static string MergeDistrictProfileConfigSnippets(ConfigurationSnippets configurationSnippets, EducationOrganizationGeneratorConfig generatorConfig, EducationOrganizationData educationOrganizationData)
        {
            var merger = new ConfigurationSnippetsMerger();
            var result = merger.Merge(configurationSnippets, generatorConfig, educationOrganizationData);

            return result;
        }

        private static CommandLineOptions ParseCommandLine(string[] args)
        {
            var parser = new CommandLineParser();
            var parseResult = parser.Parse(args);

            if (parseResult.HasErrors)
            {
                throw new Exception(parseResult.ErrorText);
            }

            return parser.Object;
        }

        private static void WriteEducationOrganizationOutputFiles(CommandLineOptions commandLineOptions, EducationOrganizationData data)
        {
            Output.EducationOrganizationOutputService.WriteOutputFiles(commandLineOptions.OutputPath, data);
        }

        private static void WriteDistrictInfoConfigurationOutputFile(CommandLineOptions commandLineOptions, DistrictProfile districtProfile, string districtProfileConfig)
        {
            Output.ConfigurationSnippetsOutputService.WriteOutputFile(commandLineOptions.OutputPath, districtProfile, districtProfileConfig);
        }
    }
}
