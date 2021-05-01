using EdFi.SampleDataGenerator.Core.DataGeneration.Common.Entity;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common.Interchanges;

namespace EdFi.SampleDataGenerator.Core.DataGeneration.InterchangeEntities
{
    public sealed partial class InterchangeEntity
    {
        public Interchange Interchange { get; }
        public IEntity[] Entities { get; }

        private InterchangeEntity(Interchange interchange, IEntity[] entities)
        {
            Interchange = interchange;
            Entities = entities;
        }

        public static readonly InterchangeEntity AssessmentMetadata = new InterchangeEntity(Interchange.AssessmentMetadata, Entity.GetAll<AssessmentMetadataEntity>());
        public static readonly InterchangeEntity Descriptors = new InterchangeEntity(Interchange.Descriptors, Entity.GetAll<DescriptorEntity>());
        public static readonly InterchangeEntity EducationOrganization = new InterchangeEntity(Interchange.EducationOrganization, Entity.GetAll<EducationOrganizationEntity>());
        public static readonly InterchangeEntity EducationOrgCalendar = new InterchangeEntity(Interchange.EducationOrgCalendar, Entity.GetAll<EducationOrgCalendarEntity>());
        public static readonly InterchangeEntity Finance = new InterchangeEntity(Interchange.Finance, new IEntity[]{});
        public static readonly InterchangeEntity MasterSchedule = new InterchangeEntity(Interchange.MasterSchedule, Entity.GetAll<MasterScheduleEntity>());
        public static readonly InterchangeEntity Parent = new InterchangeEntity(Interchange.Parent, Entity.GetAll<ParentEntity>());
        public static readonly InterchangeEntity PostSecondaryEvent = new InterchangeEntity(Interchange.PostSecondaryEvent, new IEntity[]{});
        public static readonly InterchangeEntity StaffAssociation = new InterchangeEntity(Interchange.StaffAssociation, Entity.GetAll<StaffAssociationEntity>());
        public static readonly InterchangeEntity Standards = new InterchangeEntity(Interchange.Standards, Entity.GetAll<StandardsEntity>());
        public static readonly InterchangeEntity Student = new InterchangeEntity(Interchange.Student, Entity.GetAll<StudentEntity>());
        public static readonly InterchangeEntity StudentAssessment = new InterchangeEntity(Interchange.StudentAssessment, Entity.GetAll<StudentAssessmentEntity>());
        public static readonly InterchangeEntity StudentAttendance = new InterchangeEntity(Interchange.StudentAttendance, Entity.GetAll<StudentAttendanceEntity>());
        public static readonly InterchangeEntity StudentCohort = new InterchangeEntity(Interchange.StudentCohort, Entity.GetAll<StudentCohortEntity>());
        public static readonly InterchangeEntity StudentDiscipline = new InterchangeEntity(Interchange.StudentDiscipline, Entity.GetAll<StudentDisciplineEntity>());
        public static readonly InterchangeEntity StudentEnrollment = new InterchangeEntity(Interchange.StudentEnrollment, Entity.GetAll<StudentEnrollmentEntity>());
        public static readonly InterchangeEntity StudentGrade = new InterchangeEntity(Interchange.StudentGrade, Entity.GetAll<StudentGradeEntity>());
        public static readonly InterchangeEntity StudentGradebook = new InterchangeEntity(Interchange.StudentGradebook, Entity.GetAll<StudentGradebookEntity>());
        public static readonly InterchangeEntity StudentIntervention = new InterchangeEntity(Interchange.StudentIntervention, new IEntity[]{});
        public static readonly InterchangeEntity StudentProgram = new InterchangeEntity(Interchange.StudentProgram, Entity.GetAll<StudentProgramEntity>());
        public static readonly InterchangeEntity StudentTranscript = new InterchangeEntity(Interchange.StudentTranscript, Entity.GetAll<StudentTranscriptEntity>());
    }
}
