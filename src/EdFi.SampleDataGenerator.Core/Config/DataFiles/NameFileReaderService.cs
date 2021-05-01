using System.Linq;

namespace EdFi.SampleDataGenerator.Core.Config.DataFiles
{
    public class NameFileReaderService : INameFileReaderService
    {
        private readonly INameFileReader _nameFileReader;

        public NameFileReaderService() : this(new NameFileReader())
        {
        }

        public NameFileReaderService(INameFileReader nameFileReader)
        {
            _nameFileReader = nameFileReader;
        }

        public NameFileData Read(ISampleDataGeneratorConfig config)
        {
            var firstNameFiles = config.DataFileConfig.FirstNameFiles.Select(fileMapping => ReadFirstNameFile(config, fileMapping)).ToList();
            var surnameFiles = config.DataFileConfig.SurnameFiles.Select(fileMapping => ReadSurnameFile(config, fileMapping)).ToList();

            var result = new NameFileData
            {
                StreetNameFile = ReadStreetNameFile(config),
                FirstNameFiles = firstNameFiles.ToNameFileCollection(k => k.Ethnicity, k => k.SexDescriptor),
                SurnameFiles = surnameFiles.ToNameFileCollection(k => k.Ethnicity)
            };

            return result;
        }

        public SurnameFile ReadSurnameFile(ISampleDataGeneratorConfig config, ISurnameFileMapping fileMapping)
        {
            var fileRecords = _nameFileReader.Read(fileMapping.FilePath);

            return new SurnameFile
            {
                FilePath = fileMapping.FilePath,
                FileRecords = fileRecords.ToArray(),
                Ethnicity = config.EthnicityMappings.MappingFor(fileMapping.Ethnicity)
            };
        }

        public FirstNameFile ReadFirstNameFile(ISampleDataGeneratorConfig config, IFirstNameFileMapping fileMapping)
        {
            var fileRecords = _nameFileReader.Read(fileMapping.FilePath);

            return new FirstNameFile
            {
                FilePath = fileMapping.FilePath,
                FileRecords = fileRecords.ToArray(),
                Ethnicity = config.EthnicityMappings.MappingFor(fileMapping.Ethnicity),
                SexDescriptor = config.GenderMappings.SexDescriptorFor(fileMapping.Gender)
            };
        }

        public StreetNameFile ReadStreetNameFile(ISampleDataGeneratorConfig config)
        {
            var streetFileMappingPath = config.DataFileConfig.StreetNameFile.FilePath;
            var fileRecords = _nameFileReader.Read(streetFileMappingPath);

            return new StreetNameFile
            {
                FilePath = streetFileMappingPath,
                FileRecords = fileRecords.ToArray()
            };
        }
    }
}