using System;
using System.Reflection;
using EdFi.SampleDataGenerator.Core.Helpers;
using EdFi.SampleDataGenerator.Core.Serialization.Output.Interchanges;

namespace EdFi.SampleDataGenerator.Core.Serialization.Output
{
    public static class InterchangeOutputHelpers
    {
        public static bool IsSupportedInterchangeCollectionType(this Type entityType)
        {
            return entityType.IsArray || entityType.IsList() || typeof(ISdgEntityOutputCollection).IsAssignableFrom(entityType);
        }

        public static InterchangeOutputInfo GetInterchangeOutputInfo(this PropertyInfo propertyInfo)
        {
            var interchangeOutputTypeAttribute = (InterchangeOutputAttribute)Attribute.GetCustomAttribute(propertyInfo, typeof(InterchangeOutputAttribute));
            return interchangeOutputTypeAttribute?.InterchangeOutputInfo ?? propertyInfo.PropertyType.GetInterchangeOutputInfo();
        }

        public static InterchangeOutputInfo GetInterchangeOutputInfo(this Type entityType)
        {
            var underlyingType = entityType;
            if (entityType.IsSupportedInterchangeCollectionType())
            {
                underlyingType = entityType.GetUnderlyingType();
            }

            var interchangeOutputTypeAttribute = (InterchangeOutputAttribute)Attribute.GetCustomAttribute(underlyingType, typeof(InterchangeOutputAttribute));
            return interchangeOutputTypeAttribute?.InterchangeOutputInfo;
        }

        public static bool HasInterchangeOutputInfo(this Type entityType)
        {
            return entityType.GetInterchangeOutputInfo() != null;
        }

        public static bool ShouldBeOutput(this PropertyInfo property)
        {
            var noOutputAttribute = (DoNotOutputToInterchangeAttribute)Attribute.GetCustomAttribute(property, typeof(DoNotOutputToInterchangeAttribute));
            return noOutputAttribute == null;
        }
    }
}
