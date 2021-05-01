namespace EdFi.SampleDataGenerator.Core.DataGeneration.Generators.StudentAttendance
{
    public class AttendanceTemplate
    {
        public AttendanceTemplateType AttendanceTemplateType { get; set; }
        public double Probability { get; set; }
        public double? MinPerformanceIndex { get; set; }
        public double? MaxPerformanceIndex { get; set; }
        public AttendanceParameters TemplateParameters { get; set; }
        public AttendanceParameters StandardParameters { get; set; }
    }

    public enum AttendanceTemplateType
    {
        MissSpecificDay,
        AbsentAtBeginningOfDay,
        AbsentAtEndOfDay,
        TardyForFirstPeriod,
        FrequentlyTardy,
        Standard
    }

    public class AttendanceParameters
    {
        public double? AverageAbsenceRate { get; set; }
        public double? AverageTardyRate { get; set; }
    }
}