using EdFi.SampleDataGenerator.Core.Config;
using NUnit.Framework;

namespace EdFi.SampleDataGenerator.Core.UnitTests.Config
{
    public class AssessmentParticipationRateValidatorTester : ValidatorTestBase<AssessmentParticipationRateValidator, IAssessmentParticipationRate>
    {
        public AssessmentParticipationRateValidatorTester() : base(new AssessmentParticipationRateValidator("Test School", "Test grade", "Test assessment"))
        {
        }

        [Test]
        public void ShouldFailWhenLowerPerformancePercentileLessThanZero()
        {
            var configuration = new TestAssessmentParticipationRate() { LowerPerformancePercentile = -0.1, UpperPerformancePercentile = 1, Probability = 1 };
            Validate(configuration, false);
        }

        [Test]
        public void ShouldPassWhenLowerPerformancePercentileEqualsZero()
        {
            var configuration = new TestAssessmentParticipationRate() { LowerPerformancePercentile = 0, UpperPerformancePercentile = 1, Probability = 1 };
            Validate(configuration, true);
        }

        [Test]
        public void ShouldFailWhenLowerPerformancePercentileEqualsOne()
        {
            var configuration = new TestAssessmentParticipationRate() { LowerPerformancePercentile = 1, UpperPerformancePercentile = 1, Probability = 1 };
            Validate(configuration, false);
        }

        [Test]
        public void ShouldFailWhenLowerPerformancePercentileGreaterThanOne()
        {
            var configuration = new TestAssessmentParticipationRate() { LowerPerformancePercentile = 1.1, UpperPerformancePercentile = 1.2, Probability = 1 };
            Validate(configuration, false);
        }

        [Test]
        public void ShouldFailWhenUpperPerformancePercentileLessThanZero()
        {
            var configuration = new TestAssessmentParticipationRate() { LowerPerformancePercentile = 0, UpperPerformancePercentile = -0.1, Probability = 1 };
            Validate(configuration, false);
        }

        [Test]
        public void ShouldFailWhenUpperPerformancePercentileEqualsZero()
        {
            var configuration = new TestAssessmentParticipationRate() { LowerPerformancePercentile = 0, UpperPerformancePercentile = 0, Probability = 1 };
            Validate(configuration, false);
        }

        [Test]
        public void ShouldPassWhenUpperPerformancePercentileEqualsOne()
        {
            var configuration = new TestAssessmentParticipationRate() { LowerPerformancePercentile = 0, UpperPerformancePercentile = 1, Probability = 1 };
            Validate(configuration, true);
        }

        [Test]
        public void ShouldFailWhenUpperPerformancePercentileGreaterThanOne()
        {
            var configuration = new TestAssessmentParticipationRate() { LowerPerformancePercentile = 0, UpperPerformancePercentile = 1.1, Probability = 1 };
            Validate(configuration, false);
        }

        [Test]
        public void ShouldFailWhenProbabilityLessThanZero()
        {
            var configuration = new TestAssessmentParticipationRate() { LowerPerformancePercentile = 0, UpperPerformancePercentile = 1, Probability = -0.1 };
            Validate(configuration, false);
        }

        [Test]
        public void ShouldPassWhenProbabilityEqualsZero()
        {
            var configuration = new TestAssessmentParticipationRate() { LowerPerformancePercentile = 0, UpperPerformancePercentile = 1, Probability = 0 };
            Validate(configuration, true);
        }

        [Test]
        public void ShouldPassWhenProbabilityEqualsOne()
        {
            var configuration = new TestAssessmentParticipationRate() { LowerPerformancePercentile = 0, UpperPerformancePercentile = 1, Probability = 1 };
            Validate(configuration, true);
        }

        [Test]
        public void ShouldFailWhenProbabilityGreaterThanOne()
        {
            var configuration = new TestAssessmentParticipationRate() { LowerPerformancePercentile = 0, UpperPerformancePercentile = 1, Probability = 1.1 };
            Validate(configuration, false);
        }

        [Test]
        public void ShouldFailWhenLowerPerformancePercentileGreaterThanUpperPerformancePercentile()
        {
            var configuration = new TestAssessmentParticipationRate() { LowerPerformancePercentile = 0.9, UpperPerformancePercentile = 0.5, Probability = 1 };
            Validate(configuration, false);
        }
    }
}
