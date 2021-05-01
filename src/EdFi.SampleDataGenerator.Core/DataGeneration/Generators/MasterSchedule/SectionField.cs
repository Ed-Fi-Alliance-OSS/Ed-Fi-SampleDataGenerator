using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common.Attributes;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common.Entity;
using EdFi.SampleDataGenerator.Core.DataGeneration.InterchangeEntities;

namespace EdFi.SampleDataGenerator.Core.DataGeneration.Generators.MasterSchedule
{
    public sealed class SectionField : EntityField<Entities.Section>
    {
        private SectionField(Expression<Func<Entities.Section, object>> expression, IEntity entity, string description = null)
            : base(expression, entity, description)
        {
        }

        public static readonly SectionField SectionIdentifier = new SectionField(x => x.SectionIdentifier, MasterScheduleEntity.Section);
        public static readonly SectionField SequenceOfCourse = new SectionField(x => x.SequenceOfCourse, MasterScheduleEntity.Section);
        public static readonly SectionField EducationalEnvironment = new SectionField(x => x.EducationalEnvironment, MasterScheduleEntity.Section);
        public static readonly SectionField MediumOfInstruction = new SectionField(x => x.MediumOfInstruction, MasterScheduleEntity.Section);
        public static readonly SectionField PopulationServed = new SectionField(x => x.PopulationServed, MasterScheduleEntity.Section);
        public static readonly SectionField AvailableCredits = new SectionField(x => x.AvailableCredits, MasterScheduleEntity.Section);
        public static readonly SectionField SectionCharacteristic = new SectionField(x => x.SectionCharacteristic, MasterScheduleEntity.Section);
        public static readonly SectionField InstructionLanguage = new SectionField(x => x.InstructionLanguage, MasterScheduleEntity.Section);
        public static readonly SectionField CourseOfferingReference = new SectionField(x => x.CourseOfferingReference, MasterScheduleEntity.Section);
        public static readonly SectionField LocationSchoolReference = new SectionField(x => x.LocationSchoolReference, MasterScheduleEntity.Section);
        public static readonly SectionField LocationReference = new SectionField(x => x.LocationReference, MasterScheduleEntity.Section);
        public static readonly SectionField ClassPeriodReference = new SectionField(x => x.ClassPeriodReference, MasterScheduleEntity.Section);
        public static readonly SectionField ProgramReference = new SectionField(x => x.ProgramReference, MasterScheduleEntity.Section);

        public static IEnumerable<SectionField> GetAll()
        {
            return typeof(SectionField)
                .GetFields(BindingFlags.Public | BindingFlags.Static)
                .Select(p => (SectionField)p.GetValue(null));
        }
    }
}