using System;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common.Interchanges;

namespace EdFi.SampleDataGenerator.Core.DataGeneration.Common.Entity
{
    public interface IEntity
    {
        Type EntityType { get; }
        string ClassName { get; }
        Interchange Interchange { get; }
    }
}
