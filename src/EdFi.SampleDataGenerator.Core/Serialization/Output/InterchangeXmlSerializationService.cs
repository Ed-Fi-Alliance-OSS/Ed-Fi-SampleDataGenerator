using System.IO;
using System.Xml.Serialization;

namespace EdFi.SampleDataGenerator.Core.Serialization.Output
{
    public class InterchangeXmlSerializationService : IInterchangeSerializationService
    {
        public void WriteToOutput<TInterchangeEntity>(TInterchangeEntity interchangeEntity, Stream outputStream)
        {
            var serializer = new XmlSerializer(interchangeEntity.GetType());

            serializer.Serialize(outputStream, interchangeEntity);
            outputStream.Flush();
        }
    }
}