using FluentValidation;

namespace EdFi.SampleDataGenerator.Core.Config
{
    public interface ISchoolAttendanceProfile
    {
        double AverageAttendanceRate { get; }
        double AverageTardyRate { get; }
    }

    public class SchoolAttendanceProfileValidator : AbstractValidator<ISchoolAttendanceProfile>
    {
        public SchoolAttendanceProfileValidator(string schoolName)
        {
            RuleFor(x => x.AverageAttendanceRate)
                .InclusiveBetween(0.0, 1.0)
                .WithMessage("The AverageAttendanceRate for {0} has an invalid value; AverageAttendanceRate must be between 0 and 1.", schoolName);

            RuleFor(x => x.AverageTardyRate)
                .InclusiveBetween(0.0, 1.0)
                .WithMessage("The AverageTardyRate for {0} has an invalid value; AverageTardyRate must be between 0 and 1.", schoolName);
        }
    }
}