using EdFi.SampleDataGenerator.Core.DataGeneration.Common;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common.Attributes;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common.Entity;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common.Mutators;
using EdFi.SampleDataGenerator.Core.DataGeneration.Generators;
using EdFi.SampleDataGenerator.Core.DataGeneration.Generators.Student;
using EdFi.SampleDataGenerator.Core.DataGeneration.InterchangeEntities;
using EdFi.SampleDataGenerator.Core.Helpers;

namespace EdFi.SampleDataGenerator.Core.DataGeneration.Mutators.Error
{
    public class StudentMiddleNameErrorMutator : StudentMutator
    {
        public override InterchangeEntity InterchangeEntity => InterchangeEntity.Student;
        public override IEntity Entity => StudentEntity.Student;
        public override IEntityField EntityField => StudentField.Name;
        public override string Name => "SubstituteStudentMiddleName";
        public override MutationType MutationType => MutationType.Error;

        public StudentMiddleNameErrorMutator(IRandomNumberGenerator randomNumberGenerator) : base(randomNumberGenerator)
        {
        }

        protected override MutationResult MutateCore(StudentDataGeneratorContext context)
        {
            var telephone = context.GetStudentEducationOrganization().Telephone;
            if (context.Student.Name == null || telephone == null || telephone.Length == 0) return MutationResult.NoMutation;
            var oldMiddleName = context.Student.Name.MiddleName;
            var telephoneNumberIndex = 0;
            if (telephone.Length > 1)
            {
                telephoneNumberIndex = RandomNumberGenerator.Generate(0, telephone.Length);
            }
            context.Student.Name.MiddleName = telephone[telephoneNumberIndex].TelephoneNumber;
            return MutationResult.NewMutation(oldMiddleName, context.Student.Name.MiddleName);
        }
    }
}
