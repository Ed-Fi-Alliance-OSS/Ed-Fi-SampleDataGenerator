using System;
using System.IO;
using System.Linq;
using EdFi.SampleDataGenerator.Console.Config;
using EdFi.SampleDataGenerator.Core.Config.Xml;
using EdFi.SampleDataGenerator.Core.DataGeneration.Coordination;
using log4net;
using log4net.Config;

namespace EdFi.SampleDataGenerator.Console
{
    class Program
    {
        private static ILog _log;

        static void Main(string[] args)
        {
            PrintCopyrightMessageToConsole();

            InitializeApp();

            try
            {
                var commandLineConfig = ParseCommandLine(args);

                Directory.CreateDirectory(commandLineConfig.OutputPath);

                var sampleDataGeneratorConfig = LoadXmlConfig(commandLineConfig);
                Run(sampleDataGeneratorConfig);
            }
            catch (Exception e)
            {
                _log.Fatal(e);
            }

#if DEBUG
            System.Console.Write("Press any key to continue...");
            System.Console.ReadKey();
#endif
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

        private static void InitializeApp()
        {
            BasicConfigurator.Configure();
            _log = LogManager.GetLogger(typeof (Program));
        }

        private static SampleDataGeneratorConsoleConfig ParseCommandLine(string[] args)
        {
            var parser = new CommandLineParser();
            var parseResult = parser.Parse(args);

            if (parseResult.HasErrors)
            {
                throw new Exception(parseResult.ErrorText);
            }

            ValidateCommandLineConfig(parser.Object);

            return parser.Object;
        }

        private static void ValidateCommandLineConfig(SampleDataGeneratorConsoleConfig config)
        {
            var validator = new CommandLineValidator();
            var validationResult = validator.Validate(config);

            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    _log.Error(error);
                }

                throw new Exception("Invalid command line arguments");
            }
        }

        private static SampleDataGeneratorConfig LoadXmlConfig(SampleDataGeneratorConsoleConfig commandLineConfig)
        {
            try
            {
                var config = XmlConfigHelpers.ParseConfigFileToObject<SampleDataGeneratorConfig>(commandLineConfig.ConfigXmlPath);
                config.DataFilePath = commandLineConfig.DataFilePath;
                config.OutputPath = commandLineConfig.OutputPath;
                config.SeedFilePath = commandLineConfig.SeedFilePath;
                config.OutputMode = commandLineConfig.OutputMode;
                config.CreatePerformanceFile = commandLineConfig.CreatePerformanceFile;

                var validator = new SampleDataGeneratorConfigValidator();

                if (ShouldScanForDataFiles(config))
                {
                    var dataFileScanner = new DataFileScanner();
                    dataFileScanner.ScanForDataFiles(config);
                }

                var validationErrors = validator.Validate(config);
                if (validationErrors.Any())
                    throw new Exception("Invalid configuration.");

                return config;
            }
            catch (Exception e)
            {
                throw new Exception("Error when trying to read config file", e);
            }
        }

        private static bool ShouldScanForDataFiles(SampleDataGeneratorConfig parsedConfig)
        {
            return !string.IsNullOrEmpty(parsedConfig.DataFilePath);
        }

        private static void Run(SampleDataGeneratorConfig config)
        {
            var sampleDataGenerator = new SampleDataGenerationService();
            sampleDataGenerator.Run(config);
        }
    }
}

