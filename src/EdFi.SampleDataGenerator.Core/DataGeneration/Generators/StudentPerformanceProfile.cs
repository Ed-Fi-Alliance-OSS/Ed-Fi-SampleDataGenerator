using System;
using EdFi.SampleDataGenerator.Core.Config;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common;
using EdFi.SampleDataGenerator.Core.Helpers;

namespace EdFi.SampleDataGenerator.Core.DataGeneration.Generators
{
    public class StudentPerformanceProfile
    {
        public double PerformanceIndex { get; set; }
        public bool IsPerfectStudent => PerformanceIndex > 1 - Double.Epsilon;

        public bool IsStudentHighPerforming(StudentDataGeneratorConfig configuration)
        {
            const double defaultHighPerformingStudentPercentile = 0.9;

            var highPerformingStudentPercentile =
                configuration.DistrictProfile.HighPerformingStudentPercentile ?? defaultHighPerformingStudentPercentile;

            var minimumPerformanceIndex =
                StudentPerformanceProfileDistribution.GetStudentPerformanceProfileFromPercentile(highPerformingStudentPercentile);

            return PerformanceIndex >= minimumPerformanceIndex;
        }

        public bool IsEligibleForGiftedAndTalentedProgram(StudentDataGeneratorConfig configuration)
        {
            var gradeLevel = configuration.GradeProfile.GetGradeLevel().GetNumericGradeLevel();

            return gradeLevel >= 9 &&
                   gradeLevel <= 12 &&
                   IsStudentHighPerforming(configuration);
        }
    }
}