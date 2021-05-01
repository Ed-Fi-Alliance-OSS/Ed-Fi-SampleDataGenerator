using System;
using System.Linq;
using System.Linq.Expressions;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common.Entity;
using EdFi.SampleDataGenerator.Core.Helpers;

namespace EdFi.SampleDataGenerator.Core.DataGeneration.Common.Attributes
{
    public abstract class EntityField<TEntity> : IEntityField
    {
        /// <summary>
        /// This constructor is only meant for a limited use-case where we need to generate a
        /// "virtual" property that's not actually part of the target model and therefore
        /// can't use an Expression Func as in the other constructor overload
        /// </summary>
        /// <param name="virtualFieldName">Name of the field.</param>
        /// <param name="entity">Entity to which this field is "attached"</param>
        /// <param name="description">Optional description to differentiate this field</param>
        protected EntityField(string virtualFieldName, IEntity entity, string description = null)
        {
            FieldName = virtualFieldName;
            FullyQualifiedFieldName = $"{typeof(TEntity).Name}.{virtualFieldName}";
            if (description != null)
            {
                FullyQualifiedFieldName += " (" + description + ")";
            }

            IsVirtual = true;
            Entity = entity;
        }

        protected EntityField(Expression<Func<TEntity, object>> expression, IEntity entity, string description = null)
        {
            var path = Property.Path(expression);

            FieldName = path.First().Name;

            FullyQualifiedFieldName = $"{typeof(TEntity).Name}.{string.Join(".", path.Select(x => x.Name))}";

            if (description != null)
                FullyQualifiedFieldName += " (" + description + ")";

            Entity = entity;
        }

        public IEntity Entity { get; }
        public string FieldName { get; }
        public string FullyQualifiedFieldName { get; }
        public bool IsVirtual { get; }
    }
}