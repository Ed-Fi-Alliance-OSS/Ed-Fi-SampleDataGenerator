using EdFi.SampleDataGenerator.Core.Config;
using NUnit.Framework;
using Shouldly;

namespace EdFi.SampleDataGenerator.Core.UnitTests.Config
{
    [TestFixture]
    public class AttributeConfigurationValidatorTester
    {
        [Test]
        public void ShouldFailWhenValuesSumToLessThan1()
        {
            var config = new TestAttributeConfiguration
            {
                Name = "Test",
                AttributeGeneratorConfigurationOptions = new[]
                {
                    new TestAttributeGeneratorConfigurationOption {Frequency = 0.10, Value = "Test 1"},
                    new TestAttributeGeneratorConfigurationOption {Frequency = 0.20, Value = "Test 2"},
                    new TestAttributeGeneratorConfigurationOption {Frequency = 0.30, Value = "Test 3"},
                }
            };

            var validator = new AttributeConfigurationValidator("", true);
            var result = validator.Validate(config);
            result.IsValid.ShouldBeFalse();
        }

        [Test]
        public void ShouldFailWhenValuesSumToGreaterThan1()
        {
            var config = new TestAttributeConfiguration
            {
                Name = "Test",
                AttributeGeneratorConfigurationOptions = new[]
                {
                    new TestAttributeGeneratorConfigurationOption {Frequency = 0.30, Value = "Test 1"},
                    new TestAttributeGeneratorConfigurationOption {Frequency = 0.40, Value = "Test 2"},
                    new TestAttributeGeneratorConfigurationOption {Frequency = 0.31, Value = "Test 3"},
                }
            };

            var validator = new AttributeConfigurationValidator("", true);
            var result = validator.Validate(config);
            result.IsValid.ShouldBeFalse();
        }

        [Test]
        public void ShouldFailWhenOptionsEmpty()
        {
            var config = new TestAttributeConfiguration
            {
                Name = "Test",
                AttributeGeneratorConfigurationOptions = new IAttributeGeneratorConfigurationOption[] {}
            };

            var validator = new AttributeConfigurationValidator("", true);
            var result = validator.Validate(config);
            result.IsValid.ShouldBeFalse();
        }

        [Test]
        public void ShouldFailWithEmptyName()
        {
            var config = new TestAttributeConfiguration
            {
                Name = "",
                AttributeGeneratorConfigurationOptions = new []
                {
                    new TestAttributeGeneratorConfigurationOption { Frequency = 1.0, Value = "Test 1" }
                }
            };

            var validator = new AttributeConfigurationValidator("", false);
            var result = validator.Validate(config);
            result.IsValid.ShouldBeFalse();
        }

        [Test]
        public void ShouldPassWithValidConfig()
        {
            var config = new TestAttributeConfiguration
            {
                Name = "Test",
                AttributeGeneratorConfigurationOptions = new[]
                {
                    new TestAttributeGeneratorConfigurationOption { Frequency = 1.0, Value = "Test 1" }
                }
            };

            var validator = new AttributeConfigurationValidator("", true);
            var result = validator.Validate(config);
            result.IsValid.ShouldBeTrue();
        }

        [Test]
        public void ShouldPassIfSumLessThan1WhenConfigured()
        {
            var config = new TestAttributeConfiguration
            {
                Name = "Test",
                AttributeGeneratorConfigurationOptions = new[]
                {
                    new TestAttributeGeneratorConfigurationOption { Frequency = 0.01, Value = "Test 1" }
                }
            };

            var validator = new AttributeConfigurationValidator("", false);
            var result = validator.Validate(config);
            result.IsValid.ShouldBeTrue();
        }
    }

    [TestFixture]
    public class AttributeGeneratorConfigurationOptionValidatorTester
    {
        [Test]
        public void ShouldFailWithEmptyName()
        {
            var option = new TestAttributeGeneratorConfigurationOption
            {
                Frequency = 1.0,
                Value = ""
            };

            var validator = new AttributeGeneratorConfigurationOptionValidator("", "");
            var result = validator.Validate(option);
            result.IsValid.ShouldBeFalse();
        }

        [Test]
        public void ShouldFailWithValueEqualTo0()
        {
            var option = new TestAttributeGeneratorConfigurationOption
            {
                Frequency = 0,
                Value = "Test"
            };

            var validator = new AttributeGeneratorConfigurationOptionValidator("", "");
            var result = validator.Validate(option);
            result.IsValid.ShouldBeFalse();
        }

        [Test]
        public void ShouldFailWithValueGreaterThan1()
        {
            var option = new TestAttributeGeneratorConfigurationOption
            {
                Frequency = 1.01,
                Value = "Test"
            };

            var validator = new AttributeGeneratorConfigurationOptionValidator("", "");
            var result = validator.Validate(option);
            result.IsValid.ShouldBeFalse();
        }
    }
}
