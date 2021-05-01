using System.Xml.Serialization;
using EdFi.SampleDataGenerator.Core.Config;

namespace EdFi.SampleDataGenerator.Console.Config
{
    public class FirstNameFileMapping : IFirstNameFileMapping
    {
        [XmlAttribute]
        public string Ethnicity { get; set; }

        [XmlAttribute]
        public string Gender { get; set; }

        [XmlAttribute]
        public string FilePath { get; set; }
    }
}