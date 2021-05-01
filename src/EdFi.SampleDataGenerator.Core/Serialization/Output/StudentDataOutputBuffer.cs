using EdFi.SampleDataGenerator.Core.Serialization.Output.Interchanges;

namespace EdFi.SampleDataGenerator.Core.Serialization.Output
{
    public partial class StudentDataOutputBuffer
    {
        public StudentCollection Students { get; } = new StudentCollection();
        public StudentEnrollmentCollection StudentEnrollmentRecords { get; } = new StudentEnrollmentCollection();
        public ParentCollection Parents { get; } = new ParentCollection();
        public StudentDisciplineCollection StudentDisciplineRecords { get; } = new StudentDisciplineCollection();
        public StudentAttendanceCollection StudentAttendanceRecords { get; } = new StudentAttendanceCollection();
        public StudentTranscriptCollection StudentTranscripts { get; } = new StudentTranscriptCollection();
        public StudentAssessmentCollection StudentAssessments { get; } = new StudentAssessmentCollection();
        public StudentProgramCollection StudentProgramRecords { get; } = new StudentProgramCollection();
        public StudentCohortCollection StudentCohortRecords { get; } = new StudentCohortCollection();
        public StudentGradeCollection StudentGrades { get; } = new StudentGradeCollection();
        public StudentGradebookCollection StudentGradebookRecords { get; } = new StudentGradebookCollection();
    }
}
