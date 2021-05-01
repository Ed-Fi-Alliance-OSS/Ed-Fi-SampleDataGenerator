using System.IO;

namespace EdFi.SampleDataGenerator.Core.Serialization.Output
{
    public interface IInterchangeSerializationService
    {
        void WriteToOutput<TInterchangeEntity>(TInterchangeEntity interchangeEntity, Stream outputStream);
    }
}