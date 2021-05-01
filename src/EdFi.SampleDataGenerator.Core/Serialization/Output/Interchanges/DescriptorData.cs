using System;
using System.Collections.Generic;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common.Interchanges;
using EdFi.SampleDataGenerator.Core.Entities;

namespace EdFi.SampleDataGenerator.Core.Serialization.Output.Interchanges
{
    [InterchangeOutput(Interchange.DescriptorsInterchangeName, typeof(InterchangeDescriptors), typeof(DescriptorType))]
    public partial class DescriptorData
    {
        [DoNotOutputToInterchange]
        public string DescriptorName { get; set; }
        [DoNotOutputToInterchange]
        public Type DescriptorType { get; set; }
        public List<DescriptorType> Descriptors { get; set; } = new List<DescriptorType>();
    }
}