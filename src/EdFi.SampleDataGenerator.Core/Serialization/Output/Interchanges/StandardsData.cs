using System.Collections.Generic;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common.Interchanges;
using EdFi.SampleDataGenerator.Core.Entities;

namespace EdFi.SampleDataGenerator.Core.Serialization.Output.Interchanges
{
    [InterchangeOutput(Interchange.StandardsInterchangeName, typeof(InterchangeStandards), typeof(ComplexObjectType))]
    public partial class StandardsData
    {
        public List<LearningStandard> LearningStandards { get; set; } = new List<LearningStandard>();
        public List<LearningObjective> LearningObjectives { get; set; } = new List<LearningObjective>();
    }
}