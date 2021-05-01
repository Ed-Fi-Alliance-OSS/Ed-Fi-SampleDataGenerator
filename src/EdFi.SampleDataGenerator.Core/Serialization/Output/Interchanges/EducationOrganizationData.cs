using System.Collections.Generic;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common.Interchanges;
using EdFi.SampleDataGenerator.Core.Entities;

namespace EdFi.SampleDataGenerator.Core.Serialization.Output.Interchanges
{
    [InterchangeOutput(Interchange.EducationOrganizationInterchangeName, typeof(InterchangeEducationOrganization), typeof(ComplexObjectType))]
    public partial class EducationOrganizationData
    {
        public List<StateEducationAgency> StateEducationAgencies { get; set; } = new List<StateEducationAgency>();
        public List<EducationServiceCenter> EducationServiceCenters { get; set; } = new List<EducationServiceCenter>();
        public List<FeederSchoolAssociation> FeederSchoolAssociations { get; set; } = new List<FeederSchoolAssociation>();
        public List<LocalEducationAgency> LocalEducationAgencies { get; set; } = new List<LocalEducationAgency>();
        public List<School> Schools { get; set; } = new List<School>();
        public List<Location> Locations { get; set; } = new List<Location>();
        public List<ClassPeriod> ClassPeriods { get; set; } = new List<ClassPeriod>();
        public List<Course> Courses { get; set; } = new List<Course>();
        public List<Program> Programs { get; set; } = new List<Program>();
        public List<AccountabilityRating> AccountabilityRatings { get; set; } = new List<AccountabilityRating>();
        public List<EducationOrganizationPeerAssociation> EducationOrganizationPeerAssociations { get; set; } = new List<EducationOrganizationPeerAssociation>();
        public List<EducationOrganizationNetwork> EducationOrganizationNetworks { get; set; } = new List<EducationOrganizationNetwork>();
        public List<EducationOrganizationNetworkAssociation> EducationOrganizationNetworkAssociations { get; set; } = new List<EducationOrganizationNetworkAssociation>();
    }
}