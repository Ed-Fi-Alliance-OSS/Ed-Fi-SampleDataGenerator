using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using EdFi.SampleDataGenerator.Core.Config.DataFiles;
using EdFi.SampleDataGenerator.Core.Entities;
using EdFi.SampleDataGenerator.Core.Helpers;

namespace EdFi.SampleDataGenerator.Console.Config
{
    public class DataFileScanner
    {
        private readonly IDataFilePathResolver _nameFilePathResolver;

        public DataFileScanner() : this(new DataFilePathResolver())
        {
        }

        public DataFileScanner(IDataFilePathResolver nameFilePathResolver)
        {
            _nameFilePathResolver = nameFilePathResolver;
        }

        public void ScanForDataFiles(SampleDataGeneratorConfig config)
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
            
            config.DataFileConfig.StreetNameFile = GetStreetNameFile(config);
            config.DataFileConfig.FirstNameFiles = GetFirstNameFiles(config).ToArray();
            config.DataFileConfig.SurnameFiles = GetSurnameFiles(config).ToArray();
            config.DataFileConfig.DescriptorFiles = GetDescriptorFiles(config).ToArray();
            config.DataFileConfig.StandardsFiles = GetInterchangeEntityFileMappings(typeof(InterchangeStandards)).ToArray();
            config.DataFileConfig.EducationOrganizationFiles = GetInterchangeEntityFileMappings(typeof (InterchangeEducationOrganization)).ToArray();
            config.DataFileConfig.EducationOrgCalendarFiles = GetInterchangeEntityFileMappings(typeof (InterchangeEducationOrgCalendar)).ToArray();
            config.DataFileConfig.MasterScheduleFiles = GetInterchangeEntityFileMappings(typeof (InterchangeMasterSchedule)).ToArray();
            config.DataFileConfig.AssessmentMetadataFiles = GetInterchangeEntityFileMappings(typeof (InterchangeAssessmentMetadata)).ToArray();
        }

        private StreetNameFileMapping GetStreetNameFile(SampleDataGeneratorConfig config)
        {
            var fileName = _nameFilePathResolver.GetPathForStreeNameFile();
            
            return File.Exists(fileName)
                ? new StreetNameFileMapping {FilePath = fileName} 
                : null;
        }

        private IEnumerable<FirstNameFileMapping> GetFirstNameFiles(SampleDataGeneratorConfig config)
        {
            return 
                from genderMapping in config.GenderMappings
                from ethnicityMapping in config.EthnicityMappings
                let filePath = _nameFilePathResolver.GetPathForFirstNameFile(ethnicityMapping.Ethnicity, genderMapping.Gender)
                where File.Exists(filePath)
                select new FirstNameFileMapping
            {
                Ethnicity = ethnicityMapping.Ethnicity,
                FilePath = filePath,
                Gender = genderMapping.Gender
            };
        }

        private IEnumerable<SurnameFileMapping> GetSurnameFiles(SampleDataGeneratorConfig config)
        {
            return
                from ethnicityMapping in config.EthnicityMappings
                let filePath = _nameFilePathResolver.GetPathForSurnameFile(ethnicityMapping.Ethnicity)
                where File.Exists(filePath)
                select new SurnameFileMapping
                {
                    Ethnicity = ethnicityMapping.Ethnicity,
                    FilePath = filePath
                };
        }

        private IEnumerable<DescriptorFileMapping> GetDescriptorFiles(SampleDataGeneratorConfig config)
        {
            var descriptorFilePath = _nameFilePathResolver.GetPathForInterchangeType(typeof (InterchangeDescriptors));
            var descriptorFiles = Directory.EnumerateFiles(descriptorFilePath, "*.csv", SearchOption.TopDirectoryOnly);

            return descriptorFiles.Select(filePath => new DescriptorFileMapping
            {
                DescriptorName = DescriptorHelpers.DescriptorNameFromFilePath(filePath),
                FilePath = filePath
            });
        }
        
        private IEnumerable<InterchangeEntityFileMapping> GetInterchangeEntityFileMappings(Type interchangeType)
        {
            var entityTypes = InterchangeTypeHelpers.GetEntityTypesForInterchange(interchangeType);
            return from entityType in entityTypes
                let filePath = _nameFilePathResolver.GetPathForInterchangeEntityTypeFile(interchangeType, entityType)
                where File.Exists(filePath)
                select new InterchangeEntityFileMapping
                {
                    FilePath = filePath,
                    EntityType = entityType,
                    InterchangeType = interchangeType
                };
        }
    }
}
