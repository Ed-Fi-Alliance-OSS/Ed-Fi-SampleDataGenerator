using System.Collections.Generic;
using System.Linq;
using EdFi.SampleDataGenerator.Core.Config;
using EdFi.SampleDataGenerator.Core.DataGeneration.Generators;

namespace EdFi.SampleDataGenerator.Core.Helpers
{
    public static class StudentGradeHelpers
    {
        public static int GetGradePointAverageByPerformanceIndex(this IEnumerable<IStudentGradeRange> studentGradeRanges, StudentPerformanceProfile studentPerformanceProfile)
        {
            var studentGradeRange = studentGradeRanges.First(x => studentPerformanceProfile.PerformanceIndex >= x.LowerPerformanceIndex && studentPerformanceProfile.PerformanceIndex <= x.UpperPerformanceIndex);
            var minNumericGrade = studentGradeRange.MinNumericGrade;
            var maxNumericGrade = studentGradeRange.MaxNumericGrade;
            
            //Gets the percentage of studentPerformanceProfile.PerformanceIndex related to the PerformanceIndex Range
            var studentPerformanceIndexPercentage =
                        (studentPerformanceProfile.PerformanceIndex - studentGradeRange.LowerPerformanceIndex) /
                        (studentGradeRange.UpperPerformanceIndex - studentGradeRange.LowerPerformanceIndex);

            var gradeVariance = (int)((maxNumericGrade - minNumericGrade) * studentPerformanceIndexPercentage);

            var averageStudentGrade = minNumericGrade + gradeVariance;

            return averageStudentGrade;
        }
    }
}
