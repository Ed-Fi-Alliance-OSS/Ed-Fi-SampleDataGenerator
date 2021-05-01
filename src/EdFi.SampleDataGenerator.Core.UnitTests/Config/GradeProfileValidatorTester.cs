using EdFi.SampleDataGenerator.Core.Config;
using EdFi.SampleDataGenerator.Core.Entities;
using NUnit.Framework;

namespace EdFi.SampleDataGenerator.Core.UnitTests.Config
{
    [TestFixture]
    public class GradeProfileValidatorTester : ValidatorTestBase<GradeProfileValidator, IGradeProfile>
    {
        public GradeProfileValidatorTester() : base(new GradeProfileValidator("Test School"))
        {
        }

        [Test]
        public void ShouldPassValidGradeProfile()
        {
            var profile = GetValidGradeProfile();
            Validate(profile, true);
        }

        [Test]
        public void ShouldFailInvalidGradeName()
        {
            var profile = GetValidGradeProfile();
            profile.GradeName = "Frist grade";
            Validate(profile, false);
        }

        [Test]
        public void ShouldFailNonK12GradeName()
        {
            var profile = GetValidGradeProfile();
            profile.GradeName = GradeLevelDescriptor.Ungraded.CodeValue;
            Validate(profile, false);
        }

        private static TestGradeProfile GetValidGradeProfile()
        {
            return new TestGradeProfile
            {
                GradeName = "First grade",
                InitialStudentCount = 1,
                StudentPopulationProfiles = new[]
                {
                    new TestStudentPopulationProfile
                    {
                        StudentProfileReference = "Test Student Profile",
                        InitialStudentCount = 100
                    }
                },
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
