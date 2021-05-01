using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common.Attributes;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common.Entity;
using EdFi.SampleDataGenerator.Core.DataGeneration.InterchangeEntities;

namespace EdFi.SampleDataGenerator.Core.DataGeneration.Generators.StaffAssociation
{
    public sealed class StaffField : EntityField<Entities.Staff>
    {
        private StaffField(Expression<Func<Entities.Staff, object>> expression, IEntity entity, string description = null)
            : base(expression, entity, description)
        {
        }

        public static readonly StaffField StaffUniqueId = new StaffField(x => x.StaffUniqueId, StaffAssociationEntity.Staff);
        public static readonly StaffField StaffIdentificationCode = new StaffField(x => x.StaffIdentificationCode, StaffAssociationEntity.Staff);
        public static readonly StaffField Name = new StaffField(x => x.Name, StaffAssociationEntity.Staff);
        public static readonly StaffField OtherName = new StaffField(x => x.OtherName, StaffAssociationEntity.Staff);
        public static readonly StaffField Sex = new StaffField(x => x.Sex, StaffAssociationEntity.Staff);
        public static readonly StaffField BirthDate = new StaffField(x => x.BirthDate, StaffAssociationEntity.Staff);
        public static readonly StaffField BirthDateSpecified = new StaffField(x => x.BirthDateSpecified, StaffAssociationEntity.Staff);
        public static readonly StaffField Address = new StaffField(x => x.Address, StaffAssociationEntity.Staff);
        public static readonly StaffField InternationalAddress = new StaffField(x => x.InternationalAddress, StaffAssociationEntity.Staff);
        public static readonly StaffField Telephone = new StaffField(x => x.Telephone, StaffAssociationEntity.Staff);
        public static readonly StaffField ElectronicMail = new StaffField(x => x.ElectronicMail, StaffAssociationEntity.Staff);
        public static readonly StaffField HispanicLatinoEthnicity = new StaffField(x => x.HispanicLatinoEthnicity, StaffAssociationEntity.Staff);
        public static readonly StaffField OldEthnicity = new StaffField(x => x.OldEthnicity, StaffAssociationEntity.Staff);
        public static readonly StaffField Race = new StaffField(x => x.Race, StaffAssociationEntity.Staff);
        public static readonly StaffField Citizenship = new StaffField(x => x.Citizenship, StaffAssociationEntity.Staff);
        public static readonly StaffField Language = new StaffField(x => x.Language, StaffAssociationEntity.Staff);
        public static readonly StaffField HighestCompletedLevelOfEducation = new StaffField(x => x.HighestCompletedLevelOfEducation, StaffAssociationEntity.Staff);
        public static readonly StaffField YearsOfPriorProfessionalExperience = new StaffField(x => x.YearsOfPriorProfessionalExperience, StaffAssociationEntity.Staff);
        public static readonly StaffField YearsOfPriorProfessionalExperienceSpecified = new StaffField(x => x.YearsOfPriorProfessionalExperienceSpecified, StaffAssociationEntity.Staff);
        public static readonly StaffField YearsOfPriorTeachingExperience = new StaffField(x => x.YearsOfPriorTeachingExperience, StaffAssociationEntity.Staff);
        public static readonly StaffField YearsOfPriorTeachingExperienceSpecified = new StaffField(x => x.YearsOfPriorTeachingExperienceSpecified, StaffAssociationEntity.Staff);
        public static readonly StaffField LoginId = new StaffField(x => x.LoginId, StaffAssociationEntity.Staff);
        public static readonly StaffField HighlyQualifiedTeacher = new StaffField(x => x.HighlyQualifiedTeacher, StaffAssociationEntity.Staff);
        public static readonly StaffField HighlyQualifiedTeacherSpecified = new StaffField(x => x. HighlyQualifiedTeacherSpecified, StaffAssociationEntity.Staff);

        public static IEnumerable<StaffField> GetAll()
        {
            return typeof(StaffField)
                .GetFields(BindingFlags.Public | BindingFlags.Static)
                .Select(p => (StaffField)p.GetValue(null));
        }
    }
}