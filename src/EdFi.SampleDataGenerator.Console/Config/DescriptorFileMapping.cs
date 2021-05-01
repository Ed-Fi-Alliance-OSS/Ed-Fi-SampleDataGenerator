using System.Xml.Serialization;
using EdFi.SampleDataGenerator.Core.Config;

namespace EdFi.SampleDataGenerator.Console.Config
{
    public class DescriptorFileMapping : IDescriptorFileMapping
    {
        [XmlAttribute]
        public string FilePath { get; set; }

        [XmlAttribute]
        public string DescriptorName { get; set; }
    }
}
