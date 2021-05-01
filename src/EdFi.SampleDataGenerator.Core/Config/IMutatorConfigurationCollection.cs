using System.Collections.Generic;
using System.Linq;
using FluentValidation;
using FluentValidation.Validators;

namespace EdFi.SampleDataGenerator.Core.Config
{
    public interface IMutatorConfigurationCollection
    {
       IMutatorConfiguration[] Mutators { get; }
    }

    public interface IMutatorConfiguration
    {
        double Probability { get; }
        string Name { get; }
    }

    public class MutatorConfigurationValidator : AbstractValidator<IMutatorConfigurationCollection>
    {
        public MutatorConfigurationValidator()
        {
            RuleForEach(x => x.Mutators).SetValidator(x => new MutatorValidator());
            RuleFor(x => x.Mutators).Must(HaveUniqueNames).WithMessage(
                "Mutator Names must be unique - two different mutators named {MutatorName} have been defined.");
        }

        private bool HaveUniqueNames(IMutatorConfigurationCollection mutatorConfig, IEnumerable<IMutatorConfiguration> mutators, PropertyValidatorContext context)
        {
            var mutatorNames = mutators.GroupBy(p => p.Name);
            var repeatedName = mutatorNames.FirstOrDefault(g => g.Count() > 1);
            if (repeatedName != null)
            {
                context.MessageFormatter.AppendArgument("MutatorName", repeatedName.Key);
                return false;
            }
            return true;
        }
    }

    public class MutatorValidator : AbstractValidator<IMutatorConfiguration>
    {
        public MutatorValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Mutator Name must be defined and non-empty");
            RuleFor(x => x.Probability).InclusiveBetween(0,1).WithMessage("Mutator {0} has an invalid value; Probability must be greater than zero and less than 1.", m => m.Name);
        }
    }
}
