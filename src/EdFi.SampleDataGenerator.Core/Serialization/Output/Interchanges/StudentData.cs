using EdFi.SampleDataGenerator.Core.DataGeneration.Common.Interchanges;
using EdFi.SampleDataGenerator.Core.Entities;

namespace EdFi.SampleDataGenerator.Core.Serialization.Output.Interchanges
{
    [InterchangeOutput(Interchange.StudentInterchangeName, typeof(InterchangeStudent), typeof(Student))]
    public partial class StudentData
    {
        public Student Student { get; set; } = new Student();
    }
}