using System;
using System.Collections.Generic;
using System.Linq;
using EdFi.SampleDataGenerator.Core.Entities;
using EdFi.SampleDataGenerator.Core.Helpers;

namespace EdFi.CalendarGenerator.Console
{
    public class SchoolYearTemplateGenerator
    {
        private const int WeeksToGenerate = 36;
        private readonly Random _rng = new Random();
        
        public SchoolYearTemplate Generate(CalendarGeneratorConfig config)
        {
            var termCount = GetTermCount(config);
            var gradingPeriodsPerTerm = GetGradingPeriodsPerTerm(config);
            var nextGradingPeriodStartDate = config.SchoolYearStartDate;

            var holidays = Enumerable.Concat(SchoolHolidayGenerator.GetHolidays(config.SchoolYearStartDate.Year), SchoolHolidayGenerator.GetHolidays(config.SchoolYearStartDate.Year + 1))
                    .ToList();

            var schoolTerms = new SchoolYearTemplate();

            var gradingPeriodNumber = 1;

            for (var i = 0; i < termCount; ++i)
            {
                var term = new TermTemplate
                {
                    TermNumber = i + 1
                };

                for (var j = 0; j < gradingPeriodsPerTerm; j++)
                {
                    var templateGradingPeriod = GenerateTemplateGradingPeriod(config, nextGradingPeriodStartDate, gradingPeriodNumber, holidays);
                    
                    nextGradingPeriodStartDate = templateGradingPeriod.EndDateIncludingHolidays
                        .AddDays(1)
                        .FindNextNonHolidayWeekday(holidays);

                    term.GradingPeriods.Add(templateGradingPeriod);

                    ++gradingPeriodNumber;
                }

                schoolTerms.Terms.Add(term);
            }

            return schoolTerms;
        }
        
        private int GetTermCount(CalendarGeneratorConfig config)
        {
            switch (config.TermType)
            {
                case TermType.Semester:
                    return 2;

                default:
                    return 2;
            }
        }

        private int GetGradingPeriodsPerTerm(CalendarGeneratorConfig config)
        {
            return (WeeksToGenerate / config.GradingPeriodLengthInWeeks) / GetTermCount(config);
        }

        private GradingPeriodTemplate GenerateTemplateGradingPeriod(CalendarGeneratorConfig config, DateTime startDate, int gradingPeriodNumber, List<DateTime> holidays)
        {
            var result = new GradingPeriodTemplate
            {
                GradingPeriodNumber = gradingPeriodNumber
            };

            var totalCalendarDays = config.GradingPeriodLengthInWeeks*5;
            var totalDaysGenerated = 0;
            var totalBadWeatherDaysGenerated = 0;
            var totalTeacherWorkdaysGenerated = 0;
            var currentDate = startDate;
            
            while (totalDaysGenerated < totalCalendarDays)
            {
                if (currentDate.IsWeekendDay())
                {
                    currentDate = currentDate.AddDays(1);
                    continue;
                }

                if (holidays.Contains(currentDate))
                {
                    var calendarDaysLeftToGenerate = totalCalendarDays - totalDaysGenerated;

                    //check if the holiday we're adding is at the end of the grading period (like the start of winter break)
                    //if so, we'll add the holidays as AdditionalHolidays below
                    if (holidays.CountConsecutiveDaysFrom(currentDate) > calendarDaysLeftToGenerate)
                    {
                        break;
                    }

                    result.CalendarDates.Add(CreateCalendarDate(currentDate, CalendarEventDescriptor.Holiday));
                }

                else if (totalBadWeatherDaysGenerated < config.BadWeatherDaysPerGradingPeriod && GenerateBadWeatherDay(config, currentDate))
                {
                    ++totalBadWeatherDaysGenerated;
                    result.CalendarDates.Add(CreateCalendarDate(currentDate, CalendarEventDescriptor.WeatherDay));
                }

                else if (totalTeacherWorkdaysGenerated < config.TeacherWorkdaysPerGradingPeriod && GenerateTeacherWorkDay(config, currentDate))
                {
                    ++totalTeacherWorkdaysGenerated;
                    result.CalendarDates.Add(CreateCalendarDate(currentDate, CalendarEventDescriptor.TeacherOnlyDay));
                }

                else
                {
                    result.CalendarDates.Add(CreateCalendarDate(currentDate, CalendarEventDescriptor.InstructionalDay));
                }

                ++totalDaysGenerated;
                currentDate = currentDate.AddDays(1);
            }
            
            //Any leftover holidays from the current date are added as "Additional holidays" on the grading period
            //this is primarily to capture winter and spring break
            while (holidays.Contains(currentDate))
            {
                if (!currentDate.IsWeekendDay())
                {
                    result.AdditionalHolidays.Add(CreateCalendarDate(currentDate, CalendarEventDescriptor.Holiday));
                }
                
                currentDate = currentDate.AddDays(1);
            }

            return result;
        }

        private bool GenerateBadWeatherDay(CalendarGeneratorConfig config, DateTime date)
        {
            if (date.DayOfWeek != DayOfWeek.Monday && date.DayOfWeek != DayOfWeek.Friday) return false;

            var badWeatherDaysPerWeek = (float)config.BadWeatherDaysPerGradingPeriod / (float)config.GradingPeriodLengthInWeeks;
            return _rng.NextDouble() / 2 <= badWeatherDaysPerWeek;
        }

        private bool GenerateTeacherWorkDay(CalendarGeneratorConfig config, DateTime date)
        {
            if (date.DayOfWeek != DayOfWeek.Monday && date.DayOfWeek != DayOfWeek.Friday) return false;

            var workDaysPerWeek = (float)config.TeacherWorkdaysPerGradingPeriod / (float)config.GradingPeriodLengthInWeeks;
            return _rng.NextDouble() / 2 <= workDaysPerWeek;
        }

        private static SchoolCalendarDate CreateCalendarDate(DateTime date, CalendarEventDescriptor calendarEventDescriptor)
        {
            return new SchoolCalendarDate
            {
                Descriptor = calendarEventDescriptor,
                Date = date
            };
        }
    }
}
