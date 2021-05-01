using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common.Interchanges;

namespace EdFi.SampleDataGenerator.Core.Serialization.Output
{
    public class Manifest
    {
        private class Item
        {
            public Item(Interchange interchange, string filename)
            {
                Interchange = interchange;
                Filename = filename;
            }

            public Interchange Interchange { get; }
            public string Filename { get; }
        }

        private readonly List<Item> _items = new List<Item>();

        public void Add(Interchange interchange, string filename)
        {
            _items.Add(new Item(interchange, filename));
        }

        public XDocument ToXml()
        {
            return new XDocument(
                new XElement("Interchanges",
                    _items.Select(item =>
                        new XElement("Interchange",
                            new XElement("Filename", item.Filename),
                            new XElement("Type", item.Interchange.Name)))));
        }

        public void Save(string outputFilePath)
        {
            if (_items.Any())
                ToXml().Save(outputFilePath);
        }
    }
}