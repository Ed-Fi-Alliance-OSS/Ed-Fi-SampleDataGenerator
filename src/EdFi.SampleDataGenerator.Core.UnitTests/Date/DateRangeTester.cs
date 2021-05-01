using System;
using EdFi.SampleDataGenerator.Core.Date;
using NUnit.Framework;
using Shouldly;

namespace EdFi.SampleDataGenerator.Core.UnitTests.Date
{
    [TestFixture]
    public class DateRangeTester
    {
        [Test]
        public void ContainsShouldReturnTrueIfDateInsideRange()
        {
            var range = new DateRange(new DateTime(2017, 1, 1), new DateTime(2017, 1, 7));
            range.Contains(new DateTime(2017, 01, 02)).ShouldBeTrue();
        }

        [Test]
        public void ContainsShouldReturnTrueIfDateEqualsStartOfRange()
        {
            var range = new DateRange(new DateTime(2017, 1, 1), new DateTime(2017, 1, 7));
            range.Contains(new DateTime(2017, 1, 1)).ShouldBeTrue();
        }

        [Test]
        public void ContainsShouldReturnTrueIfDateEqualsEndOfRange()
        {
            var range = new DateRange(new DateTime(2017, 1, 1), new DateTime(2017, 1, 7));
            range.Contains(new DateTime(2017, 1, 7)).ShouldBeTrue();
        }

        [Test]
        public void ContainsShouldReturnFalseIfDateLessThanStartOfRange()
        {
            var range = new DateRange(new DateTime(2017, 1, 1), new DateTime(2017, 1, 7));
            range.Contains(new DateTime(2016, 12, 31, 23, 59, 59)).ShouldBeFalse();
        }

        [Test]
        public void ContainsShouldReturnFalseIfDateGreaterThanEndOfRange()
        {
            var range = new DateRange(new DateTime(2017, 1, 1), new DateTime(2017, 1, 7));
            range.Contains(new DateTime(2017, 1, 7, 0, 0, 1)).ShouldBeFalse();
        }

        [Test]
        public void ContainsShouldReturnTrueIfContainsFullInterval()
        {
            var range = new DateRange(new DateTime(2017, 1, 1), new DateTime(2017, 1, 7));
            var otherRange = new DateRange(new DateTime(2017, 1, 2), new DateTime(2017, 1, 6));
            range.Contains(otherRange).ShouldBeTrue();
        }

        [Test]
        public void ContainsShouldReturnFalseIfIntervalIsPastEndDate()
        {
            var range = new DateRange(new DateTime(2017, 1, 1), new DateTime(2017, 1, 7));
            var otherRange = new DateRange(new DateTime(2017, 1, 1), new DateTime(2017, 1, 7, 0, 0, 1));
            range.Contains(otherRange).ShouldBeFalse();
        }

        [Test]
        public void ContainsShouldReturnFalseIfIntervalIsBeforeBeginDate()
        {
            var range = new DateRange(new DateTime(2017, 1, 1), new DateTime(2017, 1, 7));
            var otherRange = new DateRange(new DateTime(2016, 12, 31, 23, 59, 59), new DateTime(2017, 1, 7));
            range.Contains(otherRange).ShouldBeFalse();
        }

        [Test]
        public void ContainsShouldBeFalseIfArgumentIsNull()
        {
            var range = new DateRange(new DateTime(2017, 1, 1), new DateTime(2017, 1, 7));
            range.Contains(null).ShouldBeFalse();
        }

        [Test]
        public void ShouldDetectOverlapAtStart()
        {
            var range = new DateRange(new DateTime(2017, 1, 1), new DateTime(2017, 1, 7));
            var otherRange = new DateRange(new DateTime(2016, 12, 31), new DateTime(2017, 1, 2));
            range.Overlaps(otherRange).ShouldBeTrue();
        }

        [Test]
        public void ShouldDetectOverlapAtEnd()
        {
            var range = new DateRange(new DateTime(2017, 1, 1), new DateTime(2017, 1, 7));
            var otherRange = new DateRange(new DateTime(2017, 1, 6), new DateTime(2017, 1, 8));
            range.Overlaps(otherRange).ShouldBeTrue();
        }

        [Test]
        public void ShouldDetectOverlapIfContainsFullRange()
        {
            var range = new DateRange(new DateTime(2017, 1, 1), new DateTime(2017, 1, 7));
            var otherRange = new DateRange(new DateTime(2017, 1, 2), new DateTime(2017, 1, 6));
            range.Overlaps(otherRange).ShouldBeTrue();
        }

        [Test]
        public void ShouldDetectOverlapIfOtherRangeContainsThisOne()
        {
            var range = new DateRange(new DateTime(2017, 1, 1), new DateTime(2017, 1, 7));
            var otherRange = new DateRange(new DateTime(2016, 12, 31), new DateTime(2017, 1, 8));
            range.Overlaps(otherRange).ShouldBeTrue();
        }

        [Test]
        public void ShouldNotDetectOverlapIfRangesAreDisparate()
        {
            var range = new DateRange(new DateTime(2017, 1, 1), new DateTime(2017, 1, 7));
            var otherRange = new DateRange(new DateTime(2017, 1, 7, 0, 0, 1), new DateTime(2017, 1, 8));
            range.Overlaps(otherRange).ShouldBeFalse();
        }

        [Test]
        public void OverlapShouldBeFalseIfArgumentIsNull()
        {
            var range = new DateRange(new DateTime(2017, 1, 1), new DateTime(2017, 1, 7));
            range.Overlaps(null).ShouldBeFalse();
        }

        [Test]
        public void ShouldThrowIfStartDateGreaterThanEndDate()
        {
            Assert.Throws<InvalidOperationException>(() => new DateRange(new DateTime(2017, 1, 1), new DateTime(2016, 12, 31)));
        }
    }
}
