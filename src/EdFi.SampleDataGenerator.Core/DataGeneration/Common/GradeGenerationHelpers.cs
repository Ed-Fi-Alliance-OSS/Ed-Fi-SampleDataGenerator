using System;
using System.Collections.Generic;

namespace EdFi.SampleDataGenerator.Core.DataGeneration.Common
{
    public static class GradeGenerationHelpers
    {
        /// <summary>
        /// Generates a set of grades on the 100 point grading scale that average to the provided GPA
        /// </summary>
        /// <param name="targetGradeAverage">Target grade average for the set of grades returned</param>
        /// <param name="totalGradesToGenerate">Number of grades to generate</param>
        /// <param name="randomNumberGenerator">Instance of an IRandomNumberGenerator object</param>
        /// <param name="generatedGradeRange">Range of grades to generate (e.g. 60 - 100 to avoid assigning Fs on a 100 point scale)</param>
        /// <param name="maxGradeVariance">Maximum acceptable variance of a single grade from the average</param>
        /// <returns>A set of grades that, when averaged, meet the provided target grade average.</returns>
        public static IEnumerable<int> GenerateGradesFromGradePointAverage(int targetGradeAverage, int totalGradesToGenerate, IRandomNumberGenerator randomNumberGenerator,
            GradeRange generatedGradeRange, int maxGradeVariance = 6)
        {
            if (targetGradeAverage < generatedGradeRange.MinPossibleGrade || targetGradeAverage > generatedGradeRange.MaxPossibleGrade)
                throw new ArgumentOutOfRangeException(nameof(targetGradeAverage), $"Value must be within provided GradeRange ({generatedGradeRange.MinPossibleGrade} and {generatedGradeRange.MaxPossibleGrade}, inclusive)");

            if (totalGradesToGenerate < 0)
                throw new ArgumentOutOfRangeException(nameof(totalGradesToGenerate), "Value must be greater than  or equal to 0");

            if (randomNumberGenerator == null)
                throw new ArgumentNullException(nameof(randomNumberGenerator));

            if (totalGradesToGenerate == 0)
                yield break;

            var sumOfGradeVariances = 0;

            //We'll inject some variance to a student's individual grades to help generate more realistic-looking data.
            //We already have the student's average grade from their performance profile, and we need their
            //individual grades to average to this number.  So, we'll generate some random variance about that average 
            //for each individual grade, and set up the final grade to ensure we meet the overall average.
            //
            //This is accomplished by keeping a running total of variances and subtracting that total variance
            //from the student's average grade to produce the final grade.  The result should be a set of
            //grades that averages exactly to the student's average grade, as defined by their performance profile.

            //take care to ensure the grade variance can't result in a grade > Max desired grade (usually 100) or < Min desired grade (usually 0) given
            //the student's average grade
            var absoluteMaxAllowableGradeVariance = Math.Min(maxGradeVariance, Math.Min(generatedGradeRange.MaxPossibleGrade - targetGradeAverage, targetGradeAverage - generatedGradeRange.MinPossibleGrade));

            for (int i = 0; i < totalGradesToGenerate - 1; i++)
            {
                //Cap the variance in each iteration to ensure we can never exceed the min / max allowable variance
                //this prevents a scenario where we get a string of variances with the same sign and when we
                //get to the final grade, we can't generate a grade large / small enough to meet the required average.
                //
                //For example: If the student average is 90 and the sum of their grade variances going into the last grade
                //is -11, we can't assign them 101 as their final grade in order to meet the required overall grade point average.
                var gradeVarianceUpperBound = sumOfGradeVariances > 0
                    ? absoluteMaxAllowableGradeVariance - sumOfGradeVariances
                    : absoluteMaxAllowableGradeVariance;

                var gradeVarianceLowerBound = sumOfGradeVariances < 0
                    ? -absoluteMaxAllowableGradeVariance - sumOfGradeVariances
                    : -absoluteMaxAllowableGradeVariance;

                var gradeVariance = randomNumberGenerator.Generate(gradeVarianceLowerBound, gradeVarianceUpperBound);
                sumOfGradeVariances += gradeVariance;

                var numericGrade = targetGradeAverage + gradeVariance;
                yield return numericGrade;
            }

            var finalNumericGrade = targetGradeAverage - sumOfGradeVariances;
            yield return finalNumericGrade;
        }
    }

    public class GradeRange
    {
        private GradeRange(int minPossibleGrade, int maxPossibleGrade)
        {
            if (minPossibleGrade > maxPossibleGrade)
                throw new ArgumentException($"{nameof(minPossibleGrade)} must be <= {nameof(maxPossibleGrade)}");

            if (minPossibleGrade < 0)
                throw new ArgumentException($"{nameof(minPossibleGrade)} must be >= 0");

            MinPossibleGrade = minPossibleGrade;
            MaxPossibleGrade = maxPossibleGrade;
        }

        public int MinPossibleGrade { get; }
        public int MaxPossibleGrade { get; }

        public static readonly GradeRange FullGradingScale = new GradeRange(LetterGrade.F.MinNumericGrade, LetterGrade.A.MaxNumericGrade);
        public static readonly GradeRange NoFailures = new GradeRange(LetterGrade.D.MinNumericGrade, LetterGrade.A.MaxNumericGrade);
    }
}
