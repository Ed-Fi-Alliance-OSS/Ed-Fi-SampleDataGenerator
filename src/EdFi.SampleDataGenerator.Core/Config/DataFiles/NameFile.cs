using EdFi.SampleDataGenerator.Core.Entities;

namespace EdFi.SampleDataGenerator.Core.Config.DataFiles
{
    public abstract class NameFile
    {
        public string FilePath { get; set; }
        public NameFileRecord[] FileRecords { get; set; }
    }

    public abstract class NameFileForEthnicity : NameFile
    {
        public IEthnicityMapping Ethnicity { get; set; }
    }

    public abstract class NameFileForEthnicityAndGender : NameFileForEthnicity
    {
        public SexDescriptor SexDescriptor { get; set; }
    }

    public class SurnameFile : NameFileForEthnicity
    {
    }

    public class FirstNameFile : NameFileForEthnicityAndGender
    {
    }

    public class StreetNameFile : NameFile
    {
    }
}
