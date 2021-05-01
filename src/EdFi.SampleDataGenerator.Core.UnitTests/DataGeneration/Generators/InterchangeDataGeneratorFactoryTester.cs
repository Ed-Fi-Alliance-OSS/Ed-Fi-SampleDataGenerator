using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common.Interchanges;
using EdFi.SampleDataGenerator.Core.DataGeneration.Generators;
using NUnit.Framework;
using Shouldly;

namespace EdFi.SampleDataGenerator.Core.UnitTests.DataGeneration.Generators
{
    public class InterchangeDataGeneratorFactoryTester
    {
        private static readonly List<GlobalDataGenerator> GlobalDataGenerators = InterchangeDataGeneratorFactory.GetAllGlobalDataGenerators(null).ToList();
        private static readonly List<StudentDataGenerator> StudentDataGenerators = InterchangeDataGeneratorFactory.GetAllStudentDataGenerators(null).ToList();

        [Test, TestCaseSource(nameof(GetAllGlobalDataGenerators))]
        public void ShouldBuildAllGlobalDataGenerators(Type generatorType)
        {
            GlobalDataGenerators.Any(g => g.GetType() == generatorType).ShouldBeTrue();
        }
        
        [Test, TestCaseSource(nameof(GetAllStudentDataGenerators))]
        public void ShouldBuildAllStudentDataGenerators(Type generatorType)
        {
            StudentDataGenerators.Any(g => g.GetType() == generatorType).ShouldBeTrue();
        }

        private static IEnumerable<Type> GetAllGlobalDataGenerators()
        {
            var type = typeof(GlobalDataGenerator);
            return Assembly.GetAssembly(type)
                .GetTypes()
                .Where(t => type.IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract);
        }

        private static IEnumerable<Type> GetAllStudentDataGenerators()
        {
            var type = typeof(StudentDataGenerator);
            return Assembly.GetAssembly(type)
                .GetTypes()
                .Where(t => type.IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract);
        }
    }
}
