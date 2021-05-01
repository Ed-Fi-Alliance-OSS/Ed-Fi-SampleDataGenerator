using System;
using System.Linq;
using EdFi.SampleDataGenerator.Core.Config.DataFiles;
using EdFi.SampleDataGenerator.Core.Config.Xml;
using EdFi.SampleDataGenerator.Core.Entities;
using EdFi.SampleDataGenerator.Core.Serialization.CsvHelper;

namespace EdFi.EducationOrganizationGenerator.Console.Configuration
{
    public static class EducationOrganizationGeneratorConfigReader
    {
        public static EducationOrganizationGeneratorConfig Read(CommandLineOptions commandLineOptions)
        {
            return new EducationOrganizationGeneratorConfig
            {
                CourseTemplates = MappedCsvFileReader.ReadEntityFile<Course>(commandLineOptions.CourseTemplateFilePath),
                DistrictProfile = LoadDistrictConfig(commandLineOptions),
                SchoolBaseNames = SchoolNameFileReader.Read(commandLineOptions.SchoolNamesFilePath),
                StreetNameFile = LoadStreetNameFile(commandLineOptions.StreetNamesFilePath)
            };
        }

        private static DistrictProfile LoadDistrictConfig(CommandLineOptions commandLineOptions)
        {
            try
            {
                var config = XmlConfigHelpers.ParseConfigFileToObject<DistrictProfile>(commandLineOptions.DistrictConfigFilePath);
                return config;
            }
            catch (Exception e)
            {
                throw new Exception("Error when trying to read config file", e);
            }
        }

        private static StreetNameFile LoadStreetNameFile(string filePath)
        {
            return new StreetNameFile
            {
                FilePath = filePath,
                FileRecords = NameFileReader.ReadFileRecords(filePath).ToArray()
            };
        }
    }
}
