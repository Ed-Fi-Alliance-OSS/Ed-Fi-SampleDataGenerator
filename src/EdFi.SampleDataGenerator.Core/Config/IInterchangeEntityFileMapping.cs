using System;

namespace EdFi.SampleDataGenerator.Core.Config
{
    public interface IInterchangeEntityFileMapping : IFileMapping
    {
        Type InterchangeType { get; }
        Type EntityType { get; }
    }
}
