using FluentValidation;

namespace EdFi.SampleDataGenerator.Core.Config
{
    public interface ITimeConfig
    {
        ISchoolCalendarConfig SchoolCalendarConfig { get; }
        IDataClockConfig DataClockConfig { get; }
    }

    public class TimeConfigValidator : AbstractValidator<ITimeConfig>
    {
        public TimeConfigValidator()
        {
            RuleFor(x => x.SchoolCalendarConfig).NotEmpty().WithMessage("SchoolCalendar configuration is required");
            RuleFor(x => x.SchoolCalendarConfig).SetValidator(new SchoolCalendarConfigValidator());
            
            RuleFor(x => x.DataClockConfig).NotEmpty().WithMessage("DataClock configuration is required");
            RuleFor(x => x.DataClockConfig).SetValidator(new DataClockConfigValidator());
            RuleFor(x => x.DataClockConfig)
                .Must(DataClockStartsWithSchoolCalendarYear)
                .WithMessage("DataClock must start on the same day as the configured school year")
                .Must(DataClockEndsWithinSchoolCalendarYear)
                .WithMessage("DataClock must end within the configured school year");

        }

        private bool DataClockStartsWithSchoolCalendarYear(ITimeConfig timeConfig, IDataClockConfig dataClockConfig)
        {
            return dataClockConfig?.StartDate == timeConfig.SchoolCalendarConfig?.StartDate &&
                dataClockConfig?.EndDate <= timeConfig.SchoolCalendarConfig?.EndDate;
        }

        private bool DataClockEndsWithinSchoolCalendarYear(ITimeConfig timeConfig, IDataClockConfig dataClockConfig)
        {
            return dataClockConfig?.StartDate == timeConfig.SchoolCalendarConfig?.StartDate &&
                dataClockConfig?.EndDate <= timeConfig.SchoolCalendarConfig?.EndDate;
        }
    }
}
