using EdFi.SampleDataGenerator.Core.Config;
using NUnit.Framework;

namespace EdFi.SampleDataGenerator.Core.UnitTests.Config
{
    [TestFixture]
    public class MutatorValidatorTester : ValidatorTestBase<MutatorValidator, IMutatorConfiguration>
    {
        public MutatorValidatorTester() : base(new MutatorValidator())
        {
        }

        [Test]
        public void ShouldFailWithEmptyName()
        {
            var configuration = new TestMutatorConfiguration {Name = "", Probability = 0.5};
            Validate(configuration, false);
        }

        [Test]
        public void ShouldFailWhenProbabilityLessThanZero()
        {
            var configuration = new TestMutatorConfiguration { Name = "Test", Probability = -0.1 };
            Validate(configuration, false);
        }

        [Test]
        public void ShouldPassWhenProbabilityEqualsZero()
        {
            var configuration = new TestMutatorConfiguration { Name = "Test", Probability = 0 };
            Validate(configuration, true);
        }

        [Test]
        public void ShouldPassWhenProbabilityEqualsOne()
        {
            var configuration = new TestMutatorConfiguration { Name = "Test", Probability = 1 };
            Validate(configuration, true);
        }

        [Test]
        public void ShouldFailWhenProbabilityGreaterThanOne()
        {
            var configuration = new TestMutatorConfiguration { Name = "Test", Probability = 1.1 };
            Validate(configuration, false);
        }
    }
}
