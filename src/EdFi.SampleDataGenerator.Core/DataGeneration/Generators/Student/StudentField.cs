using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common.Attributes;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common.Entity;
using EdFi.SampleDataGenerator.Core.DataGeneration.InterchangeEntities;

namespace EdFi.SampleDataGenerator.Core.DataGeneration.Generators.Student
{
    public sealed class StudentField : EntityField<Entities.Student>
    {
        private const string StudentCharacteristicFieldName = "StudentCharacteristic";

        private StudentField(Expression<Func<Entities.Student, object>> expression, IEntity entity, string description = null)
            : base(expression, entity, description)
        {
        }
        private StudentField(string virtualFieldName, IEntity entity, string description = null)
            : base(virtualFieldName, entity, description)
        {
        }

        public static readonly StudentField BirthData = new StudentField(x => x.BirthData, StudentEntity.Student);
        public static readonly StudentField Name = new StudentField(x => x.Name, StudentEntity.Student);
        public static readonly StudentField PersonalIdenficationDocument = new StudentField(x => x.Name.PersonalIdentificationDocument, StudentEntity.Student); 
        public static readonly StudentField StudentUniqueId = new StudentField(x => x.StudentUniqueId, StudentEntity.Student);
        public static readonly StudentField Race = new StudentField(StudentCharacteristicFieldName, StudentEntity.Student, "Race");
        public static readonly StudentField Telephone = new StudentField(StudentCharacteristicFieldName, StudentEntity.Student, "Telephone");
        public static readonly StudentField Address = new StudentField(StudentCharacteristicFieldName, StudentEntity.Student, "Address");
        public static readonly StudentField ImmigrantStatus = new StudentField(StudentCharacteristicFieldName, StudentEntity.Student, "Immigrant Status");
        public static readonly StudentField Sex = new StudentField(StudentCharacteristicFieldName, StudentEntity.Student, "Sex");

        public static IEnumerable<StudentField> GetAll()
        {
            return typeof(StudentField)
                .GetFields(BindingFlags.Public | BindingFlags.Static)
                .Select(p => (StudentField)p.GetValue(null));
        }
    }
}
