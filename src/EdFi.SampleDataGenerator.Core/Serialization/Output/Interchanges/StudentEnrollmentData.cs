using System.Collections.Generic;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common.Interchanges;
using EdFi.SampleDataGenerator.Core.Entities;

namespace EdFi.SampleDataGenerator.Core.Serialization.Output.Interchanges
{
    [InterchangeOutput(Interchange.StudentEnrollmentInterchangeName, typeof(InterchangeStudentEnrollment), typeof(object))]
    public partial class StudentEnrollmentData
    {
        public StudentSchoolAssociation StudentSchoolAssociation { get; set; } = new StudentSchoolAssociation();
        public List<StudentEducationOrganizationAssociation> StudentEducationOrganizationAssociation { get; set; } = new List<StudentEducationOrganizationAssociation>();
        public List<StudentSectionAssociation> StudentSectionAssociations { get; set; } = new List<StudentSectionAssociation>();
        public List<GraduationPlan> GraduationPlans { get; set; } = new List<GraduationPlan>();
    }
}