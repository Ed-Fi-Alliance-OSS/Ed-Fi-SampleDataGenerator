using System.Linq;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common.Mutators;
using NUnit.Framework;
using Shouldly;

namespace EdFi.SampleDataGenerator.Core.UnitTests.DataGeneration.Common
{
    public class MutatorsConfigurationTests : GeneratorTestBase
    {
        [Test]
        public void ShouldNotHaveRepeatedNames()
        {
            var randomNumberGenerator = new RandomNumberGenerator();

            var names =
                    MutatorFactory.GlobalMutatorFactory(randomNumberGenerator).Select(x => x.Name)
                    .Concat(MutatorFactory.StudentMutatorFactory(randomNumberGenerator).Select(x => x.Name))
                    .ToList();

            var repeatedNames = names.GroupBy(a => a).Where(b => b.Count() > 1).Select(c => c.Key);
            repeatedNames.Count().ShouldBe(0);
        }
    }
}
