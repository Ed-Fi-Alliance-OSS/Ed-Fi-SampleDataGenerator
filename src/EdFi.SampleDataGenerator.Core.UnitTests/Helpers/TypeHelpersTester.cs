using System;
using System.CodeDom;
using System.Collections.Generic;
using EdFi.SampleDataGenerator.Core.Entities;
using EdFi.SampleDataGenerator.Core.Helpers;
using EdFi.SampleDataGenerator.Core.Serialization.Output.Interchanges;
using NUnit.Framework;
using Shouldly;

namespace EdFi.SampleDataGenerator.Core.UnitTests.Helpers
{
    [TestFixture]
    public class TypeHelpersTester
    {
        [Test]
        public void ReferenceTypeShouldBeNullable()
        {
            typeof(string).CanBeNull().ShouldBeTrue();
        }

        [Test]
        public void NullableTypeShouldBeNullable()
        {
            typeof(TypeHelpersTester).CanBeNull().ShouldBeTrue();
        }

        [Test]
        public void ValueTypeShouldNotBeNullable()
        {
            typeof(int).CanBeNull().ShouldBeFalse();
        }

        [TestCase(typeof(TypeHelpersTester[]), typeof(TypeHelpersTester))]
        [TestCase(typeof(List<TypeHelpersTester>), typeof(TypeHelpersTester))]
        [TestCase(typeof(StudentCollection), typeof(StudentData))]
        [TestCase(typeof(TypeHelpersTester), typeof(TypeHelpersTester))]
        public void ShouldGetUnderlyingType(Type testType, Type expectedUnderlyingType)
        {
            testType.GetUnderlyingType().ShouldBe(expectedUnderlyingType);
        }

        [TestCase(typeof(StudentCollection), typeof(ISdgEntityOutputCollection<>), typeof(ISdgEntityOutputCollection<StudentData>))]
        [TestCase(typeof(StudentCollection), typeof(ISdgEntityOutputCollection), null)] //not generic
        [TestCase(typeof(TypeHelpersTester), typeof(ISdgEntityOutputCollection<>), null)] //doesn't implement interface
        public void ShouldGetGenericInterface(Type testType, Type genericInterfaceType, Type expectedInterfaceType)
        {
            testType.GetGenericInterface(genericInterfaceType).ShouldBe(expectedInterfaceType);
        }

        [TestCase(typeof(StudentCollection), typeof(ISdgEntityOutputCollection<>), true)]
        [TestCase(typeof(StudentCollection), typeof(ISdgEntityOutputCollection), false)] //not generic
        [TestCase(typeof(TypeHelpersTester), typeof(ISdgEntityOutputCollection<>), false)] //doesn't implement interface
        public void ShouldDetectIfImplementsGenericInterface(Type testType, Type genericInterfaceType, bool expectedResult)
        {
            testType.ImplementsGenericInterface(genericInterfaceType).ShouldBe(expectedResult);
        }
    }
}
