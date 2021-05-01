using EdFi.SampleDataGenerator.Core.DataGeneration.Generators;
using EdFi.SampleDataGenerator.Core.UnitTests.Config;
using NUnit.Framework;
using Shouldly;
using static EdFi.SampleDataGenerator.Core.DataGeneration.Common.StudentPerformanceProfileDistribution;

namespace EdFi.SampleDataGenerator.Core.UnitTests.DataGeneration.Generators
{
    [TestFixture]
    public class StudentPerformanceProfileTester
    {
        private readonly StudentPerformanceProfile student89 = StudentPerformanceByPercentil(0.89);
        private readonly StudentPerformanceProfile student90 = StudentPerformanceByPercentil(0.90);
        private readonly StudentPerformanceProfile student91 = StudentPerformanceByPercentil(0.91);
        private readonly StudentPerformanceProfile student92 = StudentPerformanceByPercentil(0.92);

        private static StudentPerformanceProfile StudentPerformanceByPercentil(double percentile)
        {
            return new StudentPerformanceProfile
            {
                PerformanceIndex = GetStudentPerformanceProfileFromPercentile(percentile)
            };
        }

        [Test]
        public void TheStudentIsHighPerformingWhenTheyMeetTheConfiguredPercentileForTheDistrict()
        {
            var config = new StudentDataGeneratorConfig
            {
                DistrictProfile = new TestDistrictProfile()
            };

            config.DistrictProfile = new TestDistrictProfile {HighPerformingStudentPercentile = 0.90};
            student89.IsStudentHighPerforming(config).ShouldBeFalse();
            student90.IsStudentHighPerforming(config).ShouldBeTrue();
            student91.IsStudentHighPerforming(config).ShouldBeTrue();
            student92.IsStudentHighPerforming(config).ShouldBeTrue();

            config.DistrictProfile = new TestDistrictProfile {HighPerformingStudentPercentile = 0.91};
            student89.IsStudentHighPerforming(config).ShouldBeFalse();
            student90.IsStudentHighPerforming(config).ShouldBeFalse();
            student91.IsStudentHighPerforming(config).ShouldBeTrue();
            student92.IsStudentHighPerforming(config).ShouldBeTrue();
        }

        [Test]
        public void TheStudentIsHighPerformingWhenTheDistrictPercentileIsNotConfiguredButTheyMeetTheDefaultPercentile()
        {
            var config = new StudentDataGeneratorConfig
            {
                DistrictProfile = new TestDistrictProfile
                {
                    HighPerformingStudentPercentile = null
                }
            };

            student89.IsStudentHighPerforming(config).ShouldBeFalse();
            student90.IsStudentHighPerforming(config).ShouldBeTrue();
            student91.IsStudentHighPerforming(config).ShouldBeTrue();
            student92.IsStudentHighPerforming(config).ShouldBeTrue();
        }

        [Test]
        public void HighPerformingHighSchoolStudentsAreElibleForTheGiftedAndTalentedProgram()
        {
            var config = new StudentDataGeneratorConfig
            {
                DistrictProfile = new TestDistrictProfile
                {
                    HighPerformingStudentPercentile = 0.90
                },
                GradeProfile = new TestGradeProfile()
            };

            config.GradeProfile = new TestGradeProfile {GradeName = "Eighth grade"};
            student89.IsEligibleForGiftedAndTalentedProgram(config).ShouldBeFalse();
            student90.IsEligibleForGiftedAndTalentedProgram(config).ShouldBeFalse();

            config.GradeProfile = new TestGradeProfile {GradeName = "Ninth grade"};
            student89.IsEligibleForGiftedAndTalentedProgram(config).ShouldBeFalse();
            student90.IsEligibleForGiftedAndTalentedProgram(config).ShouldBeTrue();

            config.GradeProfile = new TestGradeProfile {GradeName = "Tenth grade"};
            student89.IsEligibleForGiftedAndTalentedProgram(config).ShouldBeFalse();
            student90.IsEligibleForGiftedAndTalentedProgram(config).ShouldBeTrue();

            config.GradeProfile = new TestGradeProfile {GradeName = "Eleventh grade"};
            student89.IsEligibleForGiftedAndTalentedProgram(config).ShouldBeFalse();
            student90.IsEligibleForGiftedAndTalentedProgram(config).ShouldBeTrue();

            config.GradeProfile = new TestGradeProfile {GradeName = "Postsecondary"};
            student89.IsEligibleForGiftedAndTalentedProgram(config).ShouldBeFalse();
            student90.IsEligibleForGiftedAndTalentedProgram(config).ShouldBeFalse();
        }
    }
}
