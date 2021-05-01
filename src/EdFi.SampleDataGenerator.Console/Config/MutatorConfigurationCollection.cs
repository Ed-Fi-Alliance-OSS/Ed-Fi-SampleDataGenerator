using System.Xml.Serialization;
using EdFi.SampleDataGenerator.Core.Config;

namespace EdFi.SampleDataGenerator.Console.Config
{
    public class MutatorConfigurationCollection : IMutatorConfigurationCollection
    {
        [XmlElement("Mutator")]
        public MutatorConfiguration[] Mutators { get; set; }
        IMutatorConfiguration[] IMutatorConfigurationCollection.Mutators => Mutators;
    }

    public class MutatorConfiguration : IMutatorConfiguration
    {
        [XmlAttribute]
        public string Name { get; set; }
        [XmlAttribute]
        public double Probability { get; set; }
    }
}
