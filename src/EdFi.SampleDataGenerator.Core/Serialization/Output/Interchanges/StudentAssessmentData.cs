using System.Collections.Generic;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common.Interchanges;
using EdFi.SampleDataGenerator.Core.Entities;

namespace EdFi.SampleDataGenerator.Core.Serialization.Output.Interchanges
{
    [InterchangeOutput(Interchange.StudentAssessmentInterchangeName, typeof(InterchangeStudentAssessment), typeof(object))]
    public partial class StudentAssessmentData
    {
        public List<StudentAssessment> StudentAssessments { get; set; } = new List<StudentAssessment>();
    }
}
