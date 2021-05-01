using EdFi.SampleDataGenerator.Core.Config;
using EdFi.SampleDataGenerator.Core.UnitTests.DataGeneration;
using NUnit.Framework;

namespace EdFi.SampleDataGenerator.Core.UnitTests.Config
{
    [TestFixture]
    public class DistrictProfileValidatorTester : ValidatorTestBase<DistrictProfileValidator, IDistrictProfile>
    {
        public DistrictProfileValidatorTester() : base(new DistrictProfileValidator(GeneratorTestBase.GetSampleDataGeneratorConfig()))
        {
        }

        [Test]
        public void ShouldPassValidDistrictProfile()
        {
            var profile = GetValidTestDistrictProfile();
            Validate(profile, true);
        }

        [Test]
        public void ShouldFailDistrictProfileWithEmtpySchoolProfiles()
        {
            var profile = GetValidTestDistrictProfile();
            profile.SchoolProfiles = new ISchoolProfile[] {};

            Validate(profile, false);
        }

        private static TestDistrictProfile GetValidTestDistrictProfile()
        {
            return new TestDistrictProfile
            {
                DistrictName = "Test",
                LocationInfo = new TestLocationInfo
                {
                    Cities = new[]
                    {
                        new TestCity
                        {
                            AreaCodes = new[]
                            {
                                new TestAreaCode
                                {
                                    Value = 123
                                }
                            },
                            Name = "Test City",
                            County = "Test County",
                            PostalCodes = new []
                            {
                                new TestPostalCode
                                {
                                    Value = "123456"
                                }
                            }
                        }
                    },
                    State = "TX"
                },
                SchoolProfiles = new[]
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
                                        StudentProfileReference = "Test Student Profile",
                                        InitialStudentCount = 1
                                    }
                                },
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
                        SchoolName = "Test School",
                        StaffProfile = new TestStaffProfile
                        {
                            StaffRaceConfiguration = new TestAttributeConfiguration
                            {
                                Name = "Race",
                                AttributeGeneratorConfigurationOptions = new[]
                                {
                                    new TestAttributeGeneratorConfigurationOption
                                    {
                                        Frequency = 1.00,
                                        Value = "White"
                                    }
                                }
                            },
                            StaffSexConfiguration = new TestAttributeConfiguration()
                            {
                                Name = "Sex",
                                AttributeGeneratorConfigurationOptions = new[]
                                {
                                    new TestAttributeGeneratorConfigurationOption
                                    {
                                        Frequency = 1.00,
                                        Value = "Male"
                                    }
                                }
                            }
                        },
                        DisciplineProfile = new TestDisciplineProfile
                        {
                            TotalExpectedDisciplineEvents = 1,
                            TotalExpectedSeriousDisciplineEvents = 1
                        },
                        InitialStudentCount = 1,
                        CourseLoad = 8
                    }
                }
            };
        }
    }
}
