using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using CsvHelper.Configuration;
using EdFi.SampleDataGenerator.Core.Entities;
using EdFi.SampleDataGenerator.Core.Helpers;
using EdFi.SampleDataGenerator.Core.Serialization.CsvHelper;
using NUnit.Framework;
using Shouldly;

namespace EdFi.SampleDataGenerator.Core.UnitTests.Serialization.CsvHelper
{
    [TestFixture]
    public class MappedCsvFileWriterTester
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
        public void ShouldThrowTryingToWriteUnmappedType()
        {
            var outputObjects = new List<UnmappedType>();
            using (var writer = new StringWriter(new StringBuilder()))
            {
                Assert.Throws<ArgumentException>(() => MappedCsvFileWriter.WriteEntityFile<UnmappedType>(writer, outputObjects));
            }
        }

        [Test]
        public void ShouldThrowIfClassMapIsNull()
        {
            var outputObjects = new List<MappedType>();
            using (var writer = new StringWriter(new StringBuilder()))
            {
                Assert.Throws<ArgumentNullException>(() => MappedCsvFileWriter.WriteEntityFile<MappedType>(writer, null, outputObjects));
            }
        }

        [Test]
        public void ShouldWriteMappedEntities()
        {
            var outputRecord = new MappedType
            {
                Num = 100,
                Str = "blah blah blah",
                Gender = SexDescriptor.Female.CodeValue
            };

            var expectedFileContent = @"Num,Str,Gender
100,blah blah blah,Female
";

            var stringBuilder = new StringBuilder();
            using (var writer = new StringWriter(stringBuilder))
            {
                MappedCsvFileWriter.WriteEntityFile<MappedType>(writer, new MappedTypeCsvClassMap(), outputRecord.Yield());
            }

            var result = stringBuilder.ToString();
            result.StripLineEndings().ShouldBe(expectedFileContent.StripLineEndings());
        }
    }
}
