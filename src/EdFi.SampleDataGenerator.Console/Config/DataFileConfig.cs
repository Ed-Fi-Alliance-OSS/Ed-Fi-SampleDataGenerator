using System.Xml.Serialization;
using EdFi.SampleDataGenerator.Core.Config;

namespace EdFi.SampleDataGenerator.Console.Config
{
    public class DataFileConfig : IDataFileConfig
    {
        [XmlElement("SurnameFile")]
        public SurnameFileMapping[] SurnameFiles { get; set; }
        ISurnameFileMapping[] IDataFileConfig.SurnameFiles => SurnameFiles;

        [XmlElement("FirstNameFile")]
        public FirstNameFileMapping[] FirstNameFiles { get; set; }
        IFirstNameFileMapping[] IDataFileConfig.FirstNameFiles => FirstNameFiles;

        [XmlElement("StreetNameFile")]
        public StreetNameFileMapping StreetNameFile { get; set; }
        IStreetNameFileMapping IDataFileConfig.StreetNameFile => StreetNameFile;

        [XmlElement("DescriptorFile")]
        public DescriptorFileMapping[] DescriptorFiles { get; set; }
        IDescriptorFileMapping[] IDataFileConfig.DescriptorFiles => DescriptorFiles;

        [XmlElement("StandardsFile")]
        public InterchangeEntityFileMapping[] StandardsFiles { get; set; }
        IInterchangeEntityFileMapping[] IDataFileConfig.StandardsFiles => StandardsFiles;

        [XmlElement("EducationOrganizationsFile")]
        public InterchangeEntityFileMapping[] EducationOrganizationFiles { get; set; }
        IInterchangeEntityFileMapping[] IDataFileConfig.EducationOrganizationFiles => EducationOrganizationFiles;

        [XmlElement("EducationOrgCalendarFile")]
        public InterchangeEntityFileMapping[] EducationOrgCalendarFiles { get; set; }
        IInterchangeEntityFileMapping[] IDataFileConfig.EducationOrgCalendarFiles => EducationOrgCalendarFiles;

        [XmlElement("MasterScheduleFile")]
        public InterchangeEntityFileMapping[] MasterScheduleFiles { get; set; }
        IInterchangeEntityFileMapping[] IDataFileConfig.MasterScheduleFiles => MasterScheduleFiles;

        [XmlElement("AssessmentMetadataFile")]
        public InterchangeEntityFileMapping[] AssessmentMetadataFiles { get; set; }
        IInterchangeEntityFileMapping[] IDataFileConfig.AssessmentMetadataFiles => AssessmentMetadataFiles;
    }
}
