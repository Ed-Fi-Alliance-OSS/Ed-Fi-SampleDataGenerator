using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common.Attributes;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common.Entity;
using EdFi.SampleDataGenerator.Core.DataGeneration.InterchangeEntities;
using EdFi.SampleDataGenerator.Core.Entities;
using NUnit.Framework;
using Shouldly;

namespace EdFi.SampleDataGenerator.Core.UnitTests.DataGeneration.Common.Attributes
{
    [TestFixture]
    public class EntityFieldTester
    {
        public sealed class ExampleStudentField : EntityField<Student>
        {
            private ExampleStudentField(Expression<Func<Student, object>> expression, IEntity entity, string description = null)
                : base(expression, entity, description)
            {
            }
            private ExampleStudentField(string fieldName, IEntity entity, string description = null)
                : base(fieldName, entity, description)
            {
            }

            public static ExampleStudentField HomelessStatus = new ExampleStudentField("StudentCharacteristic", StudentEntity.Student, "Homeless Status");
            public static ExampleStudentField ImmigrantStatus = new ExampleStudentField("StudentCharacteristic", StudentEntity.Student, "Immigrant Status");
            public static ExampleStudentField PersonalIdenficationDocument = new ExampleStudentField(x => x.Name.PersonalIdentificationDocument, StudentEntity.Student);
            public static ExampleStudentField BirthData = new ExampleStudentField(x => x.BirthData, StudentEntity.Student);
        }

        [Test]
        public void ShouldInferIEntityFieldStateFromSimpleProperties()
        {
            var field = ExampleStudentField.BirthData;
            field.FieldName.ShouldBe("BirthData");
            field.FullyQualifiedFieldName.ShouldBe("Student.BirthData");
        }

        [Test]
        public void ShouldInferIEntityFieldStateForDifferentUseCasesOfTheSameProperty()
        {
            var firstUseCase = ExampleStudentField.HomelessStatus;
            firstUseCase.FieldName.ShouldBe("StudentCharacteristic");
            firstUseCase.FullyQualifiedFieldName.ShouldBe("Student.StudentCharacteristic (Homeless Status)");

            var secondUseCase = ExampleStudentField.ImmigrantStatus;
            secondUseCase.FieldName.ShouldBe("StudentCharacteristic");
            secondUseCase.FullyQualifiedFieldName.ShouldBe("Student.StudentCharacteristic (Immigrant Status)");
        }

    [Test]
        public void ShouldInferIEntityFieldStateFromNestedProperties()
        {
            var field = ExampleStudentField.PersonalIdenficationDocument;
            field.FieldName.ShouldBe("Name");
            field.FullyQualifiedFieldName.ShouldBe("Student.Name.PersonalIdentificationDocument");
        }

        [Test]
        public void AllValuesMustUniquelyIdentifyAnAttribute()
        {
            var implementations = typeof(IEntityField)
                .Assembly
                .GetTypes()
                .Where(type => typeof(IEntityField).IsAssignableFrom(type) && !type.IsInterface && !type.IsAbstract)
                .ToArray();

            var fullyQualifiedNames = implementations
                .SelectMany(x =>
                    x.GetProperties(BindingFlags.Public | BindingFlags.Static)
                        .Select(p => (IEntityField) p.GetValue(null)))
                .Select(x => x.FullyQualifiedFieldName)
                .ToArray();

            fullyQualifiedNames.ShouldBe(fullyQualifiedNames.Distinct(),
                () => "IEntityField implementations must provide unique values. " +
                      "If an attribute needs to be meaningfully repeated for different " +
                      "use cases, distinguish them by providing the optional 'description' " +
                      "string.");
        }

        [Test]
        public void ShouldDeclareItsContainingEntity()
        {
            ExampleStudentField.HomelessStatus.Entity.ShouldBe(StudentEntity.Student);
            ExampleStudentField.ImmigrantStatus.Entity.ShouldBe(StudentEntity.Student);
            ExampleStudentField.PersonalIdenficationDocument.Entity.ShouldBe(StudentEntity.Student);
        }
    }
}