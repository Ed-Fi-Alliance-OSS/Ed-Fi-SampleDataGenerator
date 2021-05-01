using System;
using System.Linq;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common.Entity;
using EdFi.SampleDataGenerator.Core.DataGeneration.InterchangeEntities;

namespace EdFi.SampleDataGenerator.Core.DataGeneration.Common.Dependencies
{
    public static class EntityDependencies
    {
        public static IEntity[] None = Enumerable.Empty<IEntity>().ToArray();

        public static IEntity[] Create(params IEntity[] dependencies)
        {
            if (dependencies == null) throw new ArgumentNullException(nameof(dependencies));
            return dependencies;
        }
    }

    public static class InterchangeDependencies
    {
        public static InterchangeEntity[] None => Enumerable.Empty<InterchangeEntity>().ToArray();
    }
}
