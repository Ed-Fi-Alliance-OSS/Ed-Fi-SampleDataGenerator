using System.Collections.Generic;
using System.Linq;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common.Attributes;
using EdFi.SampleDataGenerator.Core.DataGeneration.Generators;
using EdFi.SampleDataGenerator.Core.DataGeneration.Generators.Student;
using EdFi.SampleDataGenerator.Core.UnitTests.DataGeneration.Common;
using NUnit.Framework;
using Shouldly;

namespace EdFi.SampleDataGenerator.Core.UnitTests.DataGeneration.Generators.Student.Attributes
{
    [TestFixture]
    public class StudentAttributeGeneratorTester
    {
        public static readonly IEnumerable<IEntityAttributeGenerator<StudentDataGeneratorContext, StudentDataGeneratorConfig>> AllGenerators = GetAllStudentAttributeGenerators();
        public readonly List<string> StudentAttributes = GetAllStudentAttributeNames();
        private static readonly List<StudentField> StudentFieldMappings = StudentField.GetAll().ToList();

        [Test, TestCaseSource(nameof(AllGenerators))]
        public void GeneratorsShouldCorrectlyMapToAStudentAttribute(IEntityAttributeGenerator<StudentDataGeneratorContext, StudentDataGeneratorConfig> instance)
        {
            if (!instance.GeneratesField.IsVirtual)
            {
                StudentAttributes.Contains(instance.FieldName).ShouldBeTrue($"No Attribute named {instance.FieldName} found on Student entity");
            }
        }

        [Test, TestCaseSource(nameof(AllGenerators))]
        public void GeneratorsShouldHaveCorrespondingFieldMapping(IEntityAttributeGenerator<StudentDataGeneratorContext, StudentDataGeneratorConfig> instance)
        {
            StudentFieldMappings.Any(m => m.FieldName == instance.FieldName).ShouldBeTrue($"A mapping must be added to {nameof(StudentField)} for FieldName '{instance.FieldName}'");
        }

        private static IEnumerable<IEntityAttributeGenerator<StudentDataGeneratorContext, StudentDataGeneratorConfig>> GetAllStudentAttributeGenerators()
        {
            var randomNumberGenerator = new TestRandomNumberGenerator();

            return EntityAttributeGeneratorFactory
                .BuildAllAttributeGenerators<StudentDataGeneratorContext, StudentDataGeneratorConfig>(randomNumberGenerator).ToList();
        }

        private static List<string> GetAllStudentAttributeNames()
        {
            var members = typeof(Entities.Student)
                    .GetProperties();

            return members.Select(m => m.Name).ToList();
        }
    }
}
