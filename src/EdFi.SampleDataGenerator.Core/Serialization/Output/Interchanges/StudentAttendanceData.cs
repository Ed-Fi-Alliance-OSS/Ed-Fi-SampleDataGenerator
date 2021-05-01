using System.Collections.Generic;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common.Interchanges;
using EdFi.SampleDataGenerator.Core.DataGeneration.Generators.StudentAttendance;
using EdFi.SampleDataGenerator.Core.Entities;

namespace EdFi.SampleDataGenerator.Core.Serialization.Output.Interchanges
{
    [InterchangeOutput(Interchange.StudentAttendanceInterchangeName, typeof(InterchangeStudentAttendance), typeof(ComplexObjectType))]
    public partial class StudentAttendanceData
    {
        public List<StudentSectionAttendanceEvent> StudentSectionAttendanceEvents { get; set; } = new List<StudentSectionAttendanceEvent>();

        [DoNotOutputToInterchange]
        public AttendanceTemplate StudentAttendanceTemplate { get; set; }
    }
}
