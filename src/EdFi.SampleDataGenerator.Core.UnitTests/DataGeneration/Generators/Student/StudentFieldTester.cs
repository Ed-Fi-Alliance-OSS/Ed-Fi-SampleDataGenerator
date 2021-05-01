using System.Collections.Generic;
using System.Linq;
using EdFi.SampleDataGenerator.Core.DataGeneration.Generators.Student;
using NUnit.Framework;
using Shouldly;

namespace EdFi.SampleDataGenerator.Core.UnitTests.DataGeneration.Generators.Student
{
    [TestFixture]
    public class StudentFieldTester
    {
        private static readonly List<string> AllStudentAttributeNames = GetAllStudentAttributeNames();
            
        [Test, TestCaseSource(nameof(GetAllStudentFields))]
        public void ShouldMapToAnAttributeOnTheStudentEntity(StudentField mapping)
        {
            if (!mapping.IsVirtual)
            {
                AllStudentAttributeNames.Contains(mapping.FieldName).ShouldBeTrue($"'{mapping.FieldName}' is not a valid property of Entities.Student");
            }
        }

        private static IEnumerable<StudentField> GetAllStudentFields()
        {
            return StudentField.GetAll();
        }

        private static List<string> GetAllStudentAttributeNames()
        {
            var members = typeof(Entities.Student)
                    .GetProperties();

            return members.Select(m => m.Name).ToList();
        }
    }
}
