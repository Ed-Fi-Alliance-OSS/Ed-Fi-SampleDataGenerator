using EdFi.SampleDataGenerator.Core.DataGeneration.Common;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common.Attributes;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common.Entity;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common.Mutators;
using EdFi.SampleDataGenerator.Core.DataGeneration.Generators;
using EdFi.SampleDataGenerator.Core.DataGeneration.Generators.Student;
using EdFi.SampleDataGenerator.Core.DataGeneration.InterchangeEntities;
using EdFi.SampleDataGenerator.Core.Entities;
using EdFi.SampleDataGenerator.Core.Helpers;

namespace EdFi.SampleDataGenerator.Core.DataGeneration.Mutators.Evolver
{
    public class StudentAdressEvolverMutator : StudentMutator
    {
        public override InterchangeEntity InterchangeEntity => InterchangeEntity.Student;
        public override IEntity Entity => StudentEntity.Student;
        public override IEntityField EntityField => StudentField.Address;
        public override string Name => "ChangeStudentAddress";
        public override MutationType MutationType => MutationType.Evolution;

        public StudentAdressEvolverMutator(IRandomNumberGenerator randomNumberGenerator) : base(randomNumberGenerator)
        {

        }
        protected override MutationResult MutateCore(StudentDataGeneratorContext context)
        {
            var oldAddress = context.GetStudentEducationOrganization().Address;
            var newAddress = new[]
            {
                Configuration.StudentConfig.DistrictProfile.LocationInfo.GenerateHomeAddress(RandomNumberGenerator, Configuration.StudentConfig.NameFileData.StreetNameFile)
            };
            context.GetStudentEducationOrganization().Address = newAddress;

            if (context.GetStudentEducationOrganization().HasCharacteristic(StudentCharacteristicDescriptor.Homeless))
            {
                context.GetStudentEducationOrganization().RemoveCharacteristic(StudentCharacteristicDescriptor.Homeless);
            }
            return MutationResult.NewMutation(oldAddress, newAddress);
        }
    }
}
