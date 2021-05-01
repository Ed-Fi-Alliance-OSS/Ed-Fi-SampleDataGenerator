using System.IO;
using System.Linq;
using CsvHelper.Configuration;
using EdFi.SampleDataGenerator.Core.Serialization.CsvHelper;
using EdFi.SampleDataGenerator.Core.UnitTests.Helpers;
using NUnit.Framework;
using Shouldly;

namespace EdFi.SampleDataGenerator.Core.UnitTests.Serialization.CsvHelper
{
    [TestFixture]
    public class InterchangeCsvReaderTester
    {
        [Test]
        public void ShouldThrowOnInvalidDataTypeInColumnValues()
        {
            var data = @"Value,Child.ChildValue,Child.Child.Id,OtherChildren.ChildValue,OtherChildren.Child.Id,OtherChildren.Child.StringValue
baddata,2,3,4,5,blah";
            using (var reader = new StringReader(data))
            {
                var map = new ParentMap().RecursivePrefixReferencesMaps();
                var sut = new InterchangeCsvReader(map, reader);
                Assert.Throws<CsvException>(() =>
                {
                    var records = sut.ReadRecords(typeof (ParentClass)).ToList();
                });
            }
        }

        [Test]
        public void ShouldSetSpecifiedProperty()
        {
            var data = @"Value,Child.ChildValue,Child.Child.Id,OtherChildren.ChildValue,OtherChildren.Child.Id,OtherChildren.Child.StringValue
1,2,3,4,5,blah";
            using (var reader = new StringReader(data))
            {
                var map = new ParentMap().RecursivePrefixReferencesMaps();
                var sut = new InterchangeCsvReader(map, reader);
                var records = sut.ReadRecords(typeof (ParentClass)).ToList();

                records.ShouldNotBeNull();
                records.Count.ShouldBe(1);

                var record = (ParentClass)records.First();
                record.Value.ShouldBe(1);
                record.ValueSpecified.ShouldBeTrue();
            }
        }

        [Test]
        public void ShouldSetSpecifiedPropertyForArrayType()
        {
            var data = @"Value,Child.ChildValue,Child.Child.Id,OtherChildren.ChildValue,OtherChildren.Child.Id,OtherChildren.Child.StringValue
1,2,3,4,5,blah";
            using (var reader = new StringReader(data))
            {
                var map = new ParentMap().RecursivePrefixReferencesMaps();
                var sut = new InterchangeCsvReader(map, reader);
                var records = sut.ReadRecords(typeof(ParentClass)).ToList();

                records.ShouldNotBeNull();
                records.Count.ShouldBe(1);

                var record = (ParentClass)records.First();
                record.OtherChildren.ShouldNotBeNull();
                record.OtherChildren.Length.ShouldBe(1);
                record.OtherChildrenSpecified.ShouldBeTrue();
            }
        }

        [Test]
        public void ShouldLeaveEntityNullIfAllPropertyColumnsAreEmpty()
        {
            var data = @"Value,Child.ChildValue,Child.Child.Id,OtherChildren.ChildValue,OtherChildren.Child.Id,OtherChildren.Child.StringValue
1,,,4,5,blah";
            using (var reader = new StringReader(data))
            {
                var map = new ParentMap().RecursivePrefixReferencesMaps();
                var sut = new InterchangeCsvReader(map, reader);
                var records = sut.ReadRecords(typeof(ParentClass)).ToList();

                records.ShouldNotBeNull();
                records.Count.ShouldBe(1);

                var record = (ParentClass)records.First();
                record.Child.ShouldBeNull();
            }
        }

        [Test]
        public void ShouldNotReadArrayOfValueType()
        {
            var data = @"ArrayProperty[0],ArrayProperty[1]
0,1";
            using (var reader = new StringReader(data))
            {
                var map = new ParentMap().RecursivePrefixReferencesMaps();
                var sut = new InterchangeCsvReader(map, reader);
                Assert.Throws<CsvException>(() => { var records = sut.ReadRecords(typeof (ParentClass)).ToList(); });
            }
        }

