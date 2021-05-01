using EdFi.SampleDataGenerator.Core.Config;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common;

namespace EdFi.SampleDataGenerator.Core.UnitTests.Config
{
    public class TestSampleDataGeneratorConfig : ISampleDataGeneratorConfig
    {
        public int? BatchSize { get; set; }
        public string DataFilePath { get; set; }
        public string OutputPath { get; set; }
        public string SeedFilePath { get; set; }
        public OutputMode OutputMode { get; set; }
        public bool CreatePerformanceFile { get; set; }
        public ITimeConfig TimeConfig { get; set; }
        public IDistrictProfile[] DistrictProfiles { get; set; }
        public IStudentProfile[] StudentProfiles { get; set; }
        public IDataFileConfig DataFileConfig { get; set; }
        public IEthnicityMapping[] EthnicityMappings { get; set; }
        public IGenderMapping[] GenderMappings { get; set; }
        public IStudentPopulationProfile[] StudentPopulationProfiles { get; set; }
        public IGraduationPlanTemplate[] GraduationPlanTemplates { get; set; }
        public IMutatorConfigurationCollection MutatorConfig { get; set; }
        public IStudentGradeRange[] StudentGradeRanges { get; set; }
    }
}
