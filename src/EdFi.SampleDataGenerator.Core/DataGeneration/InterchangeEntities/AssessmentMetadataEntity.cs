using System;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common.Entity;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common.Interchanges;
using EdFi.SampleDataGenerator.Core.Entities;

namespace EdFi.SampleDataGenerator.Core.DataGeneration.InterchangeEntities
{
    public sealed partial class AssessmentMetadataEntity : Entity
    {
        private AssessmentMetadataEntity(Type entityType) : base(entityType, Interchange.AssessmentMetadata)
        {
        }

        public static readonly AssessmentMetadataEntity Assessment = new AssessmentMetadataEntity(typeof(Assessment));
    }
}
