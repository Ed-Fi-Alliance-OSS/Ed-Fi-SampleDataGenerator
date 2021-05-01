using FluentValidation;

namespace EdFi.SampleDataGenerator.Core.Config
{
    public interface IImmigrantPopulationProfile
    {
        ICountryOfOrigin[] CountriesOfOrigin { get; }
    }

    public interface ICountryOfOrigin
    {
        string Name { get; }
        string Race { get; }
        double Frequency { get; }
    }

    public class ImmigrantPopulationProfileValidator : AbstractValidator<IImmigrantPopulationProfile>
    {
        public ImmigrantPopulationProfileValidator(ISampleDataGeneratorConfig sampleDataGeneratorConfig, IStudentProfile studentProfile)
        {
            RuleFor(x => x.CountriesOfOrigin)
                .NotEmpty()
                .WithMessage("At least one CountryOfOrigin must be defined in ImmigrationProfile for StudentProfile '{0}'",x => studentProfile.Name);

            RuleForEach(x => x.CountriesOfOrigin).SetValidator(x => new CountryOfOriginValidator(sampleDataGeneratorConfig, studentProfile));
        }
    }

    public class CountryOfOriginValidator : AbstractValidator<ICountryOfOrigin>
    {
        public CountryOfOriginValidator(ISampleDataGeneratorConfig sampleDataGeneratorConfig, IStudentProfile studentProfile)
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Name cannot be empty for CountryOfOrigin in Student Profile '{0}'", x => studentProfile.Name);

            RuleFor(x => x.Race)
                .NotEmpty()
                .WithMessage("Race cannot be empty for CountryOfOrigin '{0}' in Student Profile '{1}'", x => x.Name, x => studentProfile.Name);

            RuleFor(x => x.Race)
                .Must(sampleDataGeneratorConfig.IsValidRaceOption)
                .WithMessage("'{0}' is not a valid RaceType for CountryOfOrigin '{1}' in Student Profile '{2}'", x => x.Race, x => x.Name, x => studentProfile.Name);

            RuleFor(x => x.Frequency)
                .Must(f => f > 0.0 && f <= 1.0)
                .WithMessage("Frequency for CountryOfOrigin '{0}' / Race '{1}' in Student Profile '{2}' must be greater than 0.0 and less than or equal to 1.0", x => x.Name, x => x.Race, x => studentProfile.Name);
        }
    }
}
