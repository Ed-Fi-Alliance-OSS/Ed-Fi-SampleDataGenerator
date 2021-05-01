using System.Collections.Generic;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common.Interchanges;
using EdFi.SampleDataGenerator.Core.Entities;
using EdFi.SampleDataGenerator.Core.Serialization.Output.Interchanges;

namespace EdFi.SampleDataGenerator.Core.Serialization.Output
{
    public partial class GlobalData
    {
        public List<DescriptorData> Descriptors { get; set; } = new List<DescriptorData>();
        public StandardsData StandardsData { get; set; } = new StandardsData();
        public EducationOrganizationData EducationOrganizationData { get; set; } = new EducationOrganizationData();
        public EducationOrgCalendarData EducationOrgCalendarData { get; set; } = new EducationOrgCalendarData();
        public MasterScheduleData MasterScheduleData { get; set; } = new MasterScheduleData();
        public StaffAssociationData StaffAssociationData { get; set; } = new StaffAssociationData();

        [InterchangeOutput(Interchange.StudentEnrollmentInterchangeName, typeof(InterchangeStudentEnrollment), typeof(object))]
        public List<GraduationPlan> GraduationPlans { get; set; } = new List<GraduationPlan>();
        public AssessmentMetadataData AssessmentMetadata { get; set; } = new AssessmentMetadataData();
        public CohortData CohortData { get; set; } = new CohortData();

        [InterchangeOutput(Interchange.StudentGradebookInterchangeName, typeof(InterchangeStudentGradebook), typeof(object))]
        public List<GradebookEntry> GradebookEntries { get; set; } = new List<GradebookEntry>();
    }
}
