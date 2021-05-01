using EdFi.SampleDataGenerator.Core.Entities;
using EdFi.SampleDataGenerator.Core.Helpers;
using FluentValidation;

namespace EdFi.SampleDataGenerator.Core.Config
{
    public interface IGraduationPlanTemplate
    {
        string Name { get; }
        string GraduationPlanType { get; }
        int TotalCreditsRequired { get; }

        GraduationPlanTypeDescriptor GetGraduationPlanTypeDescriptor();
    }

    public interface IGraduationPlanTemplateReference
    {
        string Name { get; }
    }

    public class GraduationPlanTemplateValidator : AbstractValidator<IGraduationPlanTemplate>
    {
        public GraduationPlanTemplateValidator()
        {
            RuleFor(x => x.Name).NotNull();
            RuleFor(x => x.GraduationPlanType)
                .NotNull()
                .Must(BeAValidToGraduationPlanMapType)
                .WithMessage("'{0}' is not a valid GraduationPlanMapType", x => x.GraduationPlanType);

            RuleFor(x => x.TotalCreditsRequired).GreaterThanOrEqualTo(1);
        }

        private static bool BeAValidToGraduationPlanMapType(string value)
        {
            return !string.IsNullOrEmpty(value) && DescriptorHelpers.IsParseableToDescriptorFromCodeValue<GraduationPlanTypeDescriptor>(value);
        }
    }
}
