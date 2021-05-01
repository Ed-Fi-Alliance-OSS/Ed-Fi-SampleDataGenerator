using EdFi.SampleDataGenerator.Core.Config;

namespace EdFi.SampleDataGenerator.Core.UnitTests.Config
{
    public class TestDisciplineProfile : IDisciplineProfile
    {
        public int TotalExpectedDisciplineEvents { get; set; }
        public int TotalExpectedSeriousDisciplineEvents { get; set; }

        public static TestDisciplineProfile Default => new TestDisciplineProfile
        {
            TotalExpectedDisciplineEvents = 1,
            TotalExpectedSeriousDisciplineEvents = 1
        };
    }
}