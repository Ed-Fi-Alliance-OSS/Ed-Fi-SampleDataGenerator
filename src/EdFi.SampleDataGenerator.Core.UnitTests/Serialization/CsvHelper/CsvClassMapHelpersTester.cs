using System.Linq;
using CsvHelper.Configuration;
using EdFi.SampleDataGenerator.Core.Serialization.CsvHelper;
using NUnit.Framework;
using Shouldly;

namespace EdFi.SampleDataGenerator.Core.UnitTests.Serialization.CsvHelper
{
    public class TestClass
    {
        public int Prop { get; set; } = 1;
        public bool PropSpecified { get; set; }
        public NestedClass Nested { get; set; } = new NestedClass();
        public bool NestedSpecified { get; set; }
    }

    public class NestedClass
    {
        public int Prop { get; set; } = 2;
        public bool PropSpecified { get; set; }
        public DoubleNestedClass DoubleNested { get; set; } = new DoubleNestedClass();
    }

    public class DoubleNestedClass
    {
        public int Prop { get; set; } = 3;
        public TripleNestedClass TripleNested { get; set; } = new TripleNestedClass();
    }

    public class TripleNestedClass
    {
        public int Prop { get; set; } = 4;
    }

    public sealed class TestCsvClassMap : CsvClassMap<TestClass>
    {
        public TestCsvClassMap()
        {
            Map(x => x.Prop);
            References<ChildTestCsvClassMap>(x => x.Nested).RecursivePrefix();
        }
    }

    public sealed class ChildTestCsvClassMap : CsvClassMap<NestedClass>
    {
        public ChildTestCsvClassMap()
        {
            Map(x => x.Prop);
            References<GrandchildTestCsvClassMap>(x => x.DoubleNested).RecursivePrefix();
        }
    }

    public sealed class GrandchildTestCsvClassMap : CsvClassMap<DoubleNestedClass>
    {
        public GrandchildTestCsvClassMap()
        {
            Map(x => x.Prop);
            References<GreatGrandchildTestCsvClassMap>(x => x.TripleNested);
        }
    }

    public sealed class GreatGrandchildTestCsvClassMap : CsvClassMap<TripleNestedClass>
    {
        public GreatGrandchildTestCsvClassMap()
        {
            Map(x => x.Prop);
        }
    }
    
    [TestFixture]
    public class CsvClassMapHelpersTester
    {
        [Test]
        public void ShouldBuildPrefixesCorrectly()
        {
            var map = new TestCsvClassMap();
            var firstReferenceMap = map.ReferenceMaps.First();
            firstReferenceMap.Data.Prefix.ShouldBe("Nested.");

            var secondReferenceMap = firstReferenceMap.Data.Mapping.ReferenceMaps.First();
            secondReferenceMap.Data.Prefix.ShouldBe("Nested.DoubleNested.");

            var thirdReferenceMap = secondReferenceMap.Data.Mapping.ReferenceMaps.First();
            thirdReferenceMap.Data.Prefix.ShouldBe("Nested.DoubleNested.TripleNested.");

            var propertyMap = thirdReferenceMap.Data.Mapping.PropertyMaps.First();
            propertyMap.Data.Names.Prefix.ShouldBe("Nested.DoubleNested.TripleNested.");
        }

        [Test]
        public void ShouldSetPropertySpecified()
        {
            var map = new TestCsvClassMap();
            var entity = new TestClass();

            entity.Prop.ShouldBe(1);
            entity.PropSpecified.ShouldBeFalse();

            var entityCopy = (object) entity;
            map.PropertyMaps.First().SetProperty(ref entityCopy, -1);

            entity.Prop.ShouldBe(-1);
            entity.PropSpecified.ShouldBeTrue();
        }

        [Test]
        public void ShouldSetReferencePropertySpecified()
        {
            var map = new TestCsvClassMap();
            var entity = new TestClass();
            
            entity.NestedSpecified.ShouldBeFalse();

            map.ReferenceMaps.First().SetProperty(entity, new NestedClass());
            
            entity.NestedSpecified.ShouldBeTrue();
        }

        [Test]
        public void IsSpecifiedShouldBeNullIfPropertyDoesNotExist()
        {
            var testObject = new NestedClass();
            var doubleNestedProperty = typeof (NestedClass).GetProperty("DoubleNested");

            var isSpecified = CsvClassMapHelpers.GetIsSpecifiedMemberValue(doubleNestedProperty, testObject);
            isSpecified.HasValue.ShouldBeFalse();
        }

        [Test]
        public void IsSpecifiedShouldBeFalseIfPropertyExistsAndIsFalse()
        {
            var testObject = new NestedClass {PropSpecified = false};
            var doubleNestedProperty = typeof(NestedClass).GetProperty("Prop");

            var isSpecified = CsvClassMapHelpers.GetIsSpecifiedMemberValue(doubleNestedProperty, testObject);
            isSpecified.HasValue.ShouldBeTrue();
            isSpecified.Value.ShouldBeFalse();
        }

        [Test]
        public void IsSpecifiedShouldBeTrueIfPropertyExistsAndIsTrue()
        {
            var testObject = new NestedClass { PropSpecified = true };
            var doubleNestedProperty = typeof(NestedClass).GetProperty("Prop");

            var isSpecified = CsvClassMapHelpers.GetIsSpecifiedMemberValue(doubleNestedProperty, testObject);
            isSpecified.HasValue.ShouldBeTrue();
            isSpecified.Value.ShouldBeTrue();
        }
    }
}
