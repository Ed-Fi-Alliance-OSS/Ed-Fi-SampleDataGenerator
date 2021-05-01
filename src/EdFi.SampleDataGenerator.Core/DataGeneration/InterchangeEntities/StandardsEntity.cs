using System;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common.Entity;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common.Interchanges;
using EdFi.SampleDataGenerator.Core.Entities;

namespace EdFi.SampleDataGenerator.Core.DataGeneration.InterchangeEntities
{
    public sealed partial class StandardsEntity : Entity
    {
        private StandardsEntity(Type entityType) : base(entityType, Interchange.Standards)
        {
        }

        public static readonly StandardsEntity LearningStandard = new StandardsEntity(typeof(LearningStandard));
        public static readonly StandardsEntity LearningObjective = new StandardsEntity(typeof(LearningObjective));
    }
}
