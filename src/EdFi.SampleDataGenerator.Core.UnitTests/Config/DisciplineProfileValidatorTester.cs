using EdFi.SampleDataGenerator.Core.Config;
using NUnit.Framework;

namespace EdFi.SampleDataGenerator.Core.UnitTests.Config
{
    [TestFixture]
    public class DisciplineProfileValidatorTester : ValidatorTestBase<DisciplineProfileValidator, IDisciplineProfile>
    {
        public DisciplineProfileValidatorTester() : base(new DisciplineProfileValidator())
        {
        }

        [Test]
        public void ShouldFailTotalLessThanOne()
        {
            var profile = new TestDisciplineProfile
            {
                TotalExpectedDisciplineEvents = 0,
                TotalExpectedSeriousDisciplineEvents = 1
            };

            Validate(profile, false);
        }

        [Test]
        public void ShouldFailSeriousLessThanOne()
        {
            var profile = new TestDisciplineProfile
            {
                TotalExpectedDisciplineEvents = 1,
                TotalExpectedSeriousDisciplineEvents = 0
            };

            Validate(profile, false);
        }

        [Test]
        public void ShouldFailSeriousGreaterThanTotal()
        {
            var profile = new TestDisciplineProfile
            {
                TotalExpectedDisciplineEvents = 1,
                TotalExpectedSeriousDisciplineEvents = 2
            };

            Validate(profile, false);
        }

        [Test]
        public void ShouldPassValidDisciplineProfile()
        {
            var profile = new TestDisciplineProfile
            {
                TotalExpectedDisciplineEvents = 1,
                TotalExpectedSeriousDisciplineEvents = 1
            };

            Validate(profile, true);
        }
    }
}
