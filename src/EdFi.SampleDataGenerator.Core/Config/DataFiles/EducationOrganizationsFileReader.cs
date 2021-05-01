using System;
using EdFi.SampleDataGenerator.Core.Entities;
using EdFi.SampleDataGenerator.Core.Serialization.Output.Interchanges;

namespace EdFi.SampleDataGenerator.Core.Config.DataFiles
{
    public class EducationOrganizationsFileReader : InterchangeFileReaderBase<EducationOrganizationData>
    {
        public override EducationOrganizationData Read(ISampleDataGeneratorConfig config)
        {
            var data = new EducationOrganizationData
            {
                StateEducationAgencies = ReadEntityFile<StateEducationAgency>(config),
                EducationServiceCenters = ReadEntityFile<EducationServiceCenter>(config),
                FeederSchoolAssociations = ReadEntityFile<FeederSchoolAssociation>(config),
                LocalEducationAgencies = ReadEntityFile<LocalEducationAgency>(config),
                Schools = ReadEntityFile<School>(config),
                Locations = ReadEntityFile<Location>(config),
                ClassPeriods = ReadEntityFile<ClassPeriod>(config),
                Courses = ReadEntityFile<Course>(config),
                Programs = ReadEntityFile<Program>(config),
                AccountabilityRatings = ReadEntityFile<AccountabilityRating>(config),
                EducationOrganizationPeerAssociations = ReadEntityFile<EducationOrganizationPeerAssociation>(config),
                EducationOrganizationNetworks = ReadEntityFile<EducationOrganizationNetwork>(config),
                EducationOrganizationNetworkAssociations = ReadEntityFile<EducationOrganizationNetworkAssociation>(config)
            };

            return data;
        }

        protected override Func<ISampleDataGeneratorConfig, IInterchangeEntityFileMapping[]> GetFileMappingsFunc => config => config.DataFileConfig.EducationOrganizationFiles;
    }

    public class EducationOrgCalendarFileReader : InterchangeFileReaderBase<EducationOrgCalendarData>
    {
        public override EducationOrgCalendarData Read(ISampleDataGeneratorConfig config)
        {
            var data = new EducationOrgCalendarData
            {
                Calendar = ReadEntityFile<Calendar>(config),
                CalendarDates = ReadEntityFile<CalendarDate>(config),
                GradingPeriods = ReadEntityFile<GradingPeriod>(config),
                Sessions = ReadEntityFile<Session>(config)
            };

            return data;
        }

        protected override Func<ISampleDataGeneratorConfig, IInterchangeEntityFileMapping[]> GetFileMappingsFunc => config => config.DataFileConfig.EducationOrgCalendarFiles;
    }
}