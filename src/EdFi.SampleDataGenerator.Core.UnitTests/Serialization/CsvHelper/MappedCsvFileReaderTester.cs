using System;
using System.IO;
using CsvHelper.Configuration;
using EdFi.SampleDataGenerator.Core.Entities;
using EdFi.SampleDataGenerator.Core.Serialization.CsvHelper;
using NUnit.Framework;
using Shouldly;

namespace EdFi.SampleDataGenerator.Core.UnitTests.Serialization.CsvHelper
{
    [TestFixture]
    public class MappedCsvFileReaderTester
    {
        private class UnmappedType
        {
            public int Test { get; set; }
        }

        private class MappedType
        {
            public int Num { get; set; }
            public string Str { get; set; }
            public string Gender { get; set; }
        }

        private sealed class MappedTypeCsvClassMap : CsvClassMap<MappedType>
        {
            public MappedTypeCsvClassMap()
            {
                AutoMap();
                Map(x => x.Gender);
            }
        }

        [Test]
        public void ShouldThrowTryingToReadUnmappedType()
        {
            using (var reader = new StringReader(""))
            {
                Assert.Throws<ArgumentException>(() => MappedCsvFileReader.ReadEntityFile<UnmappedType>(reader));
            }
        }

        [Test]
        public void ShouldThrowIfClassMapIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => MappedCsvFileReader.ReadEntityFile<MappedType>(TextReader.Null, null));
        }

        [Test]
        public void ShouldReadMappedEntities()
        {
            var fileContent = @"Num,Str,Gender
100,blah blah blah,Female";

            using (var stringReader = new StringReader(fileContent))
            {
                var records = MappedCsvFileReader.ReadEntityFile<MappedType>(stringReader, new MappedTypeCsvClassMap());

                records.ShouldNotBeNull();
                records.Count.ShouldBe(1);

                var record = records[0];
                record.Num.ShouldBe(100);
                record.Str.ShouldBe("blah blah blah");
                record.Gender.ShouldBe(SexDescriptor.Female.CodeValue);
            }
        }
    }
}
