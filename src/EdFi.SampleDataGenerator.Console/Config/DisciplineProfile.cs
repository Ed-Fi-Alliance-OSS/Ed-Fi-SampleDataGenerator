using System.Xml.Serialization;
using EdFi.SampleDataGenerator.Core.Config;

namespace EdFi.SampleDataGenerator.Console.Config
{
    public class DisciplineProfile : IDisciplineProfile
    {
        [XmlAttribute]
        public int TotalExpectedDisciplineEvents { get; set; }

        [XmlAttribute]
        public int TotalExpectedSeriousDisciplineEvents { get; set; }
    }
}