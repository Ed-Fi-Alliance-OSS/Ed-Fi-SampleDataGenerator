using System.Collections.Generic;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common.Interchanges;
using EdFi.SampleDataGenerator.Core.Entities;

namespace EdFi.SampleDataGenerator.Core.Serialization.Output.Interchanges
{
    [InterchangeOutput(Interchange.StudentGradebookInterchangeName, typeof(InterchangeStudentGradebook), typeof(object))]
    public partial class StudentGradebookData
    {
        public List<StudentGradebookEntry> StudentGradebookEntries { get; set; } = new List<StudentGradebookEntry>();
    }
}
