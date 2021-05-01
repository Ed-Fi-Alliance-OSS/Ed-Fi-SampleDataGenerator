using EdFi.SampleDataGenerator.Core.Config;

namespace EdFi.SampleDataGenerator.Core.UnitTests.Config
{
    public class TestDataFileConfig : IDataFileConfig
    {
        public ISurnameFileMapping[] SurnameFiles { get; set; }
        public IFirstNameFileMapping[] FirstNameFiles { get; set; }
        public IStreetNameFileMapping StreetNameFile { get; set; }
        public IDescriptorFileMapping[] DescriptorFiles { get; set; }
        public IInterchangeEntityFileMapping[] StandardsFiles { get; set; }
        public IInterchangeEntityFileMapping[] EducationOrganizationFiles { get; set; }
        public IInterchangeEntityFileMapping[] EducationOrgCalendarFiles { get; set; }
        public IInterchangeEntityFileMapping[] MasterScheduleFiles { get; set; }
        public IInterchangeEntityFileMapping[] AssessmentMetadataFiles { get; set; }
    }
}