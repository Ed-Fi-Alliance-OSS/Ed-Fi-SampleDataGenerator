using System.Linq;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common;
using FluentValidation;

namespace EdFi.SampleDataGenerator.Core.Config
{
    public interface ISampleDataGeneratorConfig
    {
        int? BatchSize { get; }
        string DataFilePath { get; }
        string OutputPath { get; }
        string SeedFilePath { get; }
        OutputMode OutputMode { get; }
        bool CreatePerformanceFile { get; }

        ITimeConfig TimeConfig { get; }

        IDistrictProfile[] DistrictProfiles { get; }
        IStudentProfile[] StudentProfiles { get; }

        IDataFileConfig DataFileConfig { get; }

        IEthnicityMapping[] EthnicityMappings { get; }
        IGenderMapping[] GenderMappings { get; }

        IStudentPopulationProfile[] StudentPopulationProfiles { get; }

        IGraduationPlanTemplate[] GraduationPlanTemplates { get; }

        IMutatorConfigurationCollection MutatorConfig { get; }

        IStudentGradeRange[] StudentGradeRanges { get; }
    }

    public class SampleDataGeneratorConfigValidator : AbstractValidator<ISampleDataGeneratorConfig>
    {
        public SampleDataGeneratorConfigValidator()
        {
            RuleFor(x => x.TimeConfig).NotEmpty().WithMessage("Time config must be defined");
            RuleFor(x => x.TimeConfig).SetValidator(x => new TimeConfigValidator());
            RuleFor(x => x.DistrictProfiles).NotEmpty().WithMessage("At least one DistrictProfile must be defined");
            RuleFor(x => x.DistrictProfiles).SetCollectionValidator(x => new DistrictProfileValidator(x));
            RuleFor(x => x.StudentProfiles).NotEmpty().WithMessage("At least one StudentProfile must be defined");
            RuleFor(x => x.StudentProfiles).SetCollectionValidator(x => new StudentProfileValidator(x));
            RuleForEach(x => x.StudentProfiles).Must(UseValidRaceOptions).WithMessage("Student Profile '{0}' contains an invalid Race configuration option", (config, profile) => profile.Name);
            RuleForEach(x => x.StudentProfiles).Must(UseValidGenderOptions).WithMessage("Student Profile '{0}' contains an invalid Sex configuration option", (config, profile) => profile.Name);
            RuleFor(x => x.GraduationPlanTemplates).SetCollectionValidator(x => new GraduationPlanTemplateValidator());

            RuleFor(x => x.EthnicityMappings).NotEmpty().WithMessage("EthnicityMappings must be defined");
            RuleFor(x => x.GenderMappings).NotEmpty().WithMessage("GenderMappings must be defined");
            RuleFor(x => x.MutatorConfig).SetValidator(x => new MutatorConfigurationValidator());
        }

        private bool UseValidRaceOptions(ISampleDataGeneratorConfig config, IStudentProfile profile)
        {
            var raceConfig = profile.RaceConfiguration;
            return raceConfig != null 
                && raceConfig.AttributeGeneratorConfigurationOptions
                    .All(o => config.IsValidRaceOption(o.Value));
        }

        private bool UseValidGenderOptions(ISampleDataGeneratorConfig config, IStudentProfile profile)
        {
            var genderConfig = profile.SexConfiguration;
            return genderConfig != null
                   && genderConfig.AttributeGeneratorConfigurationOptions
                       .All(o => config.IsValidGenderOption(o.Value));
        }
    }

    public static class SampleDataGeneratorConfigHelpers
    {
        public static double GetMutationProbability(this ISampleDataGeneratorConfig config, string mutatorName)
        {
            var mutator = config.MutatorConfig.Mutators.FirstOrDefault(a => a.Name == mutatorName);
            return mutator?.Probability ?? 0;
        }
    }
}
