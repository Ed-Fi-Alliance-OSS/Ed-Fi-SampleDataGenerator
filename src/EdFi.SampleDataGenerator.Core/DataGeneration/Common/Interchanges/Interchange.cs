using Headspring;

namespace EdFi.SampleDataGenerator.Core.DataGeneration.Common.Interchanges
{
    public sealed partial class Interchange : Enumeration<Interchange, string>
    {
        public string Name { get; }

        private Interchange(string name) : base(name, name)
        {
            Name = name;
        }

        public const string AssessmentMetadataInterchangeName = "AssessmentMetadata";
        public const string DescriptorsInterchangeName = "Descriptors";
        public const string EducationOrganizationInterchangeName = "EducationOrganization";
        public const string EducationOrgCalendarInterchangeName = "EducationOrgCalendar";
        public const string FinanceInterchangeName = "Finance";
        public const string MasterScheduleInterchangeName = "MasterSchedule";
        public const string ParentInterchangeName = "Parent";
        public const string PostSecondaryEventInterchangeName = "PostSecondaryEvent";
        public const string StaffAssociationInterchangeName = "StaffAssociation";
        public const string StandardsInterchangeName = "Standards";
        public const string StudentInterchangeName = "Student";
        public const string StudentAssessmentInterchangeName = "StudentAssessment";
        public const string StudentAttendanceInterchangeName = "StudentAttendance";
        public const string StudentCohortInterchangeName = "StudentCohort";
        public const string StudentDisciplineInterchangeName = "StudentDiscipline";
        public const string StudentEnrollmentInterchangeName = "StudentEnrollment";
        public const string StudentGradeInterchangeName = "StudentGrade";
        public const string StudentGradebookInterchangeName = "StudentGradebook";
        public const string StudentInterventionInterchangeName = "StudentIntervention";
        public const string StudentProgramInterchangeName = "StudentProgram";
        public const string StudentTranscriptInterchangeName = "StudentTranscript";

        public static Interchange AssessmentMetadata = new Interchange(AssessmentMetadataInterchangeName);
        public static Interchange Descriptors = new Interchange(DescriptorsInterchangeName);
        public static Interchange EducationOrganization = new Interchange(EducationOrganizationInterchangeName);
        public static Interchange EducationOrgCalendar = new Interchange(EducationOrgCalendarInterchangeName);
        public static Interchange Finance = new Interchange(FinanceInterchangeName);
        public static Interchange MasterSchedule = new Interchange(MasterScheduleInterchangeName);
        public static Interchange Parent = new Interchange(ParentInterchangeName);
        public static Interchange PostSecondaryEvent = new Interchange(PostSecondaryEventInterchangeName);
        public static Interchange StaffAssociation = new Interchange(StaffAssociationInterchangeName);
        public static Interchange Standards = new Interchange(StandardsInterchangeName);
        public static Interchange Student = new Interchange(StudentInterchangeName);
        public static Interchange StudentAssessment = new Interchange(StudentAssessmentInterchangeName);
        public static Interchange StudentAttendance = new Interchange(StudentAttendanceInterchangeName);
        public static Interchange StudentCohort = new Interchange(StudentCohortInterchangeName);
        public static Interchange StudentDiscipline = new Interchange(StudentDisciplineInterchangeName);
        public static Interchange StudentEnrollment = new Interchange(StudentEnrollmentInterchangeName);
        public static Interchange StudentGrade = new Interchange(StudentGradeInterchangeName);
        public static Interchange StudentGradebook = new Interchange(StudentGradebookInterchangeName);
        public static Interchange StudentIntervention = new Interchange(StudentInterventionInterchangeName);
        public static Interchange StudentProgram = new Interchange(StudentProgramInterchangeName);
        public static Interchange StudentTranscript = new Interchange(StudentTranscriptInterchangeName);
    }
}
