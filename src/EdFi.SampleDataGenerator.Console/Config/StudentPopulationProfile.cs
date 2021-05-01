using System.Xml.Serialization;
using EdFi.SampleDataGenerator.Core.Config;

namespace EdFi.SampleDataGenerator.Console.Config
{
    public class StudentPopulationProfile : IStudentPopulationProfile
    {
        [XmlAttribute]
        public string StudentProfileReference { get; set; }

        [XmlAttribute]
        public int InitialStudentCount { get; set; }

        [XmlAttribute]
        public int TransfersIn { get; set; }

        [XmlAttribute]
        public int TransfersOut { get; set; }
    }
}