using System.Linq;
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
    public class StudentPhoneEvolverMutator : StudentMutator
    {
        public override InterchangeEntity InterchangeEntity => InterchangeEntity.StudentEnrollment;
        public override IEntity Entity => StudentEnrollmentEntity.StudentEducationOrganizationAssociation;
        public override IEntityField EntityField => StudentField.Telephone;
        public override string Name => "ChangeStudentPhone";
        public override MutationType MutationType => MutationType.Evolution;

        public StudentPhoneEvolverMutator(IRandomNumberGenerator randomNumberGenerator) : base(randomNumberGenerator)
        {
        }

        protected override MutationResult MutateCore(StudentDataGeneratorContext context)
        {
            if (context.GetStudentEducationOrganization().HasCharacteristic(StudentCharacteristicDescriptor.Homeless))
                return MutationResult.NoMutation;

            var oldTelephone = context.GetStudentEducationOrganization().Telephone;

            var city = context.GetStudentEducationOrganization().Address.First().City;

            var newTelephone = new[]
            {
                Configuration.StudentConfig.DistrictProfile.LocationInfo.GenerateTelephoneNumber(RandomNumberGenerator, city)
            };
            context.GetStudentEducationOrganization().Telephone = newTelephone;

            return MutationResult.NewMutation(oldTelephone, newTelephone);
        }
    }
}
