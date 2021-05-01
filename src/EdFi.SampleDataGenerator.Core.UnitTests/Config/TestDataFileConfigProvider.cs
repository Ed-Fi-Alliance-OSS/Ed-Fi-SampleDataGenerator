using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using EdFi.SampleDataGenerator.Core.Config;
using EdFi.SampleDataGenerator.Core.Config.DataFiles;
using EdFi.SampleDataGenerator.Core.Entities;
using EdFi.SampleDataGenerator.Core.Helpers;

namespace EdFi.SampleDataGenerator.Core.UnitTests.Config
{
    public class TestDataFileConfigProvider
    {
        private readonly IDataFilePathResolver _nameFilePathResolver;

        public TestDataFileConfigProvider() : this(new DataFilePathResolver())
        {
        }

        public TestDataFileConfigProvider(IDataFilePathResolver nameFilePathResolver)
        {
            _nameFilePathResolver = nameFilePathResolver;
        }

        public void PopulateDataFileConfig(TestSampleDataGeneratorConfig config)
        {
            _nameFilePathResolver.SetBasePath(config.DataFilePath);

            if (string.IsNullOrWhiteSpace(config.DataFilePath))
            {
                throw new ArgumentException("Config does not define a DataFilePath");
            }

            if (config.GenderMappings == null)
            {
                throw new ArgumentException("Config does not have any Gender Mappings defined");
            }

            if (config.EthnicityMappings == null)
            {
                throw new ArgumentException("Config does not have any Ethnicity Mappings defined");
            }

            config.DataFileConfig = new TestDataFileConfig
            {
                StreetNameFile = GetStreetNameFile(config),
                FirstNameFiles = GetFirstNameFiles(config).ToArray(),
                SurnameFiles = GetSurnameFiles(config).ToArray(),
                DescriptorFiles = GetDescriptorFiles(config).ToArray(),
                StandardsFiles = GetInterchangeEntityFileMappings(typeof(InterchangeStandards)).ToArray(),
                EducationOrganizationFiles = GetInterchangeEntityFileMappings(typeof(InterchangeEducationOrganization)).ToArray(),
                EducationOrgCalendarFiles = GetInterchangeEntityFileMappings(typeof(InterchangeEducationOrgCalendar)).ToArray(),
                MasterScheduleFiles = GetInterchangeEntityFileMappings(typeof(InterchangeMasterSchedule)).ToArray(),
                AssessmentMetadataFiles = GetInterchangeEntityFileMappings(typeof(InterchangeAssessmentMetadata)).ToArray(),
            };
        }

        private TestStreetNameFileMapping GetStreetNameFile(ISampleDataGeneratorConfig config)
        {
            var fileName = _nameFilePathResolver.GetPathForStreeNameFile();

            return File.Exists(fileName)
                ? new TestStreetNameFileMapping { FilePath = fileName }
                : null;
        }

        private IEnumerable<IFirstNameFileMapping> GetFirstNameFiles(ISampleDataGeneratorConfig config)
        {
            return
                from genderMapping in config.GenderMappings
                from ethnicityMapping in config.EthnicityMappings
                let filePath = _nameFilePathResolver.GetPathForFirstNameFile(ethnicityMapping.Ethnicity, genderMapping.Gender)
                where File.Exists(filePath)
                select new TestFirstNameFileMapping
                {
                    Ethnicity = ethnicityMapping.Ethnicity,
                    FilePath = filePath,
                    Gender = genderMapping.Gender
                };
        }

        private IEnumerable<ISurnameFileMapping> GetSurnameFiles(ISampleDataGeneratorConfig config)
        {
            return
                from ethnicityMapping in config.EthnicityMappings
                let filePath = _nameFilePathResolver.GetPathForSurnameFile(ethnicityMapping.Ethnicity)
                where File.Exists(filePath)
                select new TestSurnameFileMapping
                {
                    Ethnicity = ethnicityMapping.Ethnicity,
                    FilePath = filePath
                };
        }

        private IEnumerable<IDescriptorFileMapping> GetDescriptorFiles(ISampleDataGeneratorConfig config)
        {
            var descriptorFilePath = _nameFilePathResolver.GetPathForInterchangeType(typeof(InterchangeDescriptors));
            var descriptorFiles = Directory.EnumerateFiles(descriptorFilePath, "*.csv", SearchOption.TopDirectoryOnly);

            return descriptorFiles.Select(filePath => new TestDescriptorFileMapping
            {
                DescriptorName = DescriptorHelpers.DescriptorNameFromFilePath(filePath),
                FilePath = filePath
            });
        }

        private IEnumerable<IInterchangeEntityFileMapping> GetInterchangeEntityFileMappings(Type interchangeType)
        {
            var entityTypes = InterchangeTypeHelpers.GetEntityTypesForInterchange(interchangeType);
            return from entityType in entityTypes
                   let filePath = _nameFilePathResolver.GetPathForInterchangeEntityTypeFile(interchangeType, entityType)
                   where File.Exists(filePath)
                   select new TestInterchangeEntityFileMapping
                   {
                       FilePath = filePath,
                       EntityType = entityType,
                       InterchangeType = interchangeType
                   };
        }
    }
}
