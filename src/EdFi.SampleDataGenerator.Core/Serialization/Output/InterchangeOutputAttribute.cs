using System;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common.Interchanges;

namespace EdFi.SampleDataGenerator.Core.Serialization.Output
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property)]
    public class InterchangeOutputAttribute : Attribute
    {
        public InterchangeOutputInfo InterchangeOutputInfo { get; }

        public InterchangeOutputAttribute(string interchangeName, Type interchangeOutputType, Type interchangeItemType)
        {
            InterchangeOutputInfo = new InterchangeOutputInfo
            {
                Interchange = Interchange.FromValue(interchangeName),
                InterchangeOutputType = interchangeOutputType,
                InterchangeItemType = interchangeItemType
            };
        }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class DoNotOutputToInterchangeAttribute : Attribute
    {
    }
}