        [Test]
        public void ShouldLeaveArrayNullIfAllArrayColumnsEmpty()
        {
            var data = @"Value,Child.ChildValue,Child.Child.Id,OtherChildren.ChildValue,OtherChildren.Child.Id
1,2,3,,";
            using (var reader = new StringReader(data))
            {
                var map = new ParentMap().RecursivePrefixReferencesMaps();
                var sut = new InterchangeCsvReader(map, reader);
                var records = sut.ReadRecords(typeof(ParentClass)).ToList();

                records.ShouldNotBeNull();
                records.Count.ShouldBe(1);

                var record = (ParentClass)records.First();
                record.OtherChildren.ShouldBeNull();
                record.OtherChildrenSpecified.ShouldBeFalse();
            }
        }

        [Test]
        public void ShouldCreateArrayIfAnyArrayColumnIsNotEmpty()
        {
            var data = @"Value,Child.ChildValue,Child.Child.Id,OtherChildren.ChildValue,OtherChildren.Child.Id,OtherChildren.Child.StringValue
1,2,3,,5,";
            using (var reader = new StringReader(data))
            {
                var map = new ParentMap().RecursivePrefixReferencesMaps();
                var sut = new InterchangeCsvReader(map, reader);
                var records = sut.ReadRecords(typeof(ParentClass)).ToList();

                records.ShouldNotBeNull();
                records.Count.ShouldBe(1);

                var record = (ParentClass)records.First();
                record.OtherChildren.ShouldNotBeNull();
                record.OtherChildrenSpecified.ShouldBeTrue();
                record.OtherChildren.Length.ShouldBe(1);
                record.OtherChildren[0].Child.ShouldNotBeNull();
                record.OtherChildren[0].ChildSpecified.ShouldBeTrue();
            }
        }

        [Test]
        public void ShouldCreateArrayIfAnyNestedArrayColumnIsNotEmpty()
        {
            var data = @"Value,Child.ChildValue,Child.Child.Id,OtherChildren.ChildValue,OtherChildren.Child.Id,OtherChildren.Child.StringValue
1,2,3,,,blah";
            using (var reader = new StringReader(data))
            {
                var map = new ParentMap().RecursivePrefixReferencesMaps();
                var sut = new InterchangeCsvReader(map, reader);
                var records = sut.ReadRecords(typeof(ParentClass)).ToList();

                records.ShouldNotBeNull();
                records.Count.ShouldBe(1);

                var record = (ParentClass)records.First();
                record.OtherChildren.ShouldNotBeNull();
                record.OtherChildrenSpecified.ShouldBeTrue();
                record.OtherChildren.Length.ShouldBe(1);
                record.OtherChildren[0].Child.ShouldNotBeNull();
                record.OtherChildren[0].ChildSpecified.ShouldBeTrue();
                record.OtherChildren[0].Child.StringValue.ShouldBe("blah");
            }
        }

        [Test]
        public void ShouldReadSingleArrayRecordWithZeroIndex()
        {
            var data = @"Value,Child.ChildValue,Child.Child.Id,OtherChildren[0].ChildValue,OtherChildren[0].Child.Id,OtherChildren[0].Child.StringValue
1,2,3,,,blah";
            using (var reader = new StringReader(data))
            {
                var map = new ParentMap().RecursivePrefixReferencesMaps();
                var sut = new InterchangeCsvReader(map, reader);
                var records = sut.ReadRecords(typeof(ParentClass)).ToList();

                records.ShouldNotBeNull();
                records.Count.ShouldBe(1);

                var record = (ParentClass)records.First();
                record.OtherChildren.ShouldNotBeNull();
                record.OtherChildrenSpecified.ShouldBeTrue();
                record.OtherChildren.Length.ShouldBe(1);
                record.OtherChildren[0].Child.ShouldNotBeNull();
                record.OtherChildren[0].ChildSpecified.ShouldBeTrue();
                record.OtherChildren[0].Child.StringValue.ShouldBe("blah");
            }
        }

        [Test]
        public void ShouldReadMultipleRecordsIntoSameArray()
        {
            var data = @"Value,Child.ChildValue,Child.Child.Id,OtherChildren[0].ChildValue,OtherChildren[0].Child.Id,OtherChildren[0].Child.StringValue,OtherChildren[1].ChildValue,OtherChildren[1].Child.Id,OtherChildren[1].Child.StringValue
1,2,3,,,blah,,,blahblah";
            using (var reader = new StringReader(data))
            {
                var map = new ParentMap().RecursivePrefixReferencesMaps();
                var sut = new InterchangeCsvReader(map, reader);
                var records = sut.ReadRecords(typeof(ParentClass)).ToList();

                records.ShouldNotBeNull();
                records.Count.ShouldBe(1);

                var record = (ParentClass)records.First();
                record.OtherChildren.ShouldNotBeNull();
                record.OtherChildrenSpecified.ShouldBeTrue();
                record.OtherChildren.Length.ShouldBe(2);
                record.OtherChildren[0].Child.ShouldNotBeNull();
                record.OtherChildren[0].ChildSpecified.ShouldBeTrue();
                record.OtherChildren[0].Child.StringValue.ShouldBe("blah");
                record.OtherChildren[1].Child.ShouldNotBeNull();
                record.OtherChildren[1].ChildSpecified.ShouldBeTrue();
                record.OtherChildren[1].Child.StringValue.ShouldBe("blahblah");
            }
        }

        [Test]
        public void ShouldNotReadNonConsecutiveIndexes()
        {
            var data = @"Value,Child.ChildValue,Child.Child.Id,OtherChildren[0].ChildValue,OtherChildren[0].Child.Id,OtherChildren[0].Child.StringValue,OtherChildren[2].ChildValue,OtherChildren[2].Child.Id,OtherChildren[2].Child.StringValue
1,2,3,,,blah,,,blahblah";
            using (var reader = new StringReader(data))
            {
                var map = new ParentMap().RecursivePrefixReferencesMaps();
                var sut = new InterchangeCsvReader(map, reader);
                var records = sut.ReadRecords(typeof(ParentClass)).ToList();

                records.ShouldNotBeNull();
                records.Count.ShouldBe(1);

                var record = (ParentClass)records.First();
                record.OtherChildren.ShouldNotBeNull();
                record.OtherChildrenSpecified.ShouldBeTrue();
                record.OtherChildren.Length.ShouldBe(1);
                record.OtherChildren[0].Child.ShouldNotBeNull();
                record.OtherChildren[0].ChildSpecified.ShouldBeTrue();
                record.OtherChildren[0].Child.StringValue.ShouldBe("blah");
            }
        }



        [Test]
        public void ShouldReadMultipleEntriesForArrayOfEnums()
        {
            var data = @"TestEnums[0],TestEnums[1]
Free,Full price";

            using (var reader = new StringReader(data))
            {
                var map = new ParentMap().RecursivePrefixReferencesMaps();
                var sut = new InterchangeCsvReader(map, reader);
                var records = sut.ReadRecords(typeof(ParentClass)).ToList();

                records.ShouldNotBeNull();
                records.Count.ShouldBe(1);

                var record = (ParentClass)records.First();
                record.TestEnums.ShouldNotBeNull();
                record.TestEnums.Length.ShouldBe(2);
                record.TestEnums[0].ShouldBe(TestEnum.Free);
                record.TestEnums[1].ShouldBe(TestEnum.Fullprice);
            }
        }
    }
    
    public class ParentClass
    {
        public int Value { get; set; }
        public bool ValueSpecified { get; set; }
        public int[] ArrayProperty { get; set; }
        public TestEnum[] TestEnums { get; set; }
        public ChildClass Child { get; set; }
        public ChildClass[] OtherChildren { get; set; }
        public bool OtherChildrenSpecified { get; set; }
    }


    public class ChildClass
    {
        public int ChildValue { get; set; }
        public GrandchildClass Child { get; set; }
        public bool ChildSpecified { get; set; }
    }

    public class GrandchildClass
    {
        public int Id { get; set; }
        public string StringValue { get; set; }
    }

    public sealed class ParentMap : CsvClassMap<ParentClass>
    {
        public ParentMap()
        {
            Map(m => m.Value);
            Map(m => m.ArrayProperty);
            Map(m => m.TestEnums).ConvertEnumerationType();
            References<ChildMap>(m => m.Child);
            References<ChildMap>(m => m.OtherChildren);
        }
    }

    public sealed class ChildMap : CsvClassMap<ChildClass>
    {
        public ChildMap()
        {
            Map(m => m.ChildValue);
            References<GrandchildMap>(m => m.Child);
        }
    }

    public sealed class GrandchildMap : CsvClassMap<GrandchildClass>
    {
        public GrandchildMap()
        {
            Map(m => m.Id);
            Map(m => m.StringValue);
        }
    }
}
