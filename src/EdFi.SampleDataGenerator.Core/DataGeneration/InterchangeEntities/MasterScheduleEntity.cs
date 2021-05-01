using System;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common.Entity;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common.Interchanges;
using EdFi.SampleDataGenerator.Core.Entities;

namespace EdFi.SampleDataGenerator.Core.DataGeneration.InterchangeEntities
{
    public sealed partial class MasterScheduleEntity : Entity
    {
        private MasterScheduleEntity(Type entityType) : base(entityType, Interchange.MasterSchedule)
        {
        }

        public static readonly MasterScheduleEntity CourseOffering = new MasterScheduleEntity(typeof(CourseOffering));
        public static readonly MasterScheduleEntity Section = new MasterScheduleEntity(typeof(Section));
    }
}
