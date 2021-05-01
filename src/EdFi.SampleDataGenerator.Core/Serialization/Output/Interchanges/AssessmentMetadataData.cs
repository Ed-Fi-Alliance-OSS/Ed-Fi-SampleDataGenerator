using System.Collections.Generic;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common.Interchanges;
using EdFi.SampleDataGenerator.Core.Entities;

namespace EdFi.SampleDataGenerator.Core.Serialization.Output.Interchanges
{
    [InterchangeOutput(Interchange.AssessmentMetadataInterchangeName, typeof(InterchangeAssessmentMetadata), typeof(ComplexObjectType))]
    public partial class AssessmentMetadataData
    {
        public List<Assessment> Assessments { get; set; } = new List<Assessment>();
    }
}
