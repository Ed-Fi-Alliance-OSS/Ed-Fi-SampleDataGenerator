using System;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common.Interchanges;

namespace EdFi.SampleDataGenerator.Core.Serialization.Output
{
    public class InterchangeOutputInfo
    {
        public Interchange Interchange { get; set; }
        public Type InterchangeOutputType { get; set; }
        public Type InterchangeItemType { get; set; }
    }
}