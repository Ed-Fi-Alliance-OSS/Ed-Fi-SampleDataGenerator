namespace EdFi.SampleDataGenerator.Core.Config
{
    public interface IDataFileConfig
    {
        ISurnameFileMapping[] SurnameFiles { get; }
        IFirstNameFileMapping[] FirstNameFiles { get; }
        IStreetNameFileMapping StreetNameFile { get; }
        IDescriptorFileMapping[] DescriptorFiles { get; }
        IInterchangeEntityFileMapping[] StandardsFiles { get; }
        IInterchangeEntityFileMapping[] EducationOrganizationFiles { get; }
        IInterchangeEntityFileMapping[] EducationOrgCalendarFiles { get; }
        IInterchangeEntityFileMapping[] MasterScheduleFiles { get; }
        IInterchangeEntityFileMapping[] AssessmentMetadataFiles { get; }
    }
}