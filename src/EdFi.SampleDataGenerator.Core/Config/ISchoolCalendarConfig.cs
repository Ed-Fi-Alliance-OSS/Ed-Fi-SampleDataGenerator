using System;
using EdFi.SampleDataGenerator.Core.Entities;
using EdFi.SampleDataGenerator.Core.Helpers;
using FluentValidation;

namespace EdFi.SampleDataGenerator.Core.Config
{
    public interface ISchoolCalendarConfig
    {
        DateTime StartDate { get; }
        DateTime EndDate { get; }
    }

    public static class SchoolCalendarConfigExtensions
    {
        public static string ToSchoolYear(this ISchoolCalendarConfig config)
        {
            return $"{config.StartDate.Year}-{config.EndDate.Year}";
        }

        public static SchoolYearType SchoolYear(this ISchoolCalendarConfig config)
        {
            return EnumHelpers.Parse<SchoolYearType>(config.ToSchoolYear());
        }

        public static bool DefinesValidSchoolYear(this ISchoolCalendarConfig config)
        {
            return EnumHelpers.IsParseableToEnum<SchoolYearType>(config.ToSchoolYear());
        }
    }

    public class SchoolCalendarConfigValidator : AbstractValidator<ISchoolCalendarConfig>
    {
        public SchoolCalendarConfigValidator()
        {
            RuleFor(x => x.StartDate).NotEmpty().WithMessage("SchoolCalendar.StartDate is required");
            RuleFor(x => x.EndDate).NotEmpty().WithMessage("SchoolCalendar.EndDate is required");
            RuleFor(x => x.EndDate)
                .GreaterThan(x => x.StartDate)
                .WithMessage("SchoolCalendar.EndDate must be greater than SchoolCalendar.StartDate");

            RuleFor(x => x.EndDate)
                .Must(DefineSchoolYear)
                .WithMessage("SchoolCalendar.StartDate and SchoolCalendar.EndDate must define a valid school year -- i.e. a pair of consecutive years (e.g. 2017-2018)");
        }

        private static bool DefineSchoolYear(ISchoolCalendarConfig config, DateTime date)
        {
            return config.DefinesValidSchoolYear();
        }
    }
}