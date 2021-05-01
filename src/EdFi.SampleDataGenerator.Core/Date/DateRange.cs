using System;

namespace EdFi.SampleDataGenerator.Core.Date
{
    public class DateRange
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public DateRange(DateTime startDate, DateTime endDate)
        {
            if (endDate < startDate)
                throw new InvalidOperationException("StartDate must be less than or equal to EndDate");

            StartDate = startDate;
            EndDate = endDate;
        }

        public bool Contains(DateTime dateTime)
        {
            return dateTime >= StartDate && dateTime <= EndDate;
        }

        public bool Contains(DateRange dateRange)
        {
            return dateRange != null && Contains(dateRange.StartDate) && Contains(dateRange.EndDate);
        }

        public bool Overlaps(DateRange dateRange)
        {
            return dateRange != null && 
            (
                dateRange.Contains(this) ||
                (this.Contains(dateRange.StartDate) || this.Contains(dateRange.EndDate))
            );
        }

        public bool StartsIn(DateRange dateRange)
        {
            return dateRange.Contains(StartDate);
        }

        public bool EndsIn(DateRange dateRange)
        {
            return dateRange.Contains(EndDate);
        }

        public DateRange Intersect(DateRange dateRange)
        {
            if (!Overlaps(dateRange))
                return null;

            var startDate = dateRange.StartDate > StartDate
                ? dateRange.StartDate
                : StartDate;

            var endDate = dateRange.EndDate < EndDate
                ? dateRange.EndDate
                : EndDate;

            return new DateRange(startDate, endDate);
        }
    }
}
