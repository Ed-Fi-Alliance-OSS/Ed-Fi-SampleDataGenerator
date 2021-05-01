using System.Collections.Generic;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common.Interchanges;
using EdFi.SampleDataGenerator.Core.DataGeneration.Generators.Parent;
using EdFi.SampleDataGenerator.Core.Entities;
using Parent = EdFi.SampleDataGenerator.Core.Entities.Parent;

namespace EdFi.SampleDataGenerator.Core.Serialization.Output.Interchanges
{
    [InterchangeOutput(Interchange.ParentInterchangeName, typeof(InterchangeParent), typeof(ComplexObjectType))]
    public partial class ParentData
    {
        public Parent Parent1 { get; set; } = new Parent();
        public Parent Parent2 { get; set; } = new Parent();
        public List<StudentParentAssociation> StudentParentAssociations { get; set; } = new List<StudentParentAssociation>();

        [DoNotOutputToInterchange]
        public ParentProfile ParentProfile { get; set; }
    }
}
