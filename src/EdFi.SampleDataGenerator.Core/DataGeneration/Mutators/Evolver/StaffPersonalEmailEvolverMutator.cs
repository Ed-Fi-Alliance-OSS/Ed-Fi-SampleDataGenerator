using System.Collections.Generic;
using System.Linq;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common.Attributes;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common.Entity;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common.Mutators;
using EdFi.SampleDataGenerator.Core.DataGeneration.Generators;
using EdFi.SampleDataGenerator.Core.DataGeneration.Generators.StaffAssociation;
using EdFi.SampleDataGenerator.Core.DataGeneration.InterchangeEntities;
using EdFi.SampleDataGenerator.Core.Entities;

namespace EdFi.SampleDataGenerator.Core.DataGeneration.Mutators.Evolver
{
    public class StaffPersonalEmailEvolverMutator : GlobalMutator
    {
        public override InterchangeEntity InterchangeEntity => InterchangeEntity.StaffAssociation;
        public override IEntity Entity => StaffAssociationEntity.Staff;
        public override IEntityField EntityField => StaffField.ElectronicMail;
        public override string Name => "ChangeStaffPersonalEmail";
        public override MutationType MutationType => MutationType.Evolution;

        public StaffPersonalEmailEvolverMutator(IRandomNumberGenerator randomNumberGenerator) : base(randomNumberGenerator)
        {
        }

        protected override IEnumerable<MutationResult> MutateCore(GlobalDataGeneratorContext context)
        {
            foreach (var staffMemberToUpdate in SelectItemsToMutate(context.GlobalData.StaffAssociationData.Staff))
            {
                var emailAddressCount = staffMemberToUpdate.ElectronicMail.Length;
                if (emailAddressCount == 1 || emailAddressCount == 2)
                {
                    var oldValue = staffMemberToUpdate.ElectronicMail.ElementAtOrDefault(1);
                    var newValue = GeneratePersonalEmailAddress(staffMemberToUpdate);

                    staffMemberToUpdate.ElectronicMail = new[]
                    {
                        staffMemberToUpdate.ElectronicMail.First(),
                        newValue
                    };

                    yield return MutationResult.NewMutation(oldValue, newValue);
                }
            }
        }

        private ElectronicMail GeneratePersonalEmailAddress(Staff staffMemberToUpdate)
        {
            return BiographicalGeneratorHelpers.GeneratePersonalEmailAddress(staffMemberToUpdate.Name, RandomNumberGenerator.Generate(1, 1000));
        }
    }
}