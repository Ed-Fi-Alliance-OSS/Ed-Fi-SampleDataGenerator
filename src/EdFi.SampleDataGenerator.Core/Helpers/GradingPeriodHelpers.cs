using System;
using System.Collections.Generic;
using System.Linq;
using EdFi.SampleDataGenerator.Core.Date;
using EdFi.SampleDataGenerator.Core.Entities;

namespace EdFi.SampleDataGenerator.Core.Helpers
{
    public static class GradingPeriodHelpers
    {
        public static DateRange AsDateRange(this GradingPeriod gradingPeriod)
        {
            return new DateRange(gradingPeriod.BeginDate, gradingPeriod.EndDate);
        }

        public static GradingPeriodReferenceType GetGradingPeriodReference(this GradingPeriod gradingPeriod)
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

        public static IEnumerable<GradingPeriod> ForSchool(this IEnumerable<GradingPeriod> gradingPeriods, int schoolId)
        {
            return gradingPeriods.Where(x => x.SchoolReference.ReferencesSchool(schoolId));
        }
        public static IEnumerable<GradingPeriod> StartedOnOrAfter(this IEnumerable<GradingPeriod> gradingPeriods, DateTime targetDate)
        {
            return gradingPeriods.Where(x => x.BeginDate >= targetDate);
        }

        public static IEnumerable<GradingPeriod> CompletedOnOrBefore(this IEnumerable<GradingPeriod> gradingPeriods, DateTime targetDate)
        {
            return gradingPeriods.Where(x => x.EndDate <= targetDate);
        }

        public static IEnumerable<GradingPeriod> CompletedDuring(this IEnumerable<GradingPeriod> gradingPeriods, DateRange dateRange)
        {
            return gradingPeriods.Where(x => dateRange.Contains(x.EndDate));
        }
    }
}
