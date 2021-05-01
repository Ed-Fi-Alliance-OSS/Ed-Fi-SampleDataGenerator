namespace EdFi.SampleDataGenerator.Core.Config
{
    public interface IStudentGradeRange
    {
        double LowerPerformanceIndex { get; set; }
        double UpperPerformanceIndex { get; set; }
        int MinNumericGrade { get; set; }
        int MaxNumericGrade { get; set; }
    }
}
