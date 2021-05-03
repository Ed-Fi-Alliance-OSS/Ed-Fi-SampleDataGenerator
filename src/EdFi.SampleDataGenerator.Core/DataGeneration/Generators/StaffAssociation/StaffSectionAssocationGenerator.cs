using EdFi.SampleDataGenerator.Core.DataGeneration.Common;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common.Dependencies;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common.Entity;
using EdFi.SampleDataGenerator.Core.DataGeneration.InterchangeEntities;
using EdFi.SampleDataGenerator.Core.Entities;
using EdFi.SampleDataGenerator.Core.Helpers;

namespace EdFi.SampleDataGenerator.Core.DataGeneration.Generators.StaffAssociation
{
    public sealed class StaffSectionAssocationGenerator : StaffAssociationEntityGenerator
    {
        public StaffSectionAssocationGenerator(IRandomNumberGenerator randomNumberGenerator)
            : base(randomNumberGenerator)
        {
        }

        public override IEntity GeneratesEntity => StaffAssociationEntity.StaffSectionAssocation;
        public override IEntity[] DependsOnEntities => EntityDependencies.Create(StaffAssociationEntity.StaffRequirement);

        protected override void GenerateCore(GlobalDataGeneratorContext context)
        {
            foreach (var staffMember in context.GlobalData.StaffAssociationData.StaffRequirements)
            {
                foreach (var section in staffMember.SectionReference)
                {
                    context.GlobalData.StaffAssociationData.StaffSectionAssociation.Add(new StaffSectionAssociation
                    {
                        SectionReference = section,
                        HighlyQualifiedTeacher = staffMember.HighlyQualified,
                        StaffReference = staffMember.StaffReference,

                        ClassroomPosition = ClassroomPositionDescriptor.TeacherOfRecord.GetStructuredCodeValue(),

                        TeacherStudentDataLinkExclusionSpecified = false,
                        PercentageContributionSpecified = false,
                        HighlyQualifiedTeacherSpecified = true,
                        EndDateSpecified = false,
                        BeginDateSpecified = false
                    });
                }
            }
        }
    }
}

