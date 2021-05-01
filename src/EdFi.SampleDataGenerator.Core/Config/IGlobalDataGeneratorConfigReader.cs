using System.Linq;
using EdFi.SampleDataGenerator.Core.Config.DataFiles;
using EdFi.SampleDataGenerator.Core.Config.SeedData;
using EdFi.SampleDataGenerator.Core.DataGeneration.Generators;
using EdFi.SampleDataGenerator.Core.Helpers;
using EdFi.SampleDataGenerator.Core.Serialization.Output.Interchanges;
using log4net;

namespace EdFi.SampleDataGenerator.Core.Config
{
    public interface IGlobalDataGeneratorConfigReader
    {
        GlobalDataGeneratorConfig Read(ISampleDataGeneratorConfig config);
    }

    public class GlobalDataGeneratorConfigReader : IGlobalDataGeneratorConfigReader
    {
        private static ILog _log = LogManager.GetLogger(typeof(GlobalDataGeneratorConfigReader));

        private readonly INameFileReaderService _nameFileReaderService;
        private readonly IDescriptorFileReaderService _descriptorFileReaderService;
        private readonly IInterchangeFileReader<StandardsData> _standardsFileReader;
        private readonly IInterchangeFileReader<EducationOrganizationData> _educationOrganizationFileReader;
        private readonly IInterchangeFileReader<EducationOrgCalendarData> _educationOrgCalendarDataFileReader;
        private readonly IInterchangeFileReader<MasterScheduleData> _masterScheduleDataFileReader;
        private readonly IInterchangeFileReader<AssessmentMetadataData> _assessmentMetadataFileReader;
        private readonly ISeedDataSerializationService _seedDataSerializationService;


        public GlobalDataGeneratorConfigReader() : this(new NameFileReaderService(), new DescriptorFileReaderService(),
            new StandardsFileReader(), new EducationOrganizationsFileReader(), new EducationOrgCalendarFileReader(), new MasterScheduleFileReader(),
            new AssessmentMetadataFileReader(), new SeedDataSerializationService())
        {
        }

        public GlobalDataGeneratorConfigReader(INameFileReaderService nameFileReaderService, IDescriptorFileReaderService descriptorFileReaderService,
            IInterchangeFileReader<StandardsData> standardsFileReader, IInterchangeFileReader<EducationOrganizationData> educationOrganizationFileReader,
            IInterchangeFileReader<EducationOrgCalendarData> educationOrgCalendarDataFileReader, IInterchangeFileReader<MasterScheduleData> masterScheduleDataFileReader,
            IInterchangeFileReader<AssessmentMetadataData> assessmentMetadataFileReader, ISeedDataSerializationService seedDataSerializationService)
        {
            _nameFileReaderService = nameFileReaderService;
            _descriptorFileReaderService = descriptorFileReaderService;
            _standardsFileReader = standardsFileReader;
            _educationOrganizationFileReader = educationOrganizationFileReader;
            _educationOrgCalendarDataFileReader = educationOrgCalendarDataFileReader;
            _masterScheduleDataFileReader = masterScheduleDataFileReader;
            _assessmentMetadataFileReader = assessmentMetadataFileReader;
            _seedDataSerializationService = seedDataSerializationService;
        }

        public GlobalDataGeneratorConfig Read(ISampleDataGeneratorConfig config)
        {
            _log.Info("Parsing input files");
            var nameFileData = _nameFileReaderService.Read(config);
            var descriptorData = _descriptorFileReaderService.Read(config);
            var standardsData = _standardsFileReader.Read(config);
            var educationOrgData = _educationOrganizationFileReader.Read(config);
            var educationOrgCalendarData = _educationOrgCalendarDataFileReader.Read(config);
            var masterScheduleData = _masterScheduleDataFileReader.Read(config);
            var assessmentMetadata = _assessmentMetadataFileReader.Read(config);
            var seedRecords = _seedDataSerializationService.Read(config);

            return new GlobalDataGeneratorConfig
            {
                GlobalConfig = config,
                NameFileData = nameFileData,
                DescriptorFiles = descriptorData,
                StandardsFileData = standardsData,
                EducationOrganizationData = educationOrgData,
                EducationOrgCalendarData = educationOrgCalendarData,
                MasterScheduleData = masterScheduleData,
                AssessmentMetadataData = assessmentMetadata,
                SchoolProfilesById = config.DistrictProfiles
                    .SelectMany(x => x.SchoolProfiles)
                    .ToDictionary(x => x.SchoolId, x => x),
                CourseLookupCache = new CourseLookupCache(masterScheduleData, educationOrgData),
                SeedRecords = seedRecords,
            };
        }
    }
}
