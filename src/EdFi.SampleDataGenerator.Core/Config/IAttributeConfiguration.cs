using System.Linq;
using EdFi.SampleDataGenerator.Core.Helpers;
using FluentValidation;

namespace EdFi.SampleDataGenerator.Core.Config
{
    public interface IAttributeConfiguration
    {
        string Name { get; }
        IAttributeGeneratorConfigurationOption[] AttributeGeneratorConfigurationOptions { get; }
    }

    public interface IAttributeGeneratorConfigurationOption
    {
        string Value { get; }
        double Frequency { get; }
    }

    public class AttributeConfigurationValidator : AbstractValidator<IAttributeConfiguration>
    {
        public AttributeConfigurationValidator(string containerDescription, bool requireFullOptionDistribution)
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage($"AttributeConfiguration Name property cannot be empty ({containerDescription})");

            RuleFor(x => x.AttributeGeneratorConfigurationOptions)
                .NotNull()
                .SetCollectionValidator(config => new AttributeGeneratorConfigurationOptionValidator(containerDescription, config.Name));

            if (requireFullOptionDistribution)
            {
                RuleFor(x => x.AttributeGeneratorConfigurationOptions)
                    .Must(x => x.Sum(o => o.Frequency).IsEqualWithinTolerance(1.0))
                    .WithMessage("Options for '{0}' must have Frequency values totaling 1.0 ({1})", x => x.Name, x => containerDescription);
            }
        }
    }

    public class AttributeGeneratorConfigurationOptionValidator : AbstractValidator<IAttributeGeneratorConfigurationOption>
    {
        public AttributeGeneratorConfigurationOptionValidator(string containerDescription, string attributeName)
        {
            RuleFor(x => x.Value)
                .NotEmpty()
                .WithMessage($"Value cannot be empty for Attribute '{attributeName}' ({containerDescription})");

            RuleFor(x => x.Frequency)
                .Must(f => f > 0.0 && f <= 1.0)
                .WithMessage("Frequency for Value '{0}' must be greater than 0.0 and less than or equal to 1.0 for Attribute '{1}' ({2})", x => x.Value, x => attributeName, x => containerDescription);
        }
    }
}
