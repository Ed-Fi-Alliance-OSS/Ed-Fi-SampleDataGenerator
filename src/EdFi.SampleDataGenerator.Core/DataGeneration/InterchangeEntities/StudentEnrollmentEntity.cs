using System;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common.Entity;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common.Interchanges;
using EdFi.SampleDataGenerator.Core.Entities;

namespace EdFi.SampleDataGenerator.Core.DataGeneration.InterchangeEntities
{
    public sealed partial class StudentEnrollmentEntity : Entity
    {
        private StudentEnrollmentEntity(Type entityType) : base(entityType, Interchange.StudentEnrollment)
        {
        }

        public static readonly StudentEnrollmentEntity StudentSchoolAssociation = new StudentEnrollmentEntity(typeof (StudentSchoolAssociation));
        public static readonly StudentEnrollmentEntity StudentEducationOrganizationAssociation = new StudentEnrollmentEntity(typeof(StudentEducationOrganizationAssociation));
        public static readonly StudentEnrollmentEntity StudentSectionAssociation = new StudentEnrollmentEntity(typeof(StudentSectionAssociation));
        public static readonly StudentEnrollmentEntity GraduationPlan = new StudentEnrollmentEntity(typeof(GraduationPlan));
    }
}
