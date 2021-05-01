using EdFi.SampleDataGenerator.Core.DataGeneration.Common;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common.Attributes;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common.Entity;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common.Mutators;
using EdFi.SampleDataGenerator.Core.DataGeneration.Generators;
using EdFi.SampleDataGenerator.Core.DataGeneration.Generators.Student;
using EdFi.SampleDataGenerator.Core.DataGeneration.InterchangeEntities;
using EdFi.SampleDataGenerator.Core.Entities;

namespace EdFi.SampleDataGenerator.Core.DataGeneration.Mutators.Error
{
    public class SalutationTitleErrorMutator : StudentMutator
    {
        public override InterchangeEntity InterchangeEntity => InterchangeEntity.Student;
        public override IEntity Entity => StudentEntity.Student;
        public override IEntityField EntityField => StudentField.Name;
        public override string Name => "SwapStudentSalutation";
        public override MutationType MutationType => MutationType.Error;

        public SalutationTitleErrorMutator(IRandomNumberGenerator randomNumberGenerator):base(randomNumberGenerator)
        {
        }

        protected override MutationResult MutateCore(StudentDataGeneratorContext context)
        {
            if (context.Student.Name == null) return MutationResult.NoMutation;
            var oldTitle = context.Student.Name.PersonalTitlePrefix;
            context.Student.Name.PersonalTitlePrefix = context.StudentCharacteristics.Sex == SexDescriptor.Female ? Salutation.Mr.DisplayName : Salutation.Ms.DisplayName;
            return MutationResult.NewMutation(oldTitle, context.Student.Name.PersonalTitlePrefix);
        }
    }
}
