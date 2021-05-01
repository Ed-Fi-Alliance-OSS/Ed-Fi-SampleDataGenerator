using EdFi.SampleDataGenerator.Core.Entities;

namespace EdFi.SampleDataGenerator.Core.Config.DataFiles
{
    public class NameFileData
    {
        public NameFileCollection<IEthnicityMapping, SexDescriptor, FirstNameFile> FirstNameFiles { get; set; } = new NameFileCollection<IEthnicityMapping, SexDescriptor, FirstNameFile>();
        public NameFileCollection<IEthnicityMapping, SurnameFile> SurnameFiles { get; set; } = new NameFileCollection<IEthnicityMapping, SurnameFile>();

        public StreetNameFile StreetNameFile { get; set; }
    }
}