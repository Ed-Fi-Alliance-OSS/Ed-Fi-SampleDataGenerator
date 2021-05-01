using System;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common.Entity;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common.Interchanges;
using EdFi.SampleDataGenerator.Core.Entities;

namespace EdFi.SampleDataGenerator.Core.DataGeneration.InterchangeEntities
{
    public sealed partial class StudentDisciplineEntity : Entity
    {
        private StudentDisciplineEntity(Type entityType) : base(entityType, Interchange.StudentDiscipline)
        {
        }

        public static readonly StudentDisciplineEntity DisciplineIncident = new StudentDisciplineEntity(typeof(DisciplineIncident));
        public static readonly StudentDisciplineEntity StudentDisciplineIncidentAssociation = new StudentDisciplineEntity(typeof(StudentDisciplineIncidentAssociation));
        public static readonly StudentDisciplineEntity DisciplineAction = new StudentDisciplineEntity(typeof(DisciplineAction));
    }
}