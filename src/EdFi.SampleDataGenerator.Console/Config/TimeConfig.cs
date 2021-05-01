using System.Xml.Serialization;
using EdFi.SampleDataGenerator.Core.Config;

namespace EdFi.SampleDataGenerator.Console.Config
{
    public class TimeConfig : ITimeConfig
    {
        [XmlElement("SchoolCalendar")]
        public SchoolCalendarConfig SchoolCalendarConfig { get; set; }
        ISchoolCalendarConfig ITimeConfig.SchoolCalendarConfig  => SchoolCalendarConfig;

        [XmlElement("DataClock")]
        public DataClockConfig DataClockConfig { get; set; }
        IDataClockConfig ITimeConfig.DataClockConfig => DataClockConfig;
    }
}
