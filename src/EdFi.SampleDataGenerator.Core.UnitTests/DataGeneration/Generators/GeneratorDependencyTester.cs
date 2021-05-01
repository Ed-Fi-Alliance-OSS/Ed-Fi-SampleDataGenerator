using System.Collections.Generic;
using System.Linq;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common.Interchanges;
using EdFi.SampleDataGenerator.Core.DataGeneration.Generators;
using NUnit.Framework;
using Shouldly;

namespace EdFi.SampleDataGenerator.Core.UnitTests.DataGeneration.Generators
{
    public class GeneratorDependencyTester
    {
        private static readonly List<GlobalDataGenerator> GlobalDataGenerators = InterchangeDataGeneratorFactory.GetAllGlobalDataGenerators(null).ToList();
        private static readonly List<StudentDataGenerator> StudentDataGenerators = InterchangeDataGeneratorFactory.GetAllStudentDataGenerators(null).ToList();

        [Test, TestCaseSource(nameof(GlobalDataGenerators))]
        public void GlobalDataGeneratorsShouldNotDependOnTheirOwnInterchange(GlobalDataGenerator generator)
        {
            generator.DependsOnInterchanges.Any(i => generator.InterchangeEntity.Interchange.Name == i.Name).ShouldBeFalse();
        }

        [Test, TestCaseSource(nameof(StudentDataGenerators))]
        public void StudentDataGeneratorsShouldNotDependOnTheirOwnInterchange(StudentDataGenerator generator)
        {
            generator.DependsOnInterchanges.Any(i => generator.InterchangeEntity.Interchange.Name == i.Name).ShouldBeFalse();
        }

        [Test]
        public void StudentDataGeneratorsShouldGenerateDistinctEntities()
        {
            var generatedEntities = StudentDataGenerators.SelectMany(g => g.GeneratesEntities.Select(x => x.EntityType)).ToList();
            var distinctEntities = generatedEntities.Distinct();

            generatedEntities.Count.ShouldBe(distinctEntities.Count());
        }
    }
}
