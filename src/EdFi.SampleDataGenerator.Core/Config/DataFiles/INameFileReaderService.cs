namespace EdFi.SampleDataGenerator.Core.Config.DataFiles
{
    public interface INameFileReaderService
    {
        NameFileData Read(ISampleDataGeneratorConfig config);
        SurnameFile ReadSurnameFile(ISampleDataGeneratorConfig config, ISurnameFileMapping fileMapping);
        FirstNameFile ReadFirstNameFile(ISampleDataGeneratorConfig config, IFirstNameFileMapping fileMapping);
        StreetNameFile ReadStreetNameFile(ISampleDataGeneratorConfig config);
    }
}
