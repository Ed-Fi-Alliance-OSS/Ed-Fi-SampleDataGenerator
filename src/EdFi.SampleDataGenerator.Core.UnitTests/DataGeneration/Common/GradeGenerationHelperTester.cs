using System;
using System.Linq;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common;
using NUnit.Framework;
using Shouldly;

namespace EdFi.SampleDataGenerator.Core.UnitTests.DataGeneration.Common
{
    [TestFixture]
    public class GradeGenerationHelperTester
    {
        private const int TestIterationCount = 1000;

        [Test]
        public void ShouldThrowWhenAverageOutsideGradeRange()
        {
            var generatedGradeRange = GradeRange.NoFailures;

            Assert.Throws<ArgumentOutOfRangeException>(
                () =>
                {
                    var result = GradeGenerationHelpers.GenerateGradesFromGradePointAverage(generatedGradeRange.MinPossibleGrade - 1, 1, new RandomNumberGenerator(), generatedGradeRange).ToList();
                });

            Assert.Throws<ArgumentOutOfRangeException>(
                () =>
                {
                    var result = GradeGenerationHelpers.GenerateGradesFromGradePointAverage(generatedGradeRange.MaxPossibleGrade + 1, 1, new RandomNumberGenerator(), generatedGradeRange).ToList();
                });
        }

        [Test]
        public void ShouldThrowWhenTotalGradesLessThanZero()
        {
            var generatedGradeRange = GradeRange.NoFailures;
            Assert.Throws<ArgumentOutOfRangeException>(
                () =>
                {
                    var result = GradeGenerationHelpers.GenerateGradesFromGradePointAverage(generatedGradeRange.MinPossibleGrade, -1, new RandomNumberGenerator(), generatedGradeRange).ToList();
                });
        }

        [Test]
        public void ShouldReturnEmptyEnumerableWhenTotalGradesEqualsZero()
        {
            var generatedGradeRange = GradeRange.NoFailures;
            var result = GradeGenerationHelpers.GenerateGradesFromGradePointAverage(generatedGradeRange.MinPossibleGrade, 0, new RandomNumberGenerator(), generatedGradeRange);
            result.Count().ShouldBe(0);
        }

        [Test]
        public void ShouldThrowWhenRandomNumberGeneratorNull()
        {
            var generatedGradeRange = GradeRange.NoFailures;
            Assert.Throws<ArgumentNullException>(
                () =>
                {
                    var result = GradeGenerationHelpers.GenerateGradesFromGradePointAverage(generatedGradeRange.MinPossibleGrade, 1, null, generatedGradeRange).ToList();
                });
        }


        [Test]
        public void GeneratedGradesShouldAverageCorrectly()
        {
            var randomNumberGenerator = new RandomNumberGenerator();

            //Since we have to test with actual random number generation, the results of this test
            //are not deterministic. However, over a large enough set of runs, we can reasonably
            //expect that if there was an error, the assertion below would fail.
            //we'll essentially test the entire expected data generation space
            for (var targetGradeAverage = 0; targetGradeAverage <= 100; ++targetGradeAverage)
            {
                for (var totalGradesToGenerate = 4; totalGradesToGenerate <= 8; ++totalGradesToGenerate)
                {
                    for (var i = 0; i < TestIterationCount; ++i)
                    {
                        var grades = GradeGenerationHelpers.GenerateGradesFromGradePointAverage(targetGradeAverage, totalGradesToGenerate, randomNumberGenerator, GradeRange.FullGradingScale).ToList();

                        var expectedGradePoints = targetGradeAverage * totalGradesToGenerate;
                        var actualGradePoints = grades.Sum();
                        var actualGradesGenerated = grades.Count;

                        actualGradesGenerated.ShouldBe(totalGradesToGenerate);

                        actualGradePoints.ShouldBe(expectedGradePoints, () =>
                            $"Expected an average of {targetGradeAverage} over {totalGradesToGenerate} grades generated, " +
                            $"but got an average of {actualGradePoints / (1.0 * actualGradesGenerated)} over {actualGradesGenerated} grades generated. "
                        );
                    }
                }
            }
        }

        [Test]
        public void ShouldNotGenerateGradesOutsideProvidedRange()
        {
            var randomNumberGenerator = new RandomNumberGenerator();
            var generatedGradeRange = GradeRange.NoFailures;

            //Since we have to test with actual random number generation, the results of this test
            //are not deterministic. However, over a large enough set of runs, we can reasonably
            //expect that if there was an error, the assertion below would fail.
            //we'll essentially test the entire expected data generation space
            for (var targetGradeAverage = generatedGradeRange.MinPossibleGrade; targetGradeAverage <= generatedGradeRange.MaxPossibleGrade; ++targetGradeAverage)
            {
                for (var totalGradesToGenerate = 4; totalGradesToGenerate <= 8; ++totalGradesToGenerate)
                {
                    for (var i = 0; i < TestIterationCount; ++i)
                    {
                        var grades = GradeGenerationHelpers.GenerateGradesFromGradePointAverage(targetGradeAverage, totalGradesToGenerate, randomNumberGenerator, generatedGradeRange).ToList();

                        var actualMinGrade = grades.Min();
                        var actualMaxGrade = grades.Max();

                        actualMinGrade.ShouldBeGreaterThanOrEqualTo(generatedGradeRange.MinPossibleGrade);
                        actualMaxGrade.ShouldBeLessThanOrEqualTo(generatedGradeRange.MaxPossibleGrade);
                    }
                }
            }
        }
    }
}
