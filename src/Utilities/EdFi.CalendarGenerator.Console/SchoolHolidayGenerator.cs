using System;
using System.Collections.Generic;
using EdFi.SampleDataGenerator.Core.Helpers;

namespace EdFi.CalendarGenerator.Console
{
    public static class SchoolHolidayGenerator
    {
        /// <summary>
        /// Generates "normal" set of School holidays / breaks.  The resulting list *will* include weekend days for long breaks
        /// that cover more than one week (e.g. winter break)
        /// </summary>
        /// <param name="year">Calendar year </param>
        /// <returns>List of dates considered holidays on the school calendar</returns>
        public static IEnumerable<DateTime> GetHolidays(int year)
        {
            //New Year's / end of Winter/Christmas break
            var newYearsDay = new DateTime(year, 1, 1);
            var winterBreakEnd = newYearsDay.FindClosestDayOfWeek(DayOfWeek.Monday, DateTimeExtensions.SearchDirection.Forward);
            foreach (var date in newYearsDay.AllDatesUpTo(winterBreakEnd))
            {
                yield return date;
            }
            
            //Memorial Day - last Monday in May 
            yield return new DateTime(year, 5, 31).FindClosestDayOfWeek(DayOfWeek.Monday, DateTimeExtensions.SearchDirection.Backward).Date;

            //4th of July
            yield return new DateTime(year, 7, 4).AdjustForWeekendHoliday().Date;

            //Labor day - 1st Monday in September 
            yield return new DateTime(year, 9, 1).FindClosestDayOfWeek(DayOfWeek.Monday, DateTimeExtensions.SearchDirection.Forward).Date;

            //Thanksgiving - 4th Thursday in November 
            var thanksgivingDay = new DateTime(year, 11, 1).FindNthDayOfWeekInMonth(DayOfWeek.Thursday, 4).Date;
            yield return thanksgivingDay;
            yield return thanksgivingDay.AddDays(1).Date;

            //Winter/Christmas break
            var winterBreakStart = new DateTime(year, 12, 24).FindClosestDayOfWeek(DayOfWeek.Monday, DateTimeExtensions.SearchDirection.Backward);
            foreach (var date in winterBreakStart.AllDatesUpTo(new DateTime(year, 12, 31)))
            {
                yield return date;
            }
        }
    }
}
