using System;
using System.Collections.Generic;
using EdFi.SampleDataGenerator.Core.Config;

namespace EdFi.SampleDataGenerator.Core.UnitTests.Config
{
    public class TestTimeConfig : ITimeConfig
    {
        public ISchoolCalendarConfig SchoolCalendarConfig { get; set; }
        public IDataClockConfig DataClockConfig { get; set; }

        public static TestTimeConfig Default => new TestTimeConfig
        {
            DataClockConfig = TestDataClockConfig.Default,
            SchoolCalendarConfig = TestSchoolCalendarConfig.Default
        };
    }

    public class TestSchoolCalendarConfig : ISchoolCalendarConfig
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public static TestSchoolCalendarConfig Default => new TestSchoolCalendarConfig
        {
            StartDate = new DateTime(2016, 8, 22),
            EndDate = new DateTime(2017, 5, 8)
        };
    }

    public class TestDataPeriod : IDataPeriod
    {
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public static TestDataPeriod Default => new TestDataPeriod
        {
            Name = "2016-2017",
            StartDate = new DateTime(2016, 8, 22),
            EndDate = new DateTime(2017, 5, 8),
        };
    }

    public class TestDataClockConfig : IDataClockConfig
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public IEnumerable<IDataPeriod> DataPeriods { get; set; }

        public static TestDataClockConfig Default => new TestDataClockConfig
        {
            StartDate = new DateTime(2016, 8, 22),
            EndDate = new DateTime(2017, 5, 8),
            DataPeriods = new List<IDataPeriod>
            {
                TestDataPeriod.Default
            }
        };
    }
}
