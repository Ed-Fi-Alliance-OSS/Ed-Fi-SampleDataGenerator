using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using EdFi.SampleDataGenerator.Core.DataGeneration.Generators;

namespace EdFi.SampleDataGenerator.Core.DataGeneration.Common.Interchanges
{
    public static class InterchangeDataGeneratorFactory
    {
        public static IEnumerable<GlobalDataGenerator> GetAllGlobalDataGenerators(IRandomNumberGenerator randomNumberGenerator)
        {
            return GetAllGenerators<GlobalDataGenerator>(randomNumberGenerator);
        }

        public static IEnumerable<StudentDataGenerator> GetAllStudentDataGenerators(IRandomNumberGenerator randomNumberGenerator)
        {
            return GetAllGenerators<StudentDataGenerator>(randomNumberGenerator);
        }

        public static IEnumerable<TemplateDataGenerator> GetAllTemplateDataGenerators(IRandomNumberGenerator randomNumberGenerator)
        {
            return GetAllGenerators<TemplateDataGenerator>(randomNumberGenerator);
        }

        private static IEnumerable<TGeneratorBase> GetAllGenerators<TGeneratorBase>(IRandomNumberGenerator randomNumberGenerator)
        {
            var generatorBaseType = typeof(TGeneratorBase);
            var generatorTypes = Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(t => generatorBaseType.IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract);

            return generatorTypes.Select(g => (TGeneratorBase)Activator.CreateInstance(g, randomNumberGenerator));
        }
    }
}
