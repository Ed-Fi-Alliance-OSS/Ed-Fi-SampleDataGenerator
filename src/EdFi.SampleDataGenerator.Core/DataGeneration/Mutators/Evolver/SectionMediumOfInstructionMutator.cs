using System.Collections.Generic;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common.Attributes;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common.Entity;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common.Mutators;
using EdFi.SampleDataGenerator.Core.DataGeneration.Generators;
using EdFi.SampleDataGenerator.Core.DataGeneration.Generators.MasterSchedule;
using EdFi.SampleDataGenerator.Core.DataGeneration.InterchangeEntities;
using EdFi.SampleDataGenerator.Core.Entities;
using EdFi.SampleDataGenerator.Core.Helpers;

namespace EdFi.SampleDataGenerator.Core.DataGeneration.Mutators.Evolver
{
    public class SectionMediumOfInstructionMutator : GlobalMutator
    {
        public override InterchangeEntity InterchangeEntity => InterchangeEntity.MasterSchedule;
        public override IEntity Entity => MasterScheduleEntity.Section;
        public override IEntityField EntityField => SectionField.MediumOfInstruction;
        public override string Name => "ChangeSectionMediumOfInstruction";
        public override MutationType MutationType => MutationType.Evolution;

        public SectionMediumOfInstructionMutator(IRandomNumberGenerator randomNumberGenerator) : base(randomNumberGenerator)
        {
        }

        protected override IEnumerable<MutationResult> MutateCore(GlobalDataGeneratorContext context)
        {
            foreach (var section in SelectItemsToMutate(context.GlobalData.MasterScheduleData.Sections))
            {
                if (section.MediumOfInstruction != null)
                {
                    var oldValue = section.MediumOfInstruction;

                    section.MediumOfInstruction = null;

                    yield return MutationResult.NewMutation(oldValue, null);
                }
                else
                {
                    var newValue = MediumOfInstructionDescriptor.FaceToFaceInstruction;

                    section.MediumOfInstruction = newValue.GetStructuredCodeValue();

                    yield return MutationResult.NewMutation(null, newValue);
                }
            }
        }
    }
}