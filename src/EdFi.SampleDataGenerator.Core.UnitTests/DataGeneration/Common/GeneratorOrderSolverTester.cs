using System.Collections.Generic;
using System.Linq;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common.Attributes;
using EdFi.SampleDataGenerator.Core.DataGeneration.Generators;
using EdFi.SampleDataGenerator.Core.DataGeneration.Generators.Student;
using NUnit.Framework;
using Shouldly;

namespace EdFi.SampleDataGenerator.Core.UnitTests.DataGeneration.Common
{
    [TestFixture]
    public class GeneratorOrderSolverTester
    {
        private static readonly IRandomNumberGenerator RandomNumberGenerator = new TestRandomNumberGenerator();
        private static readonly List<IEntityAttributeGenerator<StudentDataGeneratorContext, StudentDataGeneratorConfig>> AllGenerators =
            EntityAttributeGeneratorFactory.BuildAllAttributeGenerators<StudentDataGeneratorContext, StudentDataGeneratorConfig>(RandomNumberGenerator).ToList();
        
        [Test]
        public void SortedGeneratorListShouldContainAllGenerators()
        {
            var sortedGenerators = AllGenerators.SortByDependencies();
            AllGenerators.Count.ShouldBe(sortedGenerators.Count);
            AllGenerators.All(g => sortedGenerators.Contains(g)).ShouldBeTrue();
        }

        [Test, TestCaseSource(nameof(AllGenerators))]
        public void ShouldOrderGeneratorsCorrectly(IEntityAttributeGenerator<StudentDataGeneratorContext, StudentDataGeneratorConfig> generator)
        {
            var sortedGenerators = AllGenerators.SortByDependencies();
            var generatorIndex = sortedGenerators.IndexOf(generator);
            foreach (var dependencyField in generator.DependsOnFields)
            {
                var requiredGenerators = AllGenerators.Where(g => g.FullyQualifiedFieldName == dependencyField.FullyQualifiedFieldName);
                requiredGenerators.All(rg => sortedGenerators.IndexOf(rg) < generatorIndex).ShouldBeTrue($"Generator {generator.GetType().Name} depends on but is sorted before '{dependencyField.FullyQualifiedFieldName}'");
            }
        }
    }
}
