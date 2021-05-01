using System;
using System.Collections.Generic;
using System.Linq;
using EdFi.SampleDataGenerator.Core.Serialization.Output.Interchanges;

namespace EdFi.SampleDataGenerator.Core.Helpers
{
    public static class TypeHelpers
    {
        public static bool CanBeNull(this Type type)
        {
            return type.IsClass || (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>));
        }

        public static Type GetUnderlyingType(this Type type)
        {
            if (type.IsArray)
                return type.GetElementType();

            if (type.IsList())
                return type.GetGenericArguments()[0];

            var sdgCollectionInterface = type.GetGenericInterface(typeof (ISdgEntityOutputCollection<>));
            if (sdgCollectionInterface != null)
            {
                return sdgCollectionInterface.GetGenericArguments()[0];
            }

            return type;
        }

        public static bool IsList(this Type type)
        {
            return type.GetInterface(typeof (IList<>).FullName) != null;
        }

        public static Type GetGenericInterface(this Type type, Type genericInterfaceType)
        {
            return type
                .GetInterfaces()
                .FirstOrDefault(x =>
                  x.IsGenericType &&
                  x.GetGenericTypeDefinition() == genericInterfaceType);
        }

        public static bool ImplementsGenericInterface(this Type type, Type genericInterfaceType)
        {
            return type.GetGenericInterface(genericInterfaceType) != null;
        }
    }
}
