using System;
using System.Linq;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common.Dependencies;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common.Entity;
using EdFi.SampleDataGenerator.Core.DataGeneration.InterchangeEntities;
using EdFi.SampleDataGenerator.Core.Entities;
using EdFi.SampleDataGenerator.Core.Helpers;

namespace EdFi.SampleDataGenerator.Core.DataGeneration.Generators.StaffAssociation
{
    public sealed class StaffEducationOrganizationAssignmentAssociationGenerator : StaffAssociationEntityGenerator
    {
        public StaffEducationOrganizationAssignmentAssociationGenerator(IRandomNumberGenerator randomNumberGenerator)
            : base(randomNumberGenerator)
        {
        }

        public override IEntity GeneratesEntity => StaffAssociationEntity.StaffEducationOrganizationAssignmentAssociation;

        public override IEntity[] DependsOnEntities => EntityDependencies.Create(StaffAssociationEntity.StaffRequirement, StaffAssociationEntity.StaffEducationOrganizationEmploymentAssociation);

        protected override void GenerateCore(GlobalDataGeneratorContext context)
        {
            foreach (var staffRequirement in context.GlobalData.StaffAssociationData.StaffRequirements)
            {
                context.GlobalData.StaffAssociationData.StaffEducationOrganizationAssignmentAssociation.Add(new StaffEducationOrganizationAssignmentAssociation
                {
                    StaffReference = staffRequirement.StaffReference,
                    BeginDate = GetHireDate(context, staffRequirement.StaffReference.StaffIdentity.StaffUniqueId),
                    EducationOrganizationReference = EdFiReferenceTypeHelpers.GetEducationOrganizationReference(staffRequirement.EducationOrganizationId),
                    EndDateSpecified = false,
                    PositionTitle = GetTitle(staffRequirement),
                    StaffClassification = staffRequirement.StaffClassification.GetStructuredCodeValue(),
                });
            }
        }

        private string GetTitle(StaffRequirement staffRequirement)
        {
            if (staffRequirement.StaffClassification == StaffClassificationDescriptor.Teacher && staffRequirement.GradeLevel.Any())
            {
                return $"{staffRequirement.GradeLevel.First().CodeValue} Teacher";
            }
            return staffRequirement.StaffClassification.CodeValue;
        }

        private static DateTime GetHireDate(GlobalDataGeneratorContext context, string staffUniqueId)
        {
            return context
                .GlobalData
                .StaffAssociationData
                .StaffEducationOrganizationEmploymentAssociation
                .First(x => x.StaffReference.StaffIdentity.StaffUniqueId == staffUniqueId)
                .EmploymentPeriod
                .HireDate;
        }
    }
}
