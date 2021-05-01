using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common.Attributes;
using EdFi.SampleDataGenerator.Core.DataGeneration.Generators;
using EdFi.SampleDataGenerator.Core.DataGeneration.Generators.Student;
using EdFi.SampleDataGenerator.Core.Helpers;
using EdFi.SampleDataGenerator.Core.UnitTests.DataGeneration.Common;
using NUnit.Framework;
using Shouldly;

namespace EdFi.SampleDataGenerator.Core.UnitTests.DataGeneration.Generators.Student.Attributes
{
    public class StudentAttributeGeneratorFactoryTester
    {
        private static readonly IRandomNumberGenerator RandomNumberGenerator = new TestRandomNumberGenerator();

        private static readonly List<IEntityAttributeGenerator<StudentDataGeneratorContext, StudentDataGeneratorConfig>> _studentAttributeGenerators =
            EntityAttributeGeneratorFactory.BuildAllAttributeGenerators<StudentDataGeneratorContext, StudentDataGeneratorConfig>(RandomNumberGenerator).ToList();

        [Test, TestCaseSource(nameof(GetAllStudentAttributeGenerators))]
        public void ShouldBuildAllAttributeGenerators(Type generatorType)
        {
            _studentAttributeGenerators.Any(g => g.GetType() == generatorType).ShouldBeTrue();
        }

        private static IEnumerable<Type> GetAllStudentAttributeGenerators()
        {
            var baseType = typeof(IEntityAttributeGenerator<StudentDataGeneratorContext, StudentDataGeneratorConfig>);

            return baseType
                .Assembly
                .ConcreteImplementations(baseType);
        }
    }
}
