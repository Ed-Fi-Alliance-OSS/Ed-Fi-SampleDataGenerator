using FluentValidation;
using static EdFi.SampleDataGenerator.Core.Config.ValidationHelpers;

namespace EdFi.SampleDataGenerator.Core.Config
{
    public interface IStaffProfile
    {
        IAttributeConfiguration StaffRaceConfiguration { get; }
        IAttributeConfiguration StaffSexConfiguration { get; }
    }

    public class StaffProfileValidator : AbstractValidator<IStaffProfile>
    {
        public StaffProfileValidator(string schoolName, ISampleDataGeneratorConfig globalConfig)
        {
            RuleFor(x => x.StaffRaceConfiguration)
                .NotNull().WithMessage($"{schoolName} Staff Profile must define a StaffRaceConfiguration")
                .Must(profile => ContainValidRacesOnly(profile, globalConfig)).WithMessage($"{schoolName} Staff Profile contains invalid race options")
                .SetValidator(profile => new AttributeConfigurationValidator($"{schoolName} Staff Profile", true));

            RuleFor(x => x.StaffSexConfiguration)
                .NotNull().WithMessage($"{schoolName} Staff Profile must define a StaffSexConfiguration")
                .SetValidator(profile => new AttributeConfigurationValidator($"{schoolName} Staff Profile", true));
        }
    }
}
