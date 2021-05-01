using System;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common.Entity;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common.Interchanges;

namespace EdFi.SampleDataGenerator.Core.DataGeneration.InterchangeEntities
{
    public sealed partial class ParentEntity : Entity
    {
        private ParentEntity(Type entityType) : base(entityType, Interchange.Parent)
        {
        }

        public static readonly ParentEntity Parent = new ParentEntity(typeof(Entities.Parent));
        public static readonly ParentEntity StudentParentAssociation = new ParentEntity(typeof(Entities.StudentParentAssociation));
    }
}
