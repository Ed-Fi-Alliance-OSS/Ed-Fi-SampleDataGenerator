using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using EdFi.SampleDataGenerator.Core.Config;

namespace EdFi.SampleDataGenerator.Console.Config
{
    public class DataClockConfig : IDataClockConfig
    {
        [XmlIgnore]
        public DateTime StartDate => DataPeriods.OrderBy(dp => dp.StartDate).Select(dp => dp.StartDate).FirstOrDefault();

        [XmlIgnore]
        public DateTime EndDate => DataPeriods.OrderBy(dp => dp.StartDate).Select(dp => dp.EndDate).LastOrDefault();

        IEnumerable<IDataPeriod> IDataClockConfig.DataPeriods => DataPeriods;

        [XmlElement("DataPeriod")]
        public List<DataPeriod> DataPeriods { get; set; }
    }
}