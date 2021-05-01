using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using EdFi.SampleDataGenerator.Core.Helpers;

namespace EdFi.SampleDataGenerator.Core.DataGeneration.Common.Entity
{
    public static class InterchangeEntityGeneratorFactory
    {
        public static IEnumerable<IInterchangeEntityGenerator<TContext, TConfig>> DefaultGeneratorFactory<TContext, TConfig, TGeneratorBase>(IRandomNumberGenerator randomNumberGenerator)
            where TGeneratorBase : IInterchangeEntityGenerator<TContext, TConfig>
        {
            return DefaultGeneratorFactory<TContext, TConfig, TGeneratorBase>(randomNumberGenerator, Assembly.GetExecutingAssembly());
        }

        public static IEnumerable<IInterchangeEntityGenerator<TContext, TConfig>> DefaultGeneratorFactory<TContext, TConfig, TGeneratorBase>(IRandomNumberGenerator randomNumberGenerator, Assembly assembly)
            where TGeneratorBase : IInterchangeEntityGenerator<TContext, TConfig>
        {
            var generatorBaseType = typeof(TGeneratorBase);
            var generatorTypes = assembly.ConcreteImplementations(generatorBaseType);

            return generatorTypes.Select(g => (IInterchangeEntityGenerator<TContext, TConfig>)Activator.CreateInstance(g, randomNumberGenerator));
        }
    }
}
