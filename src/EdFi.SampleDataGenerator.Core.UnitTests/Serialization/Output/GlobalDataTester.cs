using System.Collections.Generic;
using System.Reflection;
using EdFi.SampleDataGenerator.Core.Helpers;
using EdFi.SampleDataGenerator.Core.Serialization.Output;
using NUnit.Framework;
using Shouldly;

namespace EdFi.SampleDataGenerator.Core.UnitTests.Serialization.Output
{
    [TestFixture]
    public class GlobalDataTester
    {
        [Test, TestCaseSource(nameof(GlobalDataProperties))]
        public void ShouldBeConvertibleToEdFiBaseObjects(PropertyInfo property)
        {
            var hasDoNotOutputAttribute = !property.ShouldBeOutput();
            var hasOutputInfo = property.GetInterchangeOutputInfo() != null;
            var isSupportedCollectionType = property.PropertyType.IsSupportedInterchangeCollectionType();
            var collectionSubtypeHasOutputInfo = property.PropertyType.GetUnderlyingType().GetInterchangeOutputInfo() != null;

            var convertible = hasDoNotOutputAttribute || hasOutputInfo || (isSupportedCollectionType && collectionSubtypeHasOutputInfo);
            convertible.ShouldBeTrue();
        }

        private static IEnumerable<PropertyInfo> GlobalDataProperties() => typeof (GlobalData)
            .GetProperties(BindingFlags.Public | BindingFlags.Instance);
    }
}
