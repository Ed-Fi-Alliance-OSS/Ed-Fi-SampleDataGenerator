using System;
using System.IO;
using EdFi.InterchangeXmlToCsv.Console.Xml;
using EdFi.SampleDataGenerator.Core.Serialization.CsvHelper;
using log4net;

namespace EdFi.InterchangeXmlToCsv.Console
{
    public class InterchangeXmlToCsvConverter
    {
        private static readonly ILog Logger = LogManager.GetLogger(nameof(InterchangeXmlToCsvConverter));

        public void Convert(InterchangeXmlToCsvConfig config)
        {
            if (Directory.Exists(config.InputPath))
            {
                var files = Directory.EnumerateFiles(config.InputPath, "*.xml", config.Recurse ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);
                foreach (var file in files)
                {
                    ConvertFile(config, file);
                }
            }

            else if (File.Exists(config.InputPath))
            {
                ConvertFile(config, config.InputPath);
            }

            else
            {
                throw new IOException($"'{config.InputPath}' is not a valid path");
            }
        }

        private void ConvertFile(InterchangeXmlToCsvConfig config, string inputFileName)
        {
            var xmlReader = InterchangeXmlReaderFactory.BuildXmlReader(config.InterchangeName);
            var interchangeItems = xmlReader.ReadFile(inputFileName);

            var outputDirectory = GetOutputDirectory(config.InterchangeName, config.OutputPath);

            foreach (var outputType in interchangeItems.GetEntityTypesInCollection())
            {
                var map = CsvClassMapRegistry.MapFor(outputType);
                var outputFilePath = GetOutputFilePath(outputType, outputDirectory);
                var entitiesToOutput = interchangeItems[outputType];

                Logger.Info($"Saving data for type {outputType.Name} to {outputFilePath}");
                
                using (var sw = new StreamWriter(outputFilePath))
                {
                    if (map != null)
                    {
                        var csvWriter = new InterchangeCsvWriter(map, sw);
                        csvWriter.WriteRecords(entitiesToOutput);
                    }

                    else
                    {
                        Logger.Warn($"No CSV mapping defined for type {outputType.Name}.  Output may not be correct.");

                        var csvWriter = new CsvHelper.CsvWriter(sw);
                        csvWriter.WriteRecords(entitiesToOutput);
                    }
                }
            }
        }

        private string GetOutputDirectory(string interchangeName, string outputPath)
        {
            var directoryPath = Path.Combine(outputPath, interchangeName);
            Directory.CreateDirectory(directoryPath);

            return directoryPath;
        }

        private string GetOutputFilePath(Type outputType, string outputPath)
        {   
            var outputFileName = $"{outputType.Name}.csv";
            return Path.Combine(outputPath, outputFileName);
        }
    }
}
