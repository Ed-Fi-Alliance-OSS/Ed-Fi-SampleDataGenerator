using EdFi.SampleDataGenerator.Core.Entities;
using NUnit.Framework;
using Shouldly;

namespace EdFi.SampleDataGenerator.Core.UnitTests.Descriptors
{
    [TestFixture]
    public class DescriptorTypeTester
    {
        [Test]
        public void EqualsShouldBeTrueWhenSameCodeValueAndSameNamespace()
        {
            var descriptor1 = new RaceDescriptor
            {
                CodeValue = "codeValue",
                Namespace = "namespace 1"
            };

            var descriptor2 = new GradeLevelDescriptor
            {
                CodeValue = "codeValue",
                Namespace = "namespace 1"
            };
            
            var result = descriptor1 == descriptor2;

            result.ShouldBe(true);
        }

        [Test]
        public void EqualsShouldBeTrueWhenComparingSameDescriptorObject()
        {
            var descriptor1 = GradeLevelDescriptor.FirstGrade;
            var descriptor2 = GradeLevelDescriptor.FirstGrade;

            var result = descriptor1 == descriptor2;

            result.ShouldBe(true);
        }

        [Test]
        public void EqualsShouldBeFalseWhenDifferentCodeValueAndSameNamespace()
        {
            var descriptor1 = GradeLevelDescriptor.FirstGrade;
            var descriptor2 = GradeLevelDescriptor.AdultEducation;

            var result = descriptor1 == descriptor2;

            result.ShouldBe(false);
        }

        [Test]
        public void EqualsShouldBeFalseWhenSameCodeValueAndDifferentNamespace()
        {
            var descriptor1 = new GradeLevelDescriptor
            {
                CodeValue = "codeValue",
                Namespace = "namespace 1"
            };

            var descriptor2 = new GradeLevelDescriptor
            {
                CodeValue = "codeValue",
                Namespace = "namespace 2"
            };

            var result = descriptor1 == descriptor2;

            result.ShouldBe(false);
        }

        [Test]
        public void EqualsShouldBeFalseWhenComparingDescriptorAndNull()
        {
            var descriptor1 = GradeLevelDescriptor.FirstGrade; 

            var result = descriptor1 == null;

            result.ShouldBe(false);
        }

        [Test]
        public void EqualsShouldBeTrueWhenBothNull()
        {
            GradeLevelDescriptor descriptor1 = null;

            var result = descriptor1 == null;

            result.ShouldBe(true);
        }

        [Test]
        public void InequalityShouldBeTrueWhenComparingDescriptorAndNull()
        {
            var descriptor1 = GradeLevelDescriptor.FirstGrade;

            var result = descriptor1 != null;

            result.ShouldBe(true);
        }

        
    }
}
