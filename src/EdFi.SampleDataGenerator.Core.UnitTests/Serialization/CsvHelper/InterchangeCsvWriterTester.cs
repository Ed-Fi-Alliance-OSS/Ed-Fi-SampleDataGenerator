using System;
using System.Collections.Generic;
using System.IO;
using CsvHelper.Configuration;
using EdFi.SampleDataGenerator.Core.Entities;
using EdFi.SampleDataGenerator.Core.Serialization.CsvHelper;
using EdFi.SampleDataGenerator.Core.Serialization.CsvHelper.EducationOrgCalendar;
using NUnit.Framework;
using Shouldly;

namespace EdFi.SampleDataGenerator.Core.UnitTests.Serialization.CsvHelper
{
    [TestFixture]
    public class InterchangeCsvWriterTester
    {
        [Test]
        public void ShouldNotWritePropertyWhenNotSpecified()
        {
            var entityToWrite = new ParentClass
            {
                Child = null,
                OtherChildren = null,
                OtherChildrenSpecified = false,
                Value = 1,
                ValueSpecified = false
            };

            var map = new ParentMap().RecursivePrefixReferencesMaps();

            using (var writer = new StringWriter())
            {
                var sut = new InterchangeCsvWriter(map, writer);
                sut.WriteRecords(new[] { entityToWrite });

                var result = writer.ToString();

                var expectedResult = @"Value,ArrayProperty,TestEnums,Child.ChildValue,Child.Child.Id,Child.Child.StringValue,OtherChildren.ChildValue,OtherChildren.Child.Id,OtherChildren.Child.StringValue
,,,,,,,,
";

                result.StripLineEndings().ShouldBe(expectedResult.StripLineEndings());
            }
        }

        [Test]
        public void ShouldWritePropertyWhenSpecified()
        {
            var entityToWrite = new ParentClass
            {
                Child = null,
                OtherChildren = null,
                OtherChildrenSpecified = false,
                Value = 1,
                ValueSpecified = true
            };

            var map = new ParentMap().RecursivePrefixReferencesMaps();

            using (var writer = new StringWriter())
            {
                var sut = new InterchangeCsvWriter(map, writer);
                sut.WriteRecords(new[] { entityToWrite });

                var result = writer.ToString();

                var expectedResult = @"Value,ArrayProperty,TestEnums,Child.ChildValue,Child.Child.Id,Child.Child.StringValue,OtherChildren.ChildValue,OtherChildren.Child.Id,OtherChildren.Child.StringValue
1,,,,,,,,
";

                result.StripLineEndings().ShouldBe(expectedResult.StripLineEndings());
            }
        }

        private class FormatTest
        {
            public double PrecisionTest { get; set; }
        }

        private sealed class FormatTestCsvClassMap : CsvClassMap<FormatTest>
        {
            public FormatTestCsvClassMap()
            {
                Map(x => x.PrecisionTest).TypeConverterOption.Format("0.0000");
            }
        }

        [Test]
        public void ShouldHonorMapFormatting()
        {
            var record = new FormatTest
            {
                PrecisionTest = 0.12345,
            };

            var otherRecord = new FormatTest
            {
                PrecisionTest = 0.12344,
            };

            var expectedResult = @"PrecisionTest
0.1235
0.1234
";

            var map = new FormatTestCsvClassMap();
            using (var writer = new StringWriter())
            {
                var sut = new InterchangeCsvWriter(map, writer);
                sut.WriteRecords(new[] { record, otherRecord });

                var result = writer.ToString();

                result.StripLineEndings().ShouldBe(expectedResult.StripLineEndings());
            }
        }

        [Test]
        public void ShouldWorkWithRecordArray()
        {
            var session = new Session
            {
                BeginDate = new DateTime(2018, 6, 13),
                SchoolYear = SchoolYearType.Item20172018,
                GradingPeriodReference = new[]
                {
                    new GradingPeriodReferenceType
                    {
                        id = "1",
                        @ref = "1",
                    },
                    new GradingPeriodReferenceType
                    {
                        id = "2",
                        @ref = "2",
                    },
                    new GradingPeriodReferenceType
                    {
                        id = "3",
                        @ref = "3",
                    }
                }
            };

            var expectedResult =
                "id,SessionName,SchoolYear,BeginDate,EndDate,TotalInstructionalDays,Term,id,ref,SchoolId,id,ref,GradingPeriod,PeriodSequence,SchoolYear,id,ref,SchoolId" +
                Environment.NewLine + ",,2017-2018,6/13/2018 12:00:00 AM,1/1/0001 12:00:00 AM,0,,,,,1,1,,,,,," + Environment.NewLine;

            if (RunningOnUnixLikeSystem)
                expectedResult =
                    "id,SessionName,SchoolYear,BeginDate,EndDate,TotalInstructionalDays,Term,id,ref,SchoolId,id,ref,GradingPeriod,PeriodSequence,SchoolYear,id,ref,SchoolId" +
                    Environment.NewLine + ",,2017-2018,06/13/2018 00:00:00,01/01/0001 00:00:00,0,,,,,1,1,,,,,," + Environment.NewLine;

            var map = new SessionCsvClassMap();
            using (var writer = new StringWriter())
            {
                var sut = new InterchangeCsvWriter(map, writer);
                sut.WriteRecords(new[] { session });

                var result = writer.ToString();

                result.ShouldBe(expectedResult);
            }
        }

        [Test]
        public void ShouldWorkWithPropertyMapArray()
        {

            var calendar = new CalendarDate
            {
                CalendarEvent = new[]
                 {
                     "Instructional day"
                 },
                Date = new DateTime(2018, 6, 13),
                id = "1"
            };

            var expectedResult =
                "id,CalendarEvent,Date,id,ref,CalendarCode,SchoolYear,id,ref,SchoolId" + Environment.NewLine +
                "1,Instructional day,6/13/2018 12:00:00 AM,,,,,,," + Environment.NewLine;
            
            if (RunningOnUnixLikeSystem)
                expectedResult =
                    "id,CalendarEvent,Date,id,ref,CalendarCode,SchoolYear,id,ref,SchoolId" + Environment.NewLine +
                    "1,Instructional day,06/13/2018 00:00:00,,,,,,," + Environment.NewLine;

            var map = new CalendarDateCsvClassMap();
            using (var writer = new StringWriter())
            {
                var sut = new InterchangeCsvWriter(map, writer);
                sut.WriteRecords(new[] { calendar });

                var result = writer.ToString();

                result.ShouldBe(expectedResult);
            }
        }

        private static bool RunningOnUnixLikeSystem
        {
            get { return (new List<int>() {4, 6, 128}).Contains((int) Environment.OSVersion.Platform); }
        }
    }
}
