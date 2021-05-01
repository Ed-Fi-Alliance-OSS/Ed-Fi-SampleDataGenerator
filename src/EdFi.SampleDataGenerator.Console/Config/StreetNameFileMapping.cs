using System.Xml.Serialization;
using EdFi.SampleDataGenerator.Core.Config;

namespace EdFi.SampleDataGenerator.Console.Config
{
    public class StreetNameFileMapping : IStreetNameFileMapping
    {
        [XmlAttribute]
        public string FilePath { get; set; }
    }
}