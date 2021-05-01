using System.Text.RegularExpressions;
using FluentValidation;

namespace EdFi.SampleDataGenerator.Core.Config
{
    public interface IAssessmentParticipationConfiguration
    {
        string AssessmentTitle { get; set; }
        bool RegexMatch { get; set; }
        Regex AssessmentTitleMatchExpression { get; }
        IAssessmentParticipationRate[] ParticipationRates { get; }
    }

    public interface IAssessmentParticipationRate
    {
        double LowerPerformancePercentile { get; set; }
        double UpperPerformancePercentile { get; set; }
        double Probability { get; set; }
    }

    public class AssessmentParticipationConfigurationValidator : AbstractValidator<IAssessmentParticipationConfiguration>
    {
        public AssessmentParticipationConfigurationValidator(string schoolName, string gradeName)
        {
            RuleFor(a => a.AssessmentTitle).NotNull().NotEmpty().WithMessage("A assessment for {0}, {1} has an empty title; Assessment Title must be defined and non-empty.", schoolName, gradeName);
            RuleFor(a => a.ParticipationRates).NotNull().WithMessage("The configuration must include at least one assessment for School {0}, {1}.", schoolName, gradeName);
            RuleForEach(a => a.ParticipationRates).SetValidator( x => new AssessmentParticipationRateValidator(schoolName, gradeName, x.AssessmentTitle));
        }
         
    }

    public class AssessmentParticipationRateValidator : AbstractValidator<IAssessmentParticipationRate>
    {
        public AssessmentParticipationRateValidator(string schoolName, string gradeName, string assessmentTitle)
        {
            RuleFor(a => a.LowerPerformancePercentile).InclusiveBetween(0, 1).WithMessage("The assessment {0} for {1}, {2} has an invalid value; LowerPerformancePercentile must be between 0 and 1.", assessmentTitle, schoolName, gradeName);
            RuleFor(a => a.UpperPerformancePercentile).InclusiveBetween(0, 1).WithMessage("The assessment {0} for {1}, {2} has an invalid value; UpperPerformancePercentile must be between 0 and 1.", assessmentTitle, schoolName, gradeName);
            RuleFor(a => a.Probability).InclusiveBetween(0, 1).WithMessage("The assessment {0} for {1}, {2} has an invalid value; Probability must be between 0 and 1.", assessmentTitle, schoolName, gradeName);
            RuleFor(a => a.LowerPerformancePercentile).LessThan(b => b.UpperPerformancePercentile).WithMessage("The assessment {0} for {1}, {2} has an invalid value; LowerPerformancePercentile must be less than UpperPerformancePercentile.", assessmentTitle, schoolName, gradeName);
        }
    }

}
