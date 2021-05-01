using System;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common.Entity;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common.Interchanges;
using EdFi.SampleDataGenerator.Core.Entities;

namespace EdFi.SampleDataGenerator.Core.DataGeneration.InterchangeEntities
{
    public sealed partial class StudentGradebookEntity : Entity
    {
        private StudentGradebookEntity(Type entityType) : base(entityType, Interchange.StudentGradebook)
        {
        }

        public static readonly StudentGradebookEntity GradebookEntry = new StudentGradebookEntity(typeof(GradebookEntry));
        public static readonly StudentGradebookEntity StudentGradebookEntry = new StudentGradebookEntity(typeof(StudentGradebookEntry));
    }
}
