using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace EdFi.SampleDataGenerator.Core.Helpers
{
    public static class AssemblyExtensions
    {
        public static IEnumerable<Type> ConcreteImplementations(this Assembly assembly, Type baseType)
        {
            return assembly
                .GetTypes()
                .Where(t => baseType.IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract);
        }

        public static IEnumerable<TBaseType> CreateAll<TBaseType>(this Assembly assembly, params object[] constructorParameters)
        {
            var baseType = typeof(TBaseType);

            var concreteTypes = assembly.ConcreteImplementations(baseType);

            return concreteTypes.Select(g => (TBaseType)Activator.CreateInstance(g, constructorParameters));
        }
    }
}
