using EdFi.SampleDataGenerator.Core.Config;
using EdFi.SampleDataGenerator.Core.UnitTests.DataGeneration;
using NUnit.Framework;

namespace EdFi.SampleDataGenerator.Core.UnitTests.Config
{
    [TestFixture]
    public class SchoolProfileValidatorTester : ValidatorTestBase<SchoolProfileValidator, ISchoolProfile>
    {
        public SchoolProfileValidatorTester() : base(new SchoolProfileValidator(GeneratorTestBase.GetSampleDataGeneratorConfig()))
        {
        }

        [Test]
        public void ShouldPassValidSchoolProfile()
        {
            var profile = GetValidTestSchoolProfile();
            Validate(profile, true);
        }
        
        [Test]
        public void ShouldFailSchoolProfileWithEmptyName()
        {
            var profile = GetValidTestSchoolProfile();
            profile.SchoolName = "";

            Validate(profile, false);
        }

        [Test]
        public void ShouldFailSchoolProfileWithEmptyGradeProfiles()
        {
            var profile = GetValidTestSchoolProfile();
            profile.GradeProfiles = new IGradeProfile[] {};

            Validate(profile, false);
        }

        [Test]
        public void ShouldFailSchoolProfileWithInvalidGradeProfiles()
        {
            var profile = GetValidTestSchoolProfile();
            profile.GradeProfiles[0] = GetTestGradeProfile("Frist Grade");

            Validate(profile, false);
        }

        [Test]
        public void ShouldFailSchoolProfileWithNoStudents()
        {
            var profile = GetValidTestSchoolProfile();
            profile.InitialStudentCount = 0;

            Validate(profile, false);
        }

        [Test]
        public void ShouldFailSchoolProfileWithNoDisciplineProfile()
        {
            var profile = GetValidTestSchoolProfile();
            profile.DisciplineProfile = null;

            Validate(profile, false);
        }

        [Test]
        public void ShouldFailSchoolProfileWithInvalidDisciplineProfile()
        {
            var profile = GetValidTestSchoolProfile();
            profile.DisciplineProfile = new TestDisciplineProfile
            {
                TotalExpectedSeriousDisciplineEvents = 0,
                TotalExpectedDisciplineEvents = 0
            };

            Validate(profile, false);
        }

        [Test]
        public void ShouldFailWhenSchoolsHaveNamesWithFileSystemUnsafeCharacters()
        {
            var profile = GetValidTestSchoolProfile();
            profile.SchoolName = "T>e>s\\t";

            ShouldFailValidation(profile,
                "SchoolProfile SchoolName 'T>e>s\\t' must be safe for use in filenames. Invalid characters: >\\");
        }

        private static TestSchoolProfile GetValidTestSchoolProfile()
        {
            return new TestSchoolProfile
            {
                GradeProfiles = new[]
                {
                    GetTestGradeProfile()
                },
                SchoolName = "Test",
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
            };
        }

        private static TestGradeProfile GetTestGradeProfile(string gradeName = "First grade")
        {
            return new TestGradeProfile
            {
                GradeName = gradeName,
                StudentPopulationProfiles = new []{ new TestStudentPopulationProfile(), },
                InitialStudentCount = 1,
                AssessmentParticipationConfigurations = new[]
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
            };
        }
    }
}
