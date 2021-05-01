using System;
using System.Linq;
using System.Reflection;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common.Interchanges;

namespace EdFi.SampleDataGenerator.Core.DataGeneration.Common.Entity
{
    public abstract class Entity : IEntity
    {
        protected Entity(Type entityType, Interchange interchange)
        {
            EntityType = entityType;
            Interchange = interchange;
        }

        public Type EntityType { get; }
        public string ClassName => EntityType.Name;

        public Interchange Interchange { get; }

        public static IEntity[] GetAll<TEntity>()
        {
            return typeof(TEntity)
                .GetProperties(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly)
                .Select(p => p.GetValue(null))
                .Cast<IEntity>()
                .ToArray();
        }
    }
}
