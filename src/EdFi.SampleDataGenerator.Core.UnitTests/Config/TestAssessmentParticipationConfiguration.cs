using System.Text.RegularExpressions;
using EdFi.SampleDataGenerator.Core.Config;

namespace EdFi.SampleDataGenerator.Core.UnitTests.Config
{
    public class TestAssessmentParticipationConfiguration : IAssessmentParticipationConfiguration
    {
        public string AssessmentTitle { get; set; }
        public IAssessmentParticipationRate[] ParticipationRates { get; set; }
        public bool RegexMatch { get; set; }
        public Regex AssessmentTitleMatchExpression { get; set; }

        public static TestAssessmentParticipationConfiguration Default => new TestAssessmentParticipationConfiguration
        {
            ParticipationRates = new IAssessmentParticipationRate[0]
        };

    }

    public class TestAssessmentParticipationRate : IAssessmentParticipationRate
    {
        public double LowerPerformancePercentile { get; set; }
        public double UpperPerformancePercentile { get; set; }
        public double Probability { get; set; }
    }
}
