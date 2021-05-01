using System.Linq;
using EdFi.SampleDataGenerator.Core.Config;
using EdFi.SampleDataGenerator.Core.Entities;
using EdFi.SampleDataGenerator.Core.Helpers;

namespace EdFi.SampleDataGenerator.Core.DataGeneration.Generators.StaffAssociation
{
    public class StaffRequirement
    {
        public bool HighlyQualified { get; set; }
        public SectionReferenceType[] SectionReference { get; set; }
        public StaffReferenceType StaffReference { get; set; }
        public bool SpeaksSpanish { get; set; }
        public int EducationOrganizationId { get; set; }
        public string EducationOrganizationName { get; set; }
        public GradeLevelDescriptor[] GradeLevel { get; set; }
        public ProgramAssignmentDescriptor ProgramAssignment { get; set; }
        public AcademicSubjectDescriptor[] Subjects { get; set; }
        public StaffClassificationDescriptor StaffClassification { get; set; }
        public bool IsSchoolAdministrator => SchoolAdministrativePositions.Any(position => StaffClassification == position);
        public bool IsLeaAdministrator => LeaAdministrativePositions.Any(position => StaffClassification == position);

        public IStaffProfile GetStaffProfile(GlobalDataGeneratorConfig config)
        {
            return config.SchoolProfilesById[EducationOrganizationId].StaffProfile;
        }

        public static readonly StaffClassificationDescriptor[] LeaAdministrativePositions = 
        {
            StaffClassificationDescriptor.Superintendent,
            StaffClassificationDescriptor.LEAAdministrator
        };

        public static readonly StaffClassificationDescriptor[] SchoolAdministrativePositions =
        {
            StaffClassificationDescriptor.Principal,
            StaffClassificationDescriptor.AssistantPrincipal
        };
    }
}
