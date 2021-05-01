using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common.Attributes;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common.Entity;
using EdFi.SampleDataGenerator.Core.DataGeneration.InterchangeEntities;
using EdFi.SampleDataGenerator.Core.Serialization.Output.Interchanges;

namespace EdFi.SampleDataGenerator.Core.DataGeneration.Generators.StudentEnrollment
{
    public sealed class StudentEnrollmentField : EntityField<StudentEnrollmentData>
    {
        private StudentEnrollmentField(Expression<Func<StudentEnrollmentData, object>> expression, IEntity entity, string description = null)
            : base(expression, entity, description)
        {
        }

        public static StudentEnrollmentField StudentSectionAssociationList = new StudentEnrollmentField(x => x.StudentSectionAssociations, StudentEnrollmentEntity.StudentSectionAssociation);

        public static IEnumerable<StudentEnrollmentField> GetAll()
        {
            return typeof(StudentEnrollmentField)
                .GetFields(BindingFlags.Public | BindingFlags.Static)
                .Select(p => (StudentEnrollmentField)p.GetValue(null));
        }
    }
}
