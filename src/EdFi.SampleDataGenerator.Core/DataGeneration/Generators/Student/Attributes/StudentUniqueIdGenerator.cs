using EdFi.SampleDataGenerator.Core.DataGeneration.Common;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common.Attributes;

namespace EdFi.SampleDataGenerator.Core.DataGeneration.Generators.Student.Attributes
{
    public class StudentUniqueIdGenerator : SampleDataEntityAttributeGeneratorBase<StudentDataGeneratorContext, StudentDataGeneratorConfig>
    {
        public override IEntityField GeneratesField => StudentField.StudentUniqueId;
        public override IEntityField[] DependsOnFields => NoDependencies;

        public StudentUniqueIdGenerator(IRandomNumberGenerator randomNumberGenerator) : base(randomNumberGenerator)
        {
        }

        protected override void GenerateCore(StudentDataGeneratorContext context)
        {
            var id = $"{context.GlobalStudentNumber:D6}";
            context.Student.StudentUniqueId = id;
            context.Student.id = $"STU_{id}";
        }
    }
}