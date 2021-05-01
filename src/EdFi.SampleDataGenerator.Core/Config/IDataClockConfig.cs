using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using EdFi.SampleDataGenerator.Core.Date;
using FluentValidation;
using FluentValidation.Validators;

namespace EdFi.SampleDataGenerator.Core.Config
{
    public interface IDataClockConfig
    {
        DateTime StartDate { get; }
        DateTime EndDate { get; }

        IEnumerable<IDataPeriod> DataPeriods { get; }
    }

    public class DataClockConfigValidator : AbstractValidator<IDataClockConfig>
    {
        public DataClockConfigValidator()
        {
            RuleFor(x => x.DataPeriods)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .Must(x => x != null && x.Any()).WithMessage("At least one DataPeriod must be defined")
                .Must(HaveUniqueNames).WithMessage("DataPeriod Names must be unique - two different data periods named {DataPeriodName} have been defined.")
                .Must(HaveFileSystemSafeNames).WithMessage("DataPeriod Names must be safe for use in filenames. Invalid characters: {InvalidCharacters}")
                .Must(BeNonOverlapping).WithMessage("DataPeriods must be non-overlapping - '{FirstDataPeriodName}' and '{SecondDataPeriodName}' have overlapping date ranges")
                .Must(BeContiguous).WithMessage("DataPeriods must be a contiguous block of time - '{FirstDataPeriodName}' and '{SecondDataPeriodName}' have a gap in date ranges")
                .Must(AlignWithStartAndEndDates).WithMessage("DataPeriods must be a contiguous block of time starting '{GlobalStartDate}' and  ending '{GlobalEndDate}' (the configured global DataClock date range)");

            RuleForEach(x => x.DataPeriods)
                .SetValidator(new DataPeriodValidator());

            RuleFor(x => x.EndDate)
                .Must((config, ed) => ed > config.StartDate)
                .WithMessage("DataClock EndDate must be greater than StartDate");
        }

        private bool BeContiguous(IDataClockConfig dataClockConfig, IEnumerable<IDataPeriod> dataPeriods, PropertyValidatorContext context)
        {
            bool first = true;
            IDataPeriod previousDataPeriod = null;
            foreach (var dataPeriod in dataPeriods.OrderBy(p => p.StartDate))
            {
                if (!first)
                {
                    if (previousDataPeriod.EndDate.AddDays(1) != dataPeriod.StartDate)
                    {
                        context.MessageFormatter.AppendArgument("FirstDataPeriodName", previousDataPeriod.Name);
                        context.MessageFormatter.AppendArgument("SecondDataPeriodName", dataPeriod.Name);
                        return false;
                    }
                }

                first = false;
                previousDataPeriod = dataPeriod;
            }

            return true;
        }

        private bool HaveUniqueNames(IDataClockConfig dataClockConfig, IEnumerable<IDataPeriod> dataPeriods, PropertyValidatorContext context)
        {
            var dataPeriodNames = dataPeriods.GroupBy(p => p.Name);
            var repeatedName = dataPeriodNames.FirstOrDefault(g => g.Count() > 1);
            if (repeatedName != null)
            {
                context.MessageFormatter.AppendArgument("DataPeriodName", repeatedName.Key);
                return false;
            }

            return true;
        }

        private bool HaveFileSystemSafeNames(IDataClockConfig dataClockConfig, IEnumerable<IDataPeriod> dataPeriods, PropertyValidatorContext context)
        {
            var invalidCharacters = dataPeriods.SelectMany(x => x.Name).FileSystemUnsafeCharacters();

            if (invalidCharacters.Any())
            {
                context.MessageFormatter.AppendArgument("InvalidCharacters", new string(invalidCharacters));
                return false;
            }

            return true;
        }

        private bool BeNonOverlapping(IDataClockConfig dataClockConfig, IEnumerable<IDataPeriod> dataPeriods, PropertyValidatorContext context)
        {
            IDataPeriod lastDataPeriod = null;
            foreach (var dataPeriod in dataPeriods.OrderBy(p => p.StartDate))
            {
                if (lastDataPeriod == null)
                {
                    lastDataPeriod = dataPeriod;
                    continue;
                }

                if (dataPeriod.AsDateRange().Overlaps(lastDataPeriod.AsDateRange()))
                {
                    context.MessageFormatter.AppendArgument("FirstDataPeriodName", lastDataPeriod.Name);
                    context.MessageFormatter.AppendArgument("SecondDataPeriodName", dataPeriod.Name);
                    return false;
                }
            }

            return true;
        }

        private bool AlignWithStartAndEndDates(IDataClockConfig dataClockConfig, IEnumerable<IDataPeriod> dataPeriods, PropertyValidatorContext context)
        {
            context.MessageFormatter.AppendArgument("GlobalStartDate", dataClockConfig.StartDate.ToShortDateString());
            context.MessageFormatter.AppendArgument("GlobalEndDate", dataClockConfig.EndDate.ToShortDateString());

            var sortedDataPeriods = dataPeriods.OrderBy(dp => dp.StartDate);
            if (sortedDataPeriods.First().StartDate != dataClockConfig.StartDate)
                return false;

            if (sortedDataPeriods.Last().EndDate != dataClockConfig.EndDate)
                return false;

            return true;
        }
    }

    public static class DataClockConfigExtensions
    {
        public static DateRange AsDateRange(this IDataClockConfig dataClockConfig)
        {
            return dataClockConfig == null 
                ? null 
                : new DateRange(dataClockConfig.StartDate, dataClockConfig.EndDate);
        }
    }
}