using System.Collections.Generic;
using System.IO;

namespace EdFi.SampleDataGenerator.Core.Serialization.Output
{
    public interface ISerializationService<in TItem>
    {
        void WriteToOutput(IEnumerable<TItem> items, Stream outputStream);
    }
}