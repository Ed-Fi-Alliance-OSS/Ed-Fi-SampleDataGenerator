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
    public class PostalCodeErrorMutator : StudentMutator
    {
        public override InterchangeEntity InterchangeEntity => InterchangeEntity.StudentEnrollment;
        public override IEntity Entity => StudentEnrollmentEntity.StudentEducationOrganizationAssociation;
        public override IEntityField EntityField => StudentField.Address;
        public override string Name => "TransposePostalCodeDigits";
        public override MutationType MutationType => MutationType.Error;

        public PostalCodeErrorMutator(IRandomNumberGenerator randomNumberGenerator) : base(randomNumberGenerator)
        {

        }

        protected override MutationResult MutateCore(StudentDataGeneratorContext context)
        {
            var address = context.GetStudentEducationOrganization().Address;
            if (address == null || address.Length <= 0) return MutationResult.NoMutation;
            var adressIndex = 0;
            if (address.Length > 1)
            {
                adressIndex = RandomNumberGenerator.Generate(0, address.Length);
            }
            var postalCode = address[adressIndex].PostalCode;
            var swapIndex = RandomNumberGenerator.Generate(0, postalCode.Length - 1);
            address[adressIndex].PostalCode = postalCode.SwapCharacters(swapIndex, (swapIndex + 1) % postalCode.Length);
            context.GetStudentEducationOrganization().Address = address;
            return MutationResult.NewMutation(postalCode, address[adressIndex].PostalCode);
        }
    }
}
