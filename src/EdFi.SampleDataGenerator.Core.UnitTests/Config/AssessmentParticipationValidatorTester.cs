using EdFi.SampleDataGenerator.Core.Config;
using NUnit.Framework;

namespace EdFi.SampleDataGenerator.Core.UnitTests.Config
{
    public class AssessmentParticipationValidatorTester : ValidatorTestBase<AssessmentParticipationConfigurationValidator, IAssessmentParticipationConfiguration>
    {
        public AssessmentParticipationValidatorTester() : base(new AssessmentParticipationConfigurationValidator("Test school", "Test grade"))
        {
        }

        [Test]
        public void ShouldFailWithEmptyAssessmentTitle()
        {
            var configuration = new TestAssessmentParticipationConfiguration { AssessmentTitle = string.Empty};
            Validate(configuration, false);
        }

        [Test]
        public void ShouldFailWhenNullParticipationRates()
        {
            var configuration = new TestAssessmentParticipationConfiguration { AssessmentTitle = "Test", ParticipationRates = null};
            Validate(configuration, false);
        }
    }
}
