using System;
using System.Collections.Generic;
using System.Linq;

namespace EdFi.SampleDataGenerator.Core.Helpers
{
    public static class DateTimeExtensions
    {
        public static bool IsWeekday(this DayOfWeek dayOfWeek)
        {
            return !dayOfWeek.IsWeekendDay();
        }

        public static bool IsWeekendDay(this DayOfWeek dayOfWeek)
        {
            return dayOfWeek == DayOfWeek.Saturday || dayOfWeek == DayOfWeek.Sunday;
        }

        public static bool IsWeekendDay(this DateTime date)
        {
            return date.DayOfWeek.IsWeekendDay();
        }

        public enum SearchDirection
        {
            Backward = -1,
            Forward = 1
        }

        public static DateTime FindClosestDayOfWeek(this DateTime startingDate, DayOfWeek dayOfWeek, SearchDirection searchDirection)
        {
            var result = startingDate;
            while (result.DayOfWeek != dayOfWeek)
            {
                result = result.AddDays((int)searchDirection);
            }

            return result;
        }

        public static DateTime FindNthDayOfWeekInMonth(this DateTime monthStart, DayOfWeek dayOfWeek, int dayOfWeekOrdinal)
        {
            if (dayOfWeekOrdinal <= 0)
                throw new ArgumentException("value should be 1-based index (i.e. > 0)", nameof(dayOfWeekOrdinal));

            var year = monthStart.Year;
            var month = monthStart.Month;
            var daysOfWeek = (from day in Enumerable.Range(1, DateTime.DaysInMonth(year, month))
                              let calendarDay = new DateTime(year, month, day)
                              where calendarDay.DayOfWeek == dayOfWeek
                              select calendarDay).ToList();

            if (dayOfWeekOrdinal <= daysOfWeek.Count)
                return daysOfWeek.ElementAt(dayOfWeekOrdinal - 1);

            throw new ArgumentException($"{dayOfWeekOrdinal}th {dayOfWeek} does not exist for {monthStart:MMMM yyyy}");
        }

        public static DateTime FindNextWeekday(this DateTime startDate)
        {
            var result = startDate.AddDays(1);
            while (result.IsWeekendDay())
            {
                result = result.AddDays(1);
            }

            return result;
        }

        public static DateTime FindNextNonHolidayWeekday(this DateTime startDate, IEnumerable<DateTime> holidays)
        {
            var result = startDate.Date;
            var holidayDates = holidays.Select(h => h.Date).ToList();

            while (holidayDates.Contains(result) || result.IsWeekendDay())
            {
                result = result.AddDays(1);
            }

            return result;
        }

        public static IEnumerable<DateTime> AllDatesUpTo(this DateTime startDate, DateTime endDate)
        {
            var currentDate = startDate.Date;
            while (currentDate <= endDate.Date)
            {
                yield return currentDate;
                currentDate = currentDate.AddDays(1);
            }
        } 

        public static DateTime AdjustForWeekendHoliday(this DateTime holiday)
        {
            switch (holiday.DayOfWeek)
            {
                case DayOfWeek.Saturday:
                    return holiday.AddDays(-1);
                case DayOfWeek.Sunday:
                    return holiday.AddDays(1);
                default:
                    return holiday;
            }
        }
        
        public static int CountConsecutiveDaysFrom(this IEnumerable<DateTime> dateList, DateTime startDate)
        {
            return dateList.OrderBy(d => d)
                .SkipWhile(d => d.Date < startDate.Date)
                .TakeWhile((d, index) => d.Date == startDate.Date.AddDays(index))
                .Count();
        }
    }
}
