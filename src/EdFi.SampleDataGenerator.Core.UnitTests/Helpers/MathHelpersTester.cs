using EdFi.SampleDataGenerator.Core.Helpers;
using NUnit.Framework;
using Shouldly;

namespace EdFi.SampleDataGenerator.Core.UnitTests.Helpers
{
    [TestFixture]
    public class MathHelpersTester
    {
        [Test]
        public void ShouldDetectOverflow()
        {
            MathHelpers.AdditionWillOverflowInteger(int.MaxValue, 1).ShouldBeTrue();
        }

        [Test]
        public void ShouldNotDetectOverflowAtMaxValue()
        {
            MathHelpers.AdditionWillOverflowInteger(int.MaxValue-1, 1).ShouldBeFalse();
        }

        [Test]
        public void ShouldDetectUnderflow()
        {
            MathHelpers.AdditionWillOverflowInteger(int.MinValue, -1).ShouldBeTrue();
        }

        [Test]
        public void ShoudlNotDetectUnderflowAtMinValue()
        {
            MathHelpers.AdditionWillOverflowInteger(int.MinValue+1, -1).ShouldBeFalse();
        }
    }
}
