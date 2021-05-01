using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using EdFi.SampleDataGenerator.Core.Helpers;
using EdFi.SampleDataGenerator.Core.Serialization.Output;
using EdFi.SampleDataGenerator.Core.Serialization.Output.Interchanges;
using NUnit.Framework;
using Shouldly;

namespace EdFi.SampleDataGenerator.Core.UnitTests.Serialization.Output
{
    [TestFixture]
    public class StudentDataOutputBufferTester
    {
        [Test, TestCaseSource(nameof(StudentDataOutputBufferProperties))]
        public void ShouldBeConvertibleToEdFiBaseObjects(PropertyInfo property)
        {
            var hasDoNotOutputAttribute = !property.ShouldBeOutput();
            var hasOutputInfo = property.GetInterchangeOutputInfo() != null;
            var isSupportedCollectionType = property.PropertyType.IsSupportedInterchangeCollectionType();
            var collectionSubtypeHasOutputInfo = property.PropertyType.GetUnderlyingType().GetInterchangeOutputInfo() != null;

            var convertible = hasDoNotOutputAttribute || hasOutputInfo || (isSupportedCollectionType && collectionSubtypeHasOutputInfo);
            convertible.ShouldBeTrue();
        }

        [Test, TestCaseSource(nameof(StudentDataOutputBufferProperties))]
        public void AllPropertiesMustImplementISdgEntityOutputCollection(PropertyInfo propertyInfo)
        {
            propertyInfo.PropertyType.GetInterface(nameof(ISdgEntityOutputCollection)).ShouldNotBeNull();
        }

        [Test]
        public void AllPropertiesShouldBeInitializedUponConstruction()
        {
            var studentOutputBuffer = new StudentDataOutputBuffer();
            foreach (var studentDataOutputBufferProperty in StudentDataOutputBufferProperties)
            {
                var propertyValue = studentDataOutputBufferProperty.GetValue(studentOutputBuffer);
                propertyValue.ShouldNotBeNull($"{nameof(StudentDataOutputBuffer)} property {studentDataOutputBufferProperty.Name} should be initialized when the object is constructed");
            }
        }

        [Test]
        public void GeneratedStudentDataPropertiesShouldMapToAMemberOfBuffer()
        {
            var studentOutputBuffer = new StudentDataOutputBuffer();
            foreach (var studentDataOutputBufferProperty in StudentDataOutputBufferProperties)
            {
                var propertyValue = studentDataOutputBufferProperty.GetValue(studentOutputBuffer) as ISdgEntityOutputCollection;
                GeneratedStudentDataMembers
                    .Any(m => m.PropertyType == propertyValue?.SdgEntityType)
                    .ShouldBeTrue($"{nameof(StudentDataOutputBuffer)} property {studentDataOutputBufferProperty.Name} cannot be mapped to a corresponding property in class {nameof(GeneratedStudentData)}");
            }
        }

        private static readonly IEnumerable<PropertyInfo> StudentDataOutputBufferProperties = 
            typeof(StudentDataOutputBuffer)
                .GetProperties(BindingFlags.Public | BindingFlags.Instance);

        private static readonly PropertyInfo[] GeneratedStudentDataMembers =
            typeof (GeneratedStudentData)
                .GetProperties(BindingFlags.Public | BindingFlags.Instance);
    }
}
