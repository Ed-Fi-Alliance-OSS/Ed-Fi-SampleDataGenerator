using System;
using EdFi.SampleDataGenerator.Core.Entities;
using EdFi.SampleDataGenerator.Core.Helpers;
using NUnit.Framework;
using Shouldly;

namespace EdFi.SampleDataGenerator.Core.UnitTests.Helpers
{
    [TestFixture]
    public class ClassPeriodHelperTester
    {
        [Test]
        [TestCase("01 - Traditional")]
        [TestCase(" 01 - Traditional")]
        public void ShouldDetectValidClassPeriodName(string classPeriodName)
        {
            var classPeriod = new ClassPeriod
            {
                ClassPeriodName = classPeriodName
            };

            classPeriod.HasValidClassPeriodName().ShouldBeTrue();
        }

        [Test]
        [TestCase("First period")]
        [TestCase("")]
        public void ShouldDetectInvalidClassPeriodName(string classPeriodName)
        {
            var classPeriod = new ClassPeriod
            {
                ClassPeriodName = classPeriodName
            };

            classPeriod.HasValidClassPeriodName().ShouldBeFalse();
        }

        [Test]
        [TestCase("01 - Traditional")]
        [TestCase(" 01 - Traditional")]
        [TestCase("01 - Traditional 2nd grade")]
        public void ShouldGetNumericClassPeriod(string classPeriodName)
        {
            var classPeriod = new ClassPeriod
            {
                ClassPeriodName = classPeriodName
            };

            classPeriod.GetNumericClassPeriod().ShouldBe(1);
        }

        [Test]
        [TestCase("First period")]
        [TestCase("")]
        public void ShouldThrowOnInvalidClassPeriodName(string classPeriodName)
        {
            var classPeriod = new ClassPeriod
            {
                ClassPeriodName = classPeriodName
            };

            Assert.Throws<FormatException>(() => { classPeriod.GetNumericClassPeriod(); });
        }
    }
}
