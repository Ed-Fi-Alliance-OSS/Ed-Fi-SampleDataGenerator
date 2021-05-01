using EdFi.SampleDataGenerator.Core.Config;

namespace EdFi.SampleDataGenerator.Core.UnitTests.Config
{
    public class TestStudentPopulationProfile : IStudentPopulationProfile
    {
        public string StudentProfileReference { get; set; }
        public int InitialStudentCount { get; set; }
        public int TransfersIn { get; set; }
        public int TransfersOut { get; set; }

        public static TestStudentPopulationProfile Default => new TestStudentPopulationProfile
        {
            StudentProfileReference = TestStudentProfile.Default.Name,
            InitialStudentCount = 1
        };
    }
}