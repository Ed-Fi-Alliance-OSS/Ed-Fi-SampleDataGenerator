using System;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common.Entity;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common.Interchanges;
using EdFi.SampleDataGenerator.Core.DataGeneration.Generators.StaffAssociation;
using EdFi.SampleDataGenerator.Core.Entities;

namespace EdFi.SampleDataGenerator.Core.DataGeneration.InterchangeEntities
{
    public sealed partial class StaffAssociationEntity : Entity
    {
        private StaffAssociationEntity(Type entityType) : base(entityType, Interchange.StaffAssociation)
        {
        }

        public static readonly StaffAssociationEntity StaffRequirement = new StaffAssociationEntity(typeof(StaffRequirement));
        public static readonly StaffAssociationEntity Staff = new StaffAssociationEntity(typeof(Staff));
        public static readonly StaffAssociationEntity StaffSectionAssocation = new StaffAssociationEntity(typeof(StaffSectionAssociation));
        public static readonly StaffAssociationEntity StaffSchoolAssociation = new StaffAssociationEntity(typeof(StaffSchoolAssociation));
        public static readonly StaffAssociationEntity StaffEducationOrganizationEmploymentAssociation = new StaffAssociationEntity(typeof(StaffEducationOrganizationEmploymentAssociation));
        public static readonly StaffAssociationEntity StaffEducationOrganizationAssignmentAssociation = new StaffAssociationEntity(typeof(StaffEducationOrganizationAssignmentAssociation));
    }
}
