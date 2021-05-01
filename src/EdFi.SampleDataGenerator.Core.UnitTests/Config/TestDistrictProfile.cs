using EdFi.SampleDataGenerator.Core.Config;

namespace EdFi.SampleDataGenerator.Core.UnitTests.Config
{
    public class TestDistrictProfile : IDistrictProfile
    {
        public string DistrictName { get; set; }
        public double? HighPerformingStudentPercentile { get; set; }
        public ISchoolProfile[] SchoolProfiles { get; set; }
        public ILocationInfo LocationInfo { get; set; }

        public static TestDistrictProfile Default => new TestDistrictProfile
        {
            LocationInfo = TestLocationInfo.Default,
            DistrictName = "Test District",
            SchoolProfiles = new ISchoolProfile[]
            {
                TestSchoolProfile.Default
            }
        };
    }
}