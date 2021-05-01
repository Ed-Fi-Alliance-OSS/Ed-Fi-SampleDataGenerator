using System.Collections.Generic;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common.Interchanges;
using EdFi.SampleDataGenerator.Core.Entities;

namespace EdFi.SampleDataGenerator.Core.Serialization.Output.Interchanges
{
    [InterchangeOutput(Interchange.MasterScheduleInterchangeName, typeof(InterchangeMasterSchedule), typeof(ComplexObjectType))]
    public partial class MasterScheduleData
    {
        public List<CourseOffering> CourseOfferings { get; set; } = new List<CourseOffering>();
        public List<Section> Sections { get; set; } = new List<Section>();
    }
}