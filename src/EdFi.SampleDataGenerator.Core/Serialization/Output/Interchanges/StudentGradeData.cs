using System.Collections.Generic;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common.Interchanges;
using EdFi.SampleDataGenerator.Core.Entities;

namespace EdFi.SampleDataGenerator.Core.Serialization.Output.Interchanges
{
    [InterchangeOutput(Interchange.StudentGradeInterchangeName, typeof(InterchangeStudentGrade), typeof(ComplexObjectType))]
    public partial class StudentGradeData
    {
        public List<Grade> Grades { get; set; } = new List<Grade>();
    }
}
