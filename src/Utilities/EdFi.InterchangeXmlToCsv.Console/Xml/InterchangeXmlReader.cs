using System.IO;
using System.Xml.Serialization;

namespace EdFi.InterchangeXmlToCsv.Console.Xml
{
    public interface IInterchangeXmlReader
    {
        InterchangeItemCollection ReadFile(string filePath);
    }

    public abstract class InterchangeXmlReader<TInterchange> : IInterchangeXmlReader
    {
        public InterchangeItemCollection ReadFile(string filePath)
        {
            var s = new XmlSerializer(typeof(TInterchange));
            using (var fileStream = new FileStream(filePath, FileMode.Open))
            {
                var interchangeEntities = s.Deserialize(fileStream);
                var entitiesToOutput = ((TInterchange) interchangeEntities);

                return GetInterchangeItems(entitiesToOutput);
            }
        }

        protected abstract InterchangeItemCollection GetInterchangeItems(TInterchange rootInterchangeItem);
    }
}