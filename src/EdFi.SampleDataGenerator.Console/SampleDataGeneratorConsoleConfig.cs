using EdFi.SampleDataGenerator.Core.DataGeneration.Common;

namespace EdFi.SampleDataGenerator.Console
{
    public class SampleDataGeneratorConsoleConfig
    {
        public string ConfigXmlPath { get; set; }
        public string DataFilePath { get; set; }
        public string OutputPath { get; set; }
        public string SeedFilePath { get; set; }
        public OutputMode OutputMode { get; set; }
        public bool AllowOverwrite { get; set; }
        public bool CreatePerformanceFile { get; set; }
    }
}