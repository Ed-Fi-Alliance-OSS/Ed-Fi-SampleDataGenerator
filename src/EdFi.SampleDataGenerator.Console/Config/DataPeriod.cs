using System;
using System.Xml.Serialization;
using EdFi.SampleDataGenerator.Core.Config;

namespace EdFi.SampleDataGenerator.Console.Config
{
    public class DataPeriod : IDataPeriod
    {
        [XmlAttribute]
        public string Name { get; set; }

        [XmlElement]
        public DateTime StartDate { get; set; }

        [XmlElement]
        public DateTime EndDate { get; set; }
    }
}