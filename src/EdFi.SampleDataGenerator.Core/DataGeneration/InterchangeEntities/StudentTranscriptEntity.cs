using System;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common.Entity;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common.Interchanges;
using EdFi.SampleDataGenerator.Core.Entities;
using EdFi.SampleDataGenerator.Core.Serialization.Output;

namespace EdFi.SampleDataGenerator.Core.DataGeneration.InterchangeEntities
{
    public sealed partial class StudentTranscriptEntity : Entity
    {
        private StudentTranscriptEntity(Type entityType) : base(entityType, Interchange.StudentTranscript)
        {
        }

        public static readonly StudentTranscriptEntity StudentAcademicRecord = new StudentTranscriptEntity(typeof(StudentAcademicRecord));
        public static readonly StudentTranscriptEntity CourseTranscript = new StudentTranscriptEntity(typeof(CourseTranscript));

        //supporting entity only - not actually part of the Ed-Fi Model
        public static readonly StudentTranscriptEntity StudentTranscriptSession = new StudentTranscriptEntity(typeof(StudentTranscriptSession));
    }
}
