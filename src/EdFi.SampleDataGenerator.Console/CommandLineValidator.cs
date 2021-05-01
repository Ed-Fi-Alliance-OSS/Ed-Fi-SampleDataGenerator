using System.IO;
using System.Linq;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common;
using FluentValidation;

namespace EdFi.SampleDataGenerator.Console
{
    public class CommandLineValidator : AbstractValidator<SampleDataGeneratorConsoleConfig>
    {
        public CommandLineValidator()
        {
            RuleFor(x => x.DataFilePath)
                .Must((config, path) => Directory.Exists(path))
                .WithMessage("DataFilePath '{0}' does not exist", config => config.DataFilePath);

            RuleFor(x => x.ConfigXmlPath)
                .Must((config, path) => File.Exists(path))
                .WithMessage("No config file found at '{0}'", config => config.ConfigXmlPath);

            RuleFor(x => x.OutputMode)
                .Must((config, mode) => !string.IsNullOrWhiteSpace(config.SeedFilePath))
                .When(x => x.OutputMode == OutputMode.Seed)
                .WithMessage("When running in Seed mode, a SeedFilePath must be provided");

            RuleFor(x => x.OutputMode)
                .Must((config, mode) => File.Exists(config.SeedFilePath))
                .When(x => x.OutputMode == OutputMode.Standard && !string.IsNullOrWhiteSpace(x.SeedFilePath))
                .WithMessage("No seed file found at '{0}'", config => config.SeedFilePath);

            RuleFor(x => x.AllowOverwrite)
                .Must((config, allowOverwrite) => !Directory.GetFiles(config.OutputPath).Any())
                .When(x => !x.AllowOverwrite && Directory.Exists(x.OutputPath))
                .WithMessage("One or more files exist in {0}, but -AllowOverwrite argument not specified", config => config.OutputPath);

            RuleFor(x => x.AllowOverwrite)
                .Must((config, mode) => !File.Exists(config.SeedFilePath))
                .When(x => !x.AllowOverwrite && x.OutputMode == OutputMode.Seed)
                .WithMessage("Seed file '{0}' exists, but -AllowOverwrite argument not specified", config => config.SeedFilePath);
        }
    }
}
