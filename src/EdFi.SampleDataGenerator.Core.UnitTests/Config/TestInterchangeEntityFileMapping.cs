using System;
using EdFi.SampleDataGenerator.Core.Config;

namespace EdFi.SampleDataGenerator.Core.UnitTests.Config
{
    public class TestInterchangeEntityFileMapping : IInterchangeEntityFileMapping
    {
        public string FilePath { get; set; }
        public Type InterchangeType { get; set; }
        public Type EntityType { get; set; }
    }
}
