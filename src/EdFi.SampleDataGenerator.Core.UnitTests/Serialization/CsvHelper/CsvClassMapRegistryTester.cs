using System;
using System.Collections.Generic;
using CsvHelper.Configuration;
using EdFi.SampleDataGenerator.Core.Entities;
using EdFi.SampleDataGenerator.Core.Serialization.CsvHelper;
using NUnit.Framework;
using Shouldly;

namespace EdFi.SampleDataGenerator.Core.UnitTests.Serialization.CsvHelper
{
    [TestFixture]
    public class CsvClassMapRegistryTester
    {
        [Test]
        public void ShouldBuildClassMapSuccessfully()
        {
            var maps = CsvClassMapRegistry.CsvClassMaps;
            maps.Length.ShouldBeGreaterThan(0);
        }

        [Test, TestCaseSource(nameof(GetManualClassMapTypes))]
        public void ShouldContainAllManualClassMaps(Type classMapType)
        {
            var mappedType = classMapType.CsvClassMapMappedType();
            CsvClassMapRegistry.MapFor(mappedType).ShouldNotBeNull();
        }

        [Test]
        public void ShouldGetUnderlyingMappedTypeGivenItsClassMapType()
        {
            typeof(SimpleInheritance).CsvClassMapMappedType().ShouldBe(typeof(Parent));

            typeof(ComplexInheritance).CsvClassMapMappedType().ShouldBe(typeof(Parent));

            Assert.Throws<ArgumentException>(() => typeof(string).CsvClassMapMappedType())
                .Message.ShouldBe("Expected a type implementing CsvHelper.Configuration.CsvClassMap`1[T], " +
                                  "but received System.String.");

            Assert.Throws<ArgumentException>(() => typeof(ComplexInheritance<,>).CsvClassMapMappedType())
                .Message.ShouldBe("Expected a type implementing CsvHelper.Configuration.CsvClassMap`1[T], " +
                                  "but received EdFi.SampleDataGenerator.Core.UnitTests.Serialization.CsvHelper" +
                                  ".CsvClassMapRegistryTester+ComplexInheritance`2.");
        }

        class SimpleInheritance : CsvClassMap<Parent> { }

        class ComplexInheritance<T1, T2> : CsvClassMap<T2> { }

        class ComplexInheritance : ComplexInheritance<int, Parent> { }

        private static IEnumerable<Type> GetManualClassMapTypes()
        {
            return CsvClassMapHelpers.ScanForCsvClassMapTypes();
        } 
    }
}
