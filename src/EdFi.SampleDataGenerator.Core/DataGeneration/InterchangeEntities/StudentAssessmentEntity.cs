using System;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common.Entity;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common.Interchanges;

namespace EdFi.SampleDataGenerator.Core.DataGeneration.InterchangeEntities
{
    public sealed partial class StudentAssessmentEntity : Entity
    {
        private StudentAssessmentEntity(Type entityType) : base(entityType, Interchange.StudentAssessment)
        {
        }

        public static readonly StudentAssessmentEntity StudentAssessment = new StudentAssessmentEntity(typeof(Entities.StudentAssessment));
    }
}
