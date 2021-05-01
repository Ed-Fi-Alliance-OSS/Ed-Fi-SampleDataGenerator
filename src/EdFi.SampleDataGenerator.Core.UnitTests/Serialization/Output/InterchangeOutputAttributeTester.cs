using System;
using System.Linq;
using System.Reflection;
using EdFi.SampleDataGenerator.Core.Serialization.Output;
using NUnit.Framework;
using Shouldly;

namespace EdFi.SampleDataGenerator.Core.UnitTests.Serialization.Output
{
    [TestFixture]
    public class InterchangeOutputAttributeTester
    {
        [Test]
        public void InterchangeOutputDataClassesShouldInitializeAllProperties()
        {
            var interchangeOutputDataClasses =
                typeof(InterchangeOutputAttribute)
                    .Assembly
                    .GetTypes()
                    .Where(type => type.GetCustomAttribute<InterchangeOutputAttribute>() != null)
                    .ToArray();

            foreach (var dataClass in interchangeOutputDataClasses)
            {
                var instance = Activator.CreateInstance(dataClass);

                var topLevelProperties = dataClass
                    .GetProperties(BindingFlags.Public | BindingFlags.Instance);

                foreach (var property in topLevelProperties)
                {
                    if (property.ShouldBeOutput())
                    {
                        var propertyValue = property.GetValue(instance);
                        propertyValue.ShouldNotBeNull($"{dataClass.FullName} property {property.Name} must be initialized when {dataClass.Name} is constructed, or must have the [{nameof(DoNotOutputToInterchangeAttribute)}] attribute applied.");
                    }
                }
            }
        }
    }
}