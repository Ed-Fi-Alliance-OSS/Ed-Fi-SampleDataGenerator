using System.Xml.Serialization;
using EdFi.SampleDataGenerator.Core.Config;

namespace EdFi.SampleDataGenerator.Console.Config
{
    public class SurnameFileMapping : ISurnameFileMapping
    {
        [XmlAttribute]
        public string Ethnicity { get; set; }

        [XmlAttribute]
        public string FilePath { get; set; }
    }
}