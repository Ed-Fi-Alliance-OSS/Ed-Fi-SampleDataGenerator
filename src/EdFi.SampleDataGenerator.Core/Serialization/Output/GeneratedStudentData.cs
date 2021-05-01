using EdFi.SampleDataGenerator.Core.Serialization.Output.Interchanges;

namespace EdFi.SampleDataGenerator.Core.Serialization.Output
{
    public partial class GeneratedStudentData
    {
        public StudentData StudentData { get; set; } = new StudentData();
        public StudentEnrollmentData StudentEnrollmentData { get; set; } = new StudentEnrollmentData();
        public ParentData ParentData { get; set; } = new ParentData();
        public StudentDisciplineData StudentDisciplineData { get; set; } = new StudentDisciplineData();
        public StudentAttendanceData StudentAttendanceData { get; set; } = new StudentAttendanceData();
        public StudentTranscriptData StudentTranscriptData { get; set; } = new StudentTranscriptData();
        public StudentAssessmentData StudentAssessmentData { get; set; } = new StudentAssessmentData();
        public StudentProgramData StudentProgramData { get; set; } = new StudentProgramData();
        public StudentGradeData StudentGradeData { get; set; } = new StudentGradeData();
        public StudentGradebookData StudentGradebookData { get; set; } = new StudentGradebookData();
        public StudentCohortData StudentCohortData { get; set; } = new StudentCohortData();
    }
}
