using System;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common.Entity;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common.Interchanges;
using EdFi.SampleDataGenerator.Core.Entities;

namespace EdFi.SampleDataGenerator.Core.DataGeneration.InterchangeEntities
{
    public sealed partial class StudentAttendanceEntity : Entity
    {
        private StudentAttendanceEntity(Type entityType) : base(entityType, Interchange.StudentAttendance)
        {
        }

        public static readonly StudentAttendanceEntity StudentSectionAttendanceEvent = new StudentAttendanceEntity(typeof(StudentSectionAttendanceEvent));
    }
}
