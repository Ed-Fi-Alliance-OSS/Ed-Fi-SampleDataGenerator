using System;
using System.Collections.Generic;
using System.Linq;
using EdFi.SampleDataGenerator.Core.Helpers;

namespace EdFi.SampleDataGenerator.Core.DataGeneration.Common.Attributes
{
    public static class EntityAttributeGeneratorFactory
    {
        public static IEnumerable<IEntityAttributeGenerator<TContext, TConfig>> BuildAllAttributeGenerators<TContext, TConfig>(IRandomNumberGenerator randomNumberGenerator)
        {
            var baseType = typeof(IEntityAttributeGenerator<TContext, TConfig>);

            return baseType
                .Assembly
                .ConcreteImplementations(baseType)
                .Select(g => (IEntityAttributeGenerator<TContext, TConfig>)Activator.CreateInstance(g, randomNumberGenerator));
        }
    }
}
