using System.Collections.Generic;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common.Interchanges;
using EdFi.SampleDataGenerator.Core.Entities;

namespace EdFi.SampleDataGenerator.Core.Serialization.Output.Interchanges
{
    [InterchangeOutput(Interchange.StudentTranscriptInterchangeName, typeof(InterchangeStudentTranscript), typeof(ComplexObjectType))]
    public partial class StudentTranscriptData
    {
        public List<CourseTranscript> CourseTranscripts { get; set; } = new List<CourseTranscript>();
        public List<StudentAcademicRecord> StudentAcademicRecords { get; set; } = new List<StudentAcademicRecord>();

        [DoNotOutputToInterchange]
        public List<StudentTranscriptSession> StudentTranscriptSessions { get; set; } = new List<StudentTranscriptSession>();
    }
}
