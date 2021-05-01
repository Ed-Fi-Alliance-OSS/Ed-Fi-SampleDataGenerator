using System.Linq;
using EdFi.SampleDataGenerator.Core.Config;
using EdFi.SampleDataGenerator.Core.Config.DataFiles;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common;
using EdFi.SampleDataGenerator.Core.Entities;
using EdFi.SampleDataGenerator.Core.Helpers;
using EdFi.SampleDataGenerator.Core.Serialization.Output;
using EdFi.SampleDataGenerator.Core.Serialization.Output.Interchanges;

namespace EdFi.SampleDataGenerator.Core.DataGeneration.Generators
{
    public class StudentDataGeneratorConfig : IHasOutputMode
    {
        public GlobalData GlobalData { get; set; }
        public ISampleDataGeneratorConfig GlobalConfig { get; set; }
        public IStudentProfile StudentProfile { get; set; }
        public IGradeProfile GradeProfile { get; set; }
        public ISchoolProfile SchoolProfile { get; set; }
        public IDistrictProfile DistrictProfile { get; set; }
        public NameFileData NameFileData { get; set; }
        public EducationOrgCalendarData EducationOrgCalendarData { get; set; }
        public AssessmentMetadataData AssessmentMetadataData { get; set; }
        public EducationOrganizationData EducationOrganizationData { get; set; }
        public MasterScheduleData MasterScheduleData { get; set; }
        public CourseLookupCache CourseLookupCache { get; set; }

        public OutputMode OutputMode => GlobalConfig.OutputMode;

        public Program GetProgram(ProgramTypeDescriptor programType)
        {
            var educationOrganizationReference = GetEducationOrganizationReference();

            return GlobalData.EducationOrganizationData.Programs
                .FirstOrDefault(p =>
                    p.EducationOrganizationReference.ReferencesSameEducationOrganizationAs(educationOrganizationReference)
                    && p.ProgramType == programType.GetStructuredCodeValue());
        }

        public EducationOrganizationReferenceType GetEducationOrganizationReference()
        {
            var schools = GlobalData.EducationOrganizationData.Schools;
            var localEducationAgencies = GlobalData.EducationOrganizationData.LocalEducationAgencies;

            return SchoolProfile.GetLocalEducationAgency(schools, localEducationAgencies)
                .GetEducationOrganizationReference();
        }
    }
}