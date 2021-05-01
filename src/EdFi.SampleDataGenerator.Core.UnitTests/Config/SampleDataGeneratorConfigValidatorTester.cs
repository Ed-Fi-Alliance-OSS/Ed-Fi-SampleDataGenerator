using System;
using System.Collections.Generic;
using EdFi.SampleDataGenerator.Core.Config;
using NUnit.Framework;

namespace EdFi.SampleDataGenerator.Core.UnitTests.Config
{
    [TestFixture]
    public class SampleDataGeneratorConfigValidatorTester : ValidatorTestBase<SampleDataGeneratorConfigValidator, ISampleDataGeneratorConfig>
    {
        public SampleDataGeneratorConfigValidatorTester() : base(new SampleDataGeneratorConfigValidator())
        {
        }

        [Test]
        public void ShouldPassWithValidConfig()
        {
            var config = GetValidConfig();
            Validate(config, true);
        }

        [Test]
        public void ShouldFailWithEmptyStudentProfiles()
        {
            var config = GetValidConfig();
            config.StudentProfiles = new IStudentProfile[] { };

            Validate(config, false);
        }

        [Test]
        public void ShouldFailWithEmptyEthnicityMappings()
        {
            var config = GetValidConfig();
            config.EthnicityMappings = new IEthnicityMapping[] { };

            Validate(config, false);
        }

        [Test]
        public void ShouldFailWithEmptyGenderMappings()
        {
            var config = GetValidConfig();
            config.GenderMappings = new IGenderMapping[] { };

            Validate(config, false);
        }

        private static TestSampleDataGeneratorConfig GetValidConfig()
        {
            return new TestSampleDataGeneratorConfig
            {
                BatchSize = 100,
                DataFilePath = "TestPath",
                TimeConfig = new TestTimeConfig
                {
                    DataClockConfig = TestDataClockConfig.Default,
                    SchoolCalendarConfig = TestSchoolCalendarConfig.Default
                },
                DataFileConfig = new TestDataFileConfig
                {
                    FirstNameFiles = new[] { new TestFirstNameFileMapping(), },
                    StreetNameFile = new TestStreetNameFileMapping(),
                    SurnameFiles = new[] { new TestSurnameFileMapping(), }
                },
                DistrictProfiles = new[]
                {
                    new TestDistrictProfile
                    {
                        DistrictName = "Test District",
                        LocationInfo = new TestLocationInfo
                        {
                            Cities = new []
                            {
                                new TestCity
                                {
                                    AreaCodes = new []{ new TestAreaCode { Value = 123 } },
                                    Name = "Test City",
                                    PostalCodes = new []{ new TestPostalCode { Value = "12345" } },
                                    County = "Test"
                                }
                            },
                            State = "TX"
                        },
                        SchoolProfiles = new []
                        {
                            new TestSchoolProfile
                            {
                                GradeProfiles = new []
                                {
                                    new TestGradeProfile
                                    {
                                        GradeName = "First grade",
                                        StudentPopulationProfiles = new []
                                        {
                                            new TestStudentPopulationProfile
                                            {
                                                StudentProfileReference = "Test Profile",
                                                InitialStudentCount = 1
                                            }
                                        },
                                        InitialStudentCount = 1,
                                        AssessmentParticipationConfigurations = new []
                                        {
                                            new TestAssessmentParticipationConfiguration
                                            {
                                                AssessmentTitle = "STATE Reading",
                                                ParticipationRates = new []
                                                {
                                                    new TestAssessmentParticipationRate
                                                    {
                                                        LowerPerformancePercentile = 0,
                                                        UpperPerformancePercentile = 1,
                                                        Probability = 1
                                                    }
                                                }
                                            },
                                        }
                                    }
                                },
                                DisciplineProfile = new TestDisciplineProfile
                                {
                                    TotalExpectedDisciplineEvents = 1,
                                    TotalExpectedSeriousDisciplineEvents = 1
                                },
                                StaffProfile = TestStaffProfile.Default,
                                SchoolName = "Test School",
                                InitialStudentCount = 1,
                                CourseLoad = 8
                            }
                        }
                    },
                },
                EthnicityMappings = TestEthnicityMapping.Defaults,
                GenderMappings = TestGenderMapping.Defaults,
                StudentPopulationProfiles = null,
                StudentProfiles = new[]
                {
                    new TestStudentProfile
                    {
                        Name = "Test Profile",
                        RaceConfiguration = new TestAttributeConfiguration
                        {
                            Name = "Race",
                            AttributeGeneratorConfigurationOptions = new []
                            {
                                new TestAttributeGeneratorConfigurationOption
                                {
                                    Frequency = 1.00,
                                    Value = "Asian"
                                }
                            }
                        },
                        SexConfiguration = new TestAttributeConfiguration
                        {
                            Name = "Sex",
                            AttributeGeneratorConfigurationOptions = new []
                            {
                                new TestAttributeGeneratorConfigurationOption
                                {
                                    Frequency = 0.50,
                                    Value = "Male"
                                },
                                new TestAttributeGeneratorConfigurationOption
                                {
                                    Frequency = 0.50,
                                    Value = "Female"
                                }
                            }
                        }
                    }
                },
                StudentGradeRanges = TestStudentGradeRange.Defaults
            };
        }
    }
}
