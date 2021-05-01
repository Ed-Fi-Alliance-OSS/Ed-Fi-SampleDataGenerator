using EdFi.SampleDataGenerator.Core.DataGeneration.Common;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common.Attributes;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common.Entity;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common.Mutators;
using EdFi.SampleDataGenerator.Core.DataGeneration.Generators;
using EdFi.SampleDataGenerator.Core.DataGeneration.Generators.Student;
using EdFi.SampleDataGenerator.Core.DataGeneration.InterchangeEntities;
using EdFi.SampleDataGenerator.Core.Entities;
using EdFi.SampleDataGenerator.Core.Helpers;

namespace EdFi.SampleDataGenerator.Core.DataGeneration.Mutators.Error
{
    public class AreaCodeErrorMutator : StudentMutator
    {
        public override InterchangeEntity InterchangeEntity => InterchangeEntity.StudentEnrollment;
        public override IEntity Entity => StudentEnrollmentEntity.StudentEducationOrganizationAssociation;
        public override IEntityField EntityField => null ;
        public override string Name => "TransposeAreaCodeDigits";
        public override MutationType MutationType => MutationType.Error;

        public AreaCodeErrorMutator(IRandomNumberGenerator randomNumberGenerator) : base(randomNumberGenerator)
        {
        }

        protected override MutationResult MutateCore(StudentDataGeneratorContext context)
        {
            var telephone = context.GetStudentEducationOrganization().Telephone;

            if (telephone == null || telephone.Length <= 0)
                return MutationResult.NoMutation;

            var telephoneNumberIndex = 0;
            if (telephone.Length > 1)
            {
                telephoneNumberIndex = RandomNumberGenerator.Generate(0, telephone.Length);
            }
            var oldTelephoneNumber = telephone[telephoneNumberIndex].TelephoneNumber;
            var phoneSections =
                TelephoneHelpers.ParseNumber(telephone[telephoneNumberIndex].TelephoneNumber);

            var areaCode = phoneSections[0];
            var swapIndex = RandomNumberGenerator.Generate(0, 2);
            areaCode = areaCode.SwapCharacters(swapIndex, (swapIndex + 1) % areaCode.Length);

            telephone[telephoneNumberIndex].TelephoneNumber =
                TelephoneHelpers.BuildNumber(areaCode, phoneSections[1], phoneSections[2]);

            context.GetStudentEducationOrganization().Telephone = telephone;

            return MutationResult.NewMutation(oldTelephoneNumber, telephone[telephoneNumberIndex].TelephoneNumber);
        }
    }
}
