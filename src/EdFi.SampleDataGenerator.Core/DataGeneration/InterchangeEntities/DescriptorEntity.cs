using System;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common.Entity;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common.Interchanges;
using EdFi.SampleDataGenerator.Core.Entities;

namespace EdFi.SampleDataGenerator.Core.DataGeneration.InterchangeEntities
{
    public sealed partial class DescriptorEntity : Entity
    {
        private DescriptorEntity(Type entityType) : base(entityType, Interchange.Descriptors)
        {
        }

        public static readonly DescriptorEntity Descriptor = new DescriptorEntity(typeof(DescriptorType));
    }
}
