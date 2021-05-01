using FluentValidation;

namespace EdFi.SampleDataGenerator.Core.Config
{
    public interface IDisciplineProfile
    {
        int TotalExpectedDisciplineEvents { get; }
        int TotalExpectedSeriousDisciplineEvents { get; }
    }

    public class DisciplineProfileValidator : AbstractValidator<IDisciplineProfile>
    {
        public DisciplineProfileValidator()
        {
            RuleFor(x => x.TotalExpectedDisciplineEvents)
                .GreaterThan(0);

            RuleFor(x => x.TotalExpectedSeriousDisciplineEvents)
                .GreaterThan(0);

            RuleFor(x => x.TotalExpectedSeriousDisciplineEvents)
                .LessThanOrEqualTo(x => x.TotalExpectedDisciplineEvents);
        }
    }
}