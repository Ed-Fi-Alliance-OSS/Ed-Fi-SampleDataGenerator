using System.Collections.Generic;

namespace EdFi.SampleDataGenerator.Core.Config.DataFiles
{
    public interface INameFileReader
    {
        IEnumerable<NameFileRecord> Read(string fileName);
    }
}
