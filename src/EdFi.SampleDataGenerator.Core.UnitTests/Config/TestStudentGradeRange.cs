using EdFi.SampleDataGenerator.Core.Config;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common;

namespace EdFi.SampleDataGenerator.Core.UnitTests.Config
{
    public class TestStudentGradeRange : IStudentGradeRange
    {
        public double LowerPerformanceIndex { get; set; }
        public double UpperPerformanceIndex { get; set; }
        public int MinNumericGrade { get; set; }
        public int MaxNumericGrade { get; set; }

        public static IStudentGradeRange[] Defaults => new []
        { 
            new TestStudentGradeRange { LowerPerformanceIndex = StudentPerformanceProfileDistribution.GetStudentPerformanceProfileFromPercentile(0.81), UpperPerformanceIndex = 1, MinNumericGrade =90, MaxNumericGrade = 100},
            new TestStudentGradeRange { LowerPerformanceIndex = StudentPerformanceProfileDistribution.GetStudentPerformanceProfileFromPercentile(0.51), UpperPerformanceIndex = StudentPerformanceProfileDistribution.GetStudentPerformanceProfileFromPercentile(0.81), MinNumericGrade =80, MaxNumericGrade = 89},
            new TestStudentGradeRange { LowerPerformanceIndex = StudentPerformanceProfileDistribution.GetStudentPerformanceProfileFromPercentile(0.21), UpperPerformanceIndex = StudentPerformanceProfileDistribution.GetStudentPerformanceProfileFromPercentile(0.51), MinNumericGrade =70, MaxNumericGrade = 79},
            new TestStudentGradeRange { LowerPerformanceIndex = StudentPerformanceProfileDistribution.GetStudentPerformanceProfileFromPercentile(0.10), UpperPerformanceIndex = StudentPerformanceProfileDistribution.GetStudentPerformanceProfileFromPercentile(0.21), MinNumericGrade =60, MaxNumericGrade = 69},
            new TestStudentGradeRange { LowerPerformanceIndex = 0, UpperPerformanceIndex = StudentPerformanceProfileDistribution.GetStudentPerformanceProfileFromPercentile(0.10), MinNumericGrade =0, MaxNumericGrade = 59},
        };
    }
}