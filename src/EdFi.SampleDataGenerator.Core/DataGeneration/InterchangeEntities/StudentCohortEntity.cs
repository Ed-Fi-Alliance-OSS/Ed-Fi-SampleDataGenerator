using System;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common.Entity;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common.Interchanges;
using EdFi.SampleDataGenerator.Core.Entities;

namespace EdFi.SampleDataGenerator.Core.DataGeneration.InterchangeEntities
{
    public sealed partial class StudentCohortEntity : Entity
    {
        private StudentCohortEntity(Type entityType) : base(entityType, Interchange.StudentCohort)
        {
        }

        public static readonly StudentCohortEntity Cohort = new StudentCohortEntity(typeof(Cohort));
        public static readonly StudentCohortEntity StaffCohortAssociation = new StudentCohortEntity(typeof(StaffCohortAssociation));
        public static readonly StudentCohortEntity StudentCohortAssociation = new StudentCohortEntity(typeof(StudentCohortAssociation));
    }
}
