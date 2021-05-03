using System;
using System.Collections.Generic;
using System.Linq;
using EdFi.SampleDataGenerator.Core.Entities;
using EdFi.SampleDataGenerator.Core.Helpers;

namespace EdFi.CalendarGenerator.Console
{
    public static class CalendarTemplateMappingService
    {
        private const string CalendarCode = "Calendar Code";
        private const SchoolYearType SchoolYear = SchoolYearType.Item20162017;

        public static IEnumerable<GradingPeriod> MapToGradingPeriods(CalendarGeneratorConfig config, SchoolYearTemplate template)
        {
            return from schoolId in config.SchoolIds
                   from gradingPeriod in template.Terms.SelectMany(t => t.GradingPeriods)
                   select Map(config, schoolId, gradingPeriod);
        }

        public static IEnumerable<Session> MapToSessions(CalendarGeneratorConfig config, SchoolYearTemplate template)
        {
            return from schoolId in config.SchoolIds
                from term in template.Terms
                select Map(config, schoolId, template, term);
        }

        public static IEnumerable<Calendar> MapToCalendars(CalendarGeneratorConfig config, SchoolYearTemplate template)
        {
            return from schoolId in config.SchoolIds
                select Map(schoolId);
        }

        public static IEnumerable<CalendarDate> MapToCalendarDates(CalendarGeneratorConfig config, SchoolYearTemplate template)
        {
            return from schoolId in config.SchoolIds
                from calendarDate in GetSchoolCalendarDates(template)
                select Map(schoolId, calendarDate);
        }

        private static IEnumerable<SchoolCalendarDate> GetSchoolCalendarDates(SchoolYearTemplate template)
        {
            var totalGradingPeriods = template.Terms.Sum(t => t.GradingPeriods.Count);
            return template.Terms.SelectMany(t =>
                t.GradingPeriods.SelectMany(gp =>
                    gp.GradingPeriodNumber == totalGradingPeriods
                        ? gp.CalendarDates
                        : gp.CalendarDates.Concat<SchoolCalendarDate>(gp.AdditionalHolidays)));
        } 

        private static Calendar Map(string schoolId)
        {
            return new Calendar
            {
                id = $"CAL_{schoolId}_{SchoolYear}",
                CalendarCode = CalendarCode,
                CalendarType = CalendarTypeDescriptor.IEP.GetStructuredCodeValue(),
                SchoolReference = GetSchoolReference(schoolId),
                SchoolYear = SchoolYear
            };
        }

        private static CalendarDate Map(string schoolId, SchoolCalendarDate calendarDate)
        {
            return new CalendarDate
            {
                id = $"CLND_{calendarDate.Date:yyyyMMdd}{calendarDate.Descriptor.CodeValue.Replace(" ", "_")}_{schoolId}",
                CalendarEvent = new[] {calendarDate.Descriptor.GetStructuredCodeValue()},
                Date = calendarDate.Date, 
                CalendarReference  =  new CalendarReferenceType
                {
                    CalendarIdentity =  new CalendarIdentityType
                    {
                        CalendarCode = CalendarCode,
                        SchoolReference = GetSchoolReference(schoolId),
                        SchoolYear = SchoolYear
                    }
                }
            };
        }

        private static Session Map(CalendarGeneratorConfig config, string schoolId, SchoolYearTemplate schoolYearTemplate,  TermTemplate template)
        {
            var term = GetTerm(template.TermNumber);
            var gradingPeriodReferences = template.GradingPeriods
                .Select(gp => Map(config, schoolId, gp))
                .Select(GetGradingPeriodReference)
                .ToArray();

            var result = new Session
            {
                id = $"SESS_{template.StartDate:yyyyMMdd}_{schoolId}",
                SessionName = $"{schoolYearTemplate.StartDate.Year} - {schoolYearTemplate.EndDate.Year} {term.CodeValue}",
                SchoolReference = GetSchoolReference(schoolId),
                Term = term.GetStructuredCodeValue(),
                SchoolYear = GetSchoolYear(schoolYearTemplate),
                BeginDate = template.StartDate,
                EndDate = template.EndDate,
                TotalInstructionalDays = template.TotalInstructionalDays,
                GradingPeriodReference = gradingPeriodReferences
            };

            return result;
        }

        private static SchoolReferenceType GetSchoolReference(string schoolId)
        {
            var schoolIdNum = Int32.Parse(schoolId);

            var schoolReference = new SchoolReferenceType
            {
                SchoolIdentity = new SchoolIdentityType { SchoolId = schoolIdNum },
                SchoolLookup = new SchoolLookupType { SchoolId = schoolIdNum }
            };
            return schoolReference;
        }

        private static TermDescriptor GetTerm(int termNumber)
        {
            if (termNumber == 1)
                return TermDescriptor.FallSemester;

            if (termNumber == 2)
                return TermDescriptor.SpringSemester;

            throw new NotSupportedException($"Cannot infer TermDescriptor for term {termNumber}");
        }

        private static SchoolYearType GetSchoolYear(SchoolYearTemplate template)
        {
            var schoolYearString = $"{template.StartDate.Year}-{template.EndDate.Year}";
            return EnumHelpers.Parse<SchoolYearType>(schoolYearString);
        }

        private static GradingPeriod Map(CalendarGeneratorConfig config, string schoolId, GradingPeriodTemplate template)
        {
            var result = new GradingPeriod
            {
                id = $"GRDP_{template.StartDate:yyyyMMdd}_{schoolId}",
                BeginDate = template.StartDate,
                EndDate = template.EndDate,
                GradingPeriod1 = GetGradingPeriod(config, template.GradingPeriodNumber),
                SchoolReference = GetSchoolReference(schoolId),
                TotalInstructionalDays = template.TotalInstructionalDays,
                PeriodSequence = template.GradingPeriodNumber
            };
            
            return result;
        }

        private static GradingPeriodReferenceType GetGradingPeriodReference(GradingPeriod gradingPeriod)
        {
            return new GradingPeriodReferenceType
            {
                GradingPeriodIdentity = new GradingPeriodIdentityType
                {
                    SchoolReference = gradingPeriod.SchoolReference,
                    GradingPeriod = gradingPeriod.GradingPeriod1,
                    PeriodSequence = gradingPeriod.PeriodSequence,
                    SchoolYear = gradingPeriod.SchoolYear
                }
            };
        }

        private static string GetGradingPeriod(CalendarGeneratorConfig config, int gradingPeriodNumber)
        { 

            if (config.GradingPeriodLengthInWeeks == 6)
            {
                switch (gradingPeriodNumber)
                {
                    case 1:
                        return GradingPeriodDescriptor.FirstSixWeeks.GetStructuredCodeValue(); 
                    case 2:
                        return GradingPeriodDescriptor.SecondSixWeeks.GetStructuredCodeValue(); 
                    case 3:
                        return GradingPeriodDescriptor.ThirdSixWeeks.GetStructuredCodeValue(); 
                    case 4:
                        return GradingPeriodDescriptor.FourthSixWeeks.GetStructuredCodeValue(); 
                    case 5:
                        return GradingPeriodDescriptor.FifthSixWeeks.GetStructuredCodeValue(); 
                    case 6:
                        return GradingPeriodDescriptor.SixthSixWeeks.GetStructuredCodeValue(); 
                }
            }

            if (config.GradingPeriodLengthInWeeks == 9)
            {
                switch (gradingPeriodNumber)
                {
                    case 1:
                        return GradingPeriodDescriptor.FirstNineWeeks.GetStructuredCodeValue(); 
                    case 2:
                        return GradingPeriodDescriptor.SecondNineWeeks.GetStructuredCodeValue(); 
                    case 3:
                        return GradingPeriodDescriptor.ThirdNineWeeks.GetStructuredCodeValue(); 
                    case 4:
                        return GradingPeriodDescriptor.FourthNineWeeks.GetStructuredCodeValue(); 
                }
            }

            throw new Exception("Grading Period number not found.");
        }
    }
}
