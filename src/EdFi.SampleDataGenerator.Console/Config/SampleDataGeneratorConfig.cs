using System.Xml.Serialization;
using EdFi.SampleDataGenerator.Core.Config;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common;
using EdFi.SampleDataGenerator.Core.Entities;

namespace EdFi.SampleDataGenerator.Console.Config
{
    [XmlRoot]
    public class SampleDataGeneratorConfig : ISampleDataGeneratorConfig
    {
        public string DataFilePath { get; set; }
        public string OutputPath { get; set; }
        public string SeedFilePath { get; set; }
        public OutputMode OutputMode { get; set; }
        public bool CreatePerformanceFile { get; set; }

        [XmlElement]
        public int? BatchSize { get; set; }

        [XmlElement("TimeConfig")]
        public TimeConfig TimeConfig { get; set; }
        ITimeConfig ISampleDataGeneratorConfig.TimeConfig => TimeConfig;

        [XmlElement("DistrictProfile")]
        public DistrictProfile[] DistrictProfiles { get; set; }
        IDistrictProfile[] ISampleDataGeneratorConfig.DistrictProfiles => DistrictProfiles;

        [XmlElement("StudentProfile")]
        public StudentProfile[] StudentProfiles { get; set; }
        IStudentProfile[] ISampleDataGeneratorConfig.StudentProfiles => StudentProfiles;

        [XmlElement]
        public DataFileConfig DataFileConfig { get; set; } = new DataFileConfig();
        IDataFileConfig ISampleDataGeneratorConfig.DataFileConfig => DataFileConfig;

        [XmlIgnore]
        public EthnicityMapping[] EthnicityMappings { get; set; } =
        {
            new EthnicityMapping {Ethnicity = "AmericanIndianAlaskanNative", EdFiRaceType = RaceDescriptor.AmericanIndianAlaskaNative.CodeValue},
            new EthnicityMapping {Ethnicity = "Asian", EdFiRaceType = RaceDescriptor.Asian.CodeValue},
            new EthnicityMapping {Ethnicity = "Black", EdFiRaceType = RaceDescriptor.BlackAfricanAmerican.CodeValue},
            new EthnicityMapping {Ethnicity = "Hispanic", EdFiRaceType = RaceDescriptor.White.CodeValue, HispanicLatinoEthnicity = true},
            new EthnicityMapping {Ethnicity = "NativeHawaiinPacificIslander", EdFiRaceType = RaceDescriptor.NativeHawaiianPacificIslander.CodeValue},
            new EthnicityMapping {Ethnicity = "White", EdFiRaceType = RaceDescriptor.White.CodeValue}
        };
        IEthnicityMapping[] ISampleDataGeneratorConfig.EthnicityMappings => EthnicityMappings;

        [XmlIgnore]
        public GenderMapping[] GenderMappings { get; set; } = 
        {
            new GenderMapping { EdFiGender = SexDescriptor.Male.CodeValue, Gender = "Male" },
            new GenderMapping { EdFiGender = SexDescriptor.Female.CodeValue, Gender = "Female" }
        };
        IGenderMapping[] ISampleDataGeneratorConfig.GenderMappings => GenderMappings;

        
        [XmlElement(typeof(StudentPopulationProfile))]
        public StudentPopulationProfile[] StudentPopulationProfiles { get; set; }
        IStudentPopulationProfile[] ISampleDataGeneratorConfig.StudentPopulationProfiles => StudentPopulationProfiles;

        [XmlElement("GraduationPlanTemplate")]
        public GraduationPlanTemplate[] GraduationPlanTemplates { get; set; }
        IGraduationPlanTemplate[] ISampleDataGeneratorConfig.GraduationPlanTemplates => GraduationPlanTemplates;

        [XmlElement("MutatorConfig")]
        public MutatorConfigurationCollection MutatorConfig { get; set; }
        IMutatorConfigurationCollection ISampleDataGeneratorConfig.MutatorConfig => MutatorConfig;

        [XmlIgnore]
        public IStudentGradeRange[] StudentGradeRanges { get; set; } =
        {
            new StudentGradeRange { LowerPerformanceIndex = StudentPerformanceProfileDistribution.GetStudentPerformanceProfileFromPercentile(0.81), UpperPerformanceIndex = 1, MinNumericGrade =90, MaxNumericGrade = 100},
            new StudentGradeRange { LowerPerformanceIndex = StudentPerformanceProfileDistribution.GetStudentPerformanceProfileFromPercentile(0.51), UpperPerformanceIndex = StudentPerformanceProfileDistribution.GetStudentPerformanceProfileFromPercentile(0.81), MinNumericGrade =80, MaxNumericGrade = 89},
            new StudentGradeRange { LowerPerformanceIndex = StudentPerformanceProfileDistribution.GetStudentPerformanceProfileFromPercentile(0.21), UpperPerformanceIndex = StudentPerformanceProfileDistribution.GetStudentPerformanceProfileFromPercentile(0.51), MinNumericGrade =70, MaxNumericGrade = 79},
            new StudentGradeRange { LowerPerformanceIndex = StudentPerformanceProfileDistribution.GetStudentPerformanceProfileFromPercentile(0.10), UpperPerformanceIndex = StudentPerformanceProfileDistribution.GetStudentPerformanceProfileFromPercentile(0.21), MinNumericGrade =60, MaxNumericGrade = 69},
            new StudentGradeRange { LowerPerformanceIndex = 0, UpperPerformanceIndex = StudentPerformanceProfileDistribution.GetStudentPerformanceProfileFromPercentile(0.10), MinNumericGrade =0, MaxNumericGrade = 59},
        };
        IStudentGradeRange[] ISampleDataGeneratorConfig.StudentGradeRanges => StudentGradeRanges;
    }
}
