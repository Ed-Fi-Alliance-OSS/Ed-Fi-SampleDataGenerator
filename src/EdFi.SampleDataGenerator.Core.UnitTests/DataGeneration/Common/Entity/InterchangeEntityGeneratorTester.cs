using System;
using System.Linq;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common.Entity;
using EdFi.SampleDataGenerator.Core.DataGeneration.Generators;
using EdFi.SampleDataGenerator.Core.Helpers;
using NUnit.Framework;
using Shouldly;

namespace EdFi.SampleDataGenerator.Core.UnitTests.DataGeneration.Common.Entity
{
    [TestFixture]
    public class InterchangeEntityGeneratorTester
    {
        [Test]
        public void InterchangeEntityGeneratorsShouldAlwaysDeclareWhatEntityTheyGenerate()
        {
            foreach (var interchangeDataGenerator in AllInterchangeDataGenerators<GlobalDataGeneratorContext, GlobalDataGeneratorConfig>())
                interchangeDataGenerator.GeneratesEntity.ShouldNotBeNull($"{interchangeDataGenerator.GetType()}.GeneratesEntity is null.");

            foreach (var interchangeDataGenerator in AllInterchangeDataGenerators<StudentDataGeneratorContext, StudentDataGeneratorConfig>())
                interchangeDataGenerator.GeneratesEntity.ShouldNotBeNull($"{interchangeDataGenerator.GetType()}.GeneratesEntity is null.");
        }

        private static IInterchangeEntityGenerator<TContext, TConfig>[] AllInterchangeDataGenerators<TContext, TConfig>()
        {
            IRandomNumberGenerator randomNumberGenerator = new TestRandomNumberGenerator();

            var baseType = typeof(IInterchangeEntityGenerator<TContext, TConfig>);

            return baseType
                .Assembly
                .ConcreteImplementations(baseType)
                .Select(x => (IInterchangeEntityGenerator<TContext, TConfig>)Activator.CreateInstance(x, randomNumberGenerator))
                .ToArray();
        }
    }
}
