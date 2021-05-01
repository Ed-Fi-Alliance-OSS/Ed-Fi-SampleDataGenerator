using System;
using System.Collections.Generic;
using System.Linq;
using FluentValidation;
using static EdFi.SampleDataGenerator.Core.Config.ValidationHelpers;

namespace EdFi.SampleDataGenerator.Core.Config
{
    public interface IStudentProfile
    {
        string Name { get; }
        IAttributeConfiguration RaceConfiguration { get; }
        IAttributeConfiguration SexConfiguration { get; }
        IAttributeConfiguration EconomicDisadvantageConfiguration { get; }
        IAttributeConfiguration HomelessStatusConfiguration { get; }
        IImmigrantPopulationProfile ImmigrantPopulationProfile { get; }
    }

    public static class StudentProfileExtensions
    {
        public static IStudentProfile GetProfileByName(this IEnumerable<IStudentProfile> studentProfiles, string profileName)
        {
            return studentProfiles.FirstOrDefault(sp => sp.Name.Equals(profileName, StringComparison.OrdinalIgnoreCase));
        }

        public static IStudentProfile GetProfileByName(this IStudentProfile[] studentProfiles, string profileName)
        {
            return studentProfiles.FirstOrDefault(sp => sp.Name.Equals(profileName, StringComparison.OrdinalIgnoreCase));
        }
    }

    public class StudentProfileValidator : AbstractValidator<IStudentProfile>
    {
        private readonly ISampleDataGeneratorConfig _globalConfig;

        public StudentProfileValidator(ISampleDataGeneratorConfig globalConfig)
        {
            _globalConfig = globalConfig;
            RuleFor(x => x.Name).NotEmpty().WithMessage("StudentProfile Name may not be empty");

            RuleFor(x => x.RaceConfiguration).NotNull().WithMessage("Student Profile '{0}' must define a RaceConfiguration", p => p.Name);
            RuleFor(x => x.RaceConfiguration)
                .SetValidator(profile => new AttributeConfigurationValidator($"Student Profile '{profile.Name}'", true));

            RuleFor(x => x.SexConfiguration).NotNull().WithMessage("Student Profile '{0}' must define a SexConfiguration", p => p.Name);
            RuleFor(x => x.SexConfiguration)
                .SetValidator(profile => new AttributeConfigurationValidator($"Student Profile '{profile.Name}'", true));

            RuleFor(x => x.EconomicDisadvantageConfiguration)
                .SetValidator(profile => new AttributeConfigurationValidator($"Student Profile '{profile.Name}'", false));
            RuleFor(x => x.EconomicDisadvantageConfiguration)
                .Must(profile => ContainValidRacesOnly(profile, _globalConfig))
                .WithMessage("StudentProfile '{0}' EconomicDisadvantageConfiguration contains invalid Race Options");

            RuleFor(x => x.HomelessStatusConfiguration)
                .SetValidator(profile => new AttributeConfigurationValidator($"Student Profile '{profile.Name}'", false));
            RuleFor(x => x.HomelessStatusConfiguration)
                .Must(profile => ContainValidRacesOnly(profile, _globalConfig))
                .WithMessage("StudentProfile '{0}' HomelessStatusConfiguration contains invalid Race Options");

            RuleFor(x => x.ImmigrantPopulationProfile)
                .SetValidator(x => new ImmigrantPopulationProfileValidator(_globalConfig, x));
        }
    }
}