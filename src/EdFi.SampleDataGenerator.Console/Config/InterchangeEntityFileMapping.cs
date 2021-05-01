using System;
using System.Xml.Serialization;
using EdFi.SampleDataGenerator.Core.Config;

namespace EdFi.SampleDataGenerator.Console.Config
{
    public class InterchangeEntityFileMapping : IInterchangeEntityFileMapping
    {
        [XmlAttribute]
        public string FilePath { get; set; }

        public Type InterchangeType { get; set; }
        public Type EntityType { get; set; }
    }
}
