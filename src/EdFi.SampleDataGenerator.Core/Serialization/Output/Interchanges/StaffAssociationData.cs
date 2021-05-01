using System.Collections.Generic;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common.Interchanges;
using EdFi.SampleDataGenerator.Core.DataGeneration.Generators.StaffAssociation;
using EdFi.SampleDataGenerator.Core.Entities;

namespace EdFi.SampleDataGenerator.Core.Serialization.Output.Interchanges
{
    [InterchangeOutput(Interchange.StaffAssociationInterchangeName, typeof(InterchangeStaffAssociation), typeof(ComplexObjectType))]
    public partial class StaffAssociationData
    {
        public List<Staff> Staff { get; set; } = new List<Staff>();
        public List<StaffEducationOrganizationEmploymentAssociation> StaffEducationOrganizationEmploymentAssociation { get; set; } = new List<StaffEducationOrganizationEmploymentAssociation>();
        public List<StaffEducationOrganizationAssignmentAssociation> StaffEducationOrganizationAssignmentAssociation { get; set; } = new List<StaffEducationOrganizationAssignmentAssociation>();
        public List<StaffSchoolAssociation> StaffSchoolAssociation { get; set; } = new List<StaffSchoolAssociation>();
        public List<StaffSectionAssociation> StaffSectionAssociation { get; set; } = new List<StaffSectionAssociation>();
        public List<StaffLeave> StaffLeave { get; set; } = new List<StaffLeave>();
        public List<StaffProgramAssociation> StaffProgramAssociation { get; set; } = new List<StaffProgramAssociation>();

        [DoNotOutputToInterchange]
        public List<StaffRequirement> StaffRequirements { get; set; } = new List<StaffRequirement>();
    }
}
