using System;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common.Entity;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common.Interchanges;

namespace EdFi.SampleDataGenerator.Core.DataGeneration.InterchangeEntities
{
    public sealed partial class StudentEntity : Entity
    {
        private StudentEntity(Type entityType) : base(entityType, Interchange.Student)
        {
        }

        public static readonly StudentEntity Student = new StudentEntity(typeof(Entities.Student));
    }
}
