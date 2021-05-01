using System;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common.Entity;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common.Interchanges;
using EdFi.SampleDataGenerator.Core.Entities;

namespace EdFi.SampleDataGenerator.Core.DataGeneration.InterchangeEntities
{
    public sealed partial class EducationOrganizationEntity : Entity
    {
        private EducationOrganizationEntity(Type entityType) : base(entityType, Interchange.EducationOrganization)
        {
        }

        public static readonly EducationOrganizationEntity StateEducationAgency = new EducationOrganizationEntity(typeof(StateEducationAgency));
        public static readonly EducationOrganizationEntity EducationServiceCenter = new EducationOrganizationEntity(typeof(EducationServiceCenter));
        public static readonly EducationOrganizationEntity FeederSchoolAssociation = new EducationOrganizationEntity(typeof(FeederSchoolAssociation));
        public static readonly EducationOrganizationEntity LocalEducationAgency = new EducationOrganizationEntity(typeof(LocalEducationAgency));
        public static readonly EducationOrganizationEntity School = new EducationOrganizationEntity(typeof(School));
        public static readonly EducationOrganizationEntity Location = new EducationOrganizationEntity(typeof(Location));
        public static readonly EducationOrganizationEntity ClassPeriod = new EducationOrganizationEntity(typeof(ClassPeriod));
        public static readonly EducationOrganizationEntity Course = new EducationOrganizationEntity(typeof(Course));
        public static readonly EducationOrganizationEntity Program = new EducationOrganizationEntity(typeof(Program));
        public static readonly EducationOrganizationEntity AccountabilityRating = new EducationOrganizationEntity(typeof(AccountabilityRating));
        public static readonly EducationOrganizationEntity EducationOrganizationPeerAssociation = new EducationOrganizationEntity(typeof(EducationOrganizationPeerAssociation));
        public static readonly EducationOrganizationEntity EducationOrganizationNetwork = new EducationOrganizationEntity(typeof(EducationOrganizationNetwork));
        public static readonly EducationOrganizationEntity EducationOrganizationNetworkAssociation = new EducationOrganizationEntity(typeof(EducationOrganizationNetworkAssociation));
    }
}
