using System;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common;
using NUnit.Framework;
using Shouldly;

namespace EdFi.SampleDataGenerator.Core.UnitTests.DataGeneration.Common
{
    [TestFixture]
    public class StudentPerformanceProfileDistributionTester
    {
        [Test]
        [TestCase(0.25, StudentPerformanceProfileDistribution.BottomQuartile)]
        [TestCase(0.50, StudentPerformanceProfileDistribution.FiftiethPercentile)]
        [TestCase(0.75, StudentPerformanceProfileDistribution.ThirdQuartile)]
        public void ShouldGetPerformanceIndexBasedonPercentile(double percentile, double expectedResult)
        {
            StudentPerformanceProfileDistribution.GetStudentPerformanceProfileFromPercentile(percentile).ShouldBe(expectedResult, tolerance: 0.001);
        }

        [Test]
        public void GetPerformanceIndexByPercentileShouldThrowWhenPercentileIsZero()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                StudentPerformanceProfileDistribution.GetStudentPerformanceProfileFromPercentile(0.0);
            });
        }

        [Test]
        public void GetPerformanceIndexByPercentileShouldThrowWhenPercentileIsOne()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                StudentPerformanceProfileDistribution.GetStudentPerformanceProfileFromPercentile(1.0);
            });
        }

        [Test]
        [TestCase(StudentPerformanceProfileDistribution.BottomQuartile, 0.25)]
        [TestCase(StudentPerformanceProfileDistribution.FiftiethPercentile, 0.50)]
        [TestCase(StudentPerformanceProfileDistribution.ThirdQuartile, 0.75)]
        public void ShouldGetStudentPercentileFromPerformanceProfile(double performanceProfileIndex, double expectedResult)
        {
            StudentPerformanceProfileDistribution.GetStudentPercentileFromPerformanceProfile(performanceProfileIndex).ShouldBe(expectedResult, tolerance: 0.001);
        }

        [Test]
        public void PerformanceIndexMethodsShouldBeInverseFunctions()
        {
            for (var i = 1; i < 100; ++i)
            {
                var performanceProfileIndex = i / 100.0;
                var percentile = StudentPerformanceProfileDistribution.GetStudentPercentileFromPerformanceProfile(performanceProfileIndex);
                var calculatedPerformanceProfileIndex = StudentPerformanceProfileDistribution.GetStudentPerformanceProfileFromPercentile(percentile);

                calculatedPerformanceProfileIndex.ShouldBe(performanceProfileIndex, tolerance: 0.001);
            }
        }
    }
}
