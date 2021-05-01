using System;
using EdFi.SampleDataGenerator.Core.Date;
using FluentValidation;

namespace EdFi.SampleDataGenerator.Core.Config
{
    public interface IDataPeriod
    {
        string Name { get; }
        DateTime StartDate { get; }
        DateTime EndDate { get; }
    }

    public class DataPeriodValidator : AbstractValidator<IDataPeriod>
    {
        public DataPeriodValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("DataPeriod Name must be defined");

            RuleFor(x => x.StartDate)
                .NotEmpty();

            RuleFor(x => x.EndDate)
                .Must((config, ed) => ed > config.StartDate)
                .WithMessage("DataPeriod EndDate must be greater than StartDate");
        }
    }

    public static class DataPeriodExtensions
    {
        public static DateRange AsDateRange(this IDataPeriod dataPeriod)
        {
            return dataPeriod == null
                ? null
                : new DateRange(dataPeriod.StartDate, dataPeriod.EndDate);
        }

        public static DateRange Intersect(this IDataPeriod dataPeriod, DateRange dateRange)
        {
            return dataPeriod.AsDateRange()?.Intersect(dateRange);
        }
    }
}