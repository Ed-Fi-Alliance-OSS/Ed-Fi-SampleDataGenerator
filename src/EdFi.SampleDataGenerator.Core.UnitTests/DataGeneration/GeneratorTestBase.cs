using System;
using System.Collections.Generic;
using System.Linq;
using EdFi.SampleDataGenerator.Core.Config;
using EdFi.SampleDataGenerator.Core.Config.DataFiles;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common;
using EdFi.SampleDataGenerator.Core.DataGeneration.Generators;
using EdFi.SampleDataGenerator.Core.Entities;
using EdFi.SampleDataGenerator.Core.Helpers;
using EdFi.SampleDataGenerator.Core.Serialization.Output;
using EdFi.SampleDataGenerator.Core.Serialization.Output.Interchanges;
using EdFi.SampleDataGenerator.Core.UnitTests.Config;

namespace EdFi.SampleDataGenerator.Core.UnitTests.DataGeneration
{
    public abstract class GeneratorTestBase
    {
        private static readonly RandomNumberGenerator _randomNumberGenerator = new RandomNumberGenerator();

        public static StudentDataGeneratorConfig GetStudentGeneratorConfig(GlobalData globalData, ISampleDataGeneratorConfig sampleDataGeneratorConfig, GlobalDataGeneratorConfig globalDataGeneratorConfig)
        {
            return new StudentDataGeneratorConfig
            {
                GlobalData = globalData,
                GlobalConfig = sampleDataGeneratorConfig,
                StudentProfile = TestStudentProfile.Default,
                DistrictProfile = TestDistrictProfile.Default,
                NameFileData = globalDataGeneratorConfig.NameFileData,
                GradeProfile = GetTestGradeProfile(),
                SchoolProfile = GetTestSchoolProfile(),
                EducationOrgCalendarData = globalDataGeneratorConfig.EducationOrgCalendarData,
                AssessmentMetadataData = globalDataGeneratorConfig.AssessmentMetadataData,
                EducationOrganizationData = globalDataGeneratorConfig.EducationOrganizationData,
                MasterScheduleData = globalDataGeneratorConfig.MasterScheduleData,
                CourseLookupCache = globalDataGeneratorConfig.CourseLookupCache
            };
        }

        private static GlobalDataGeneratorConfig ReadGlobalDataGeneratorConfig(TestSampleDataGeneratorConfig sampleDataGeneratorConfig)
        {
            var schoolProfiles = sampleDataGeneratorConfig.DistrictProfiles.SelectMany(dp => dp.SchoolProfiles);

            var configReader = new GlobalDataGeneratorConfigReader();

            var config = configReader.Read(sampleDataGeneratorConfig);
            config.GraduationPlans = schoolProfiles.SelectMany(sp => 
                sp.GradeProfiles.Select(gp => new GraduationPlan
                {
                    EducationOrganizationReference = sp.GetEducationOrganizationReference(),
                    GraduationPlanType = GraduationPlanTypeDescriptor.Standard.GetStructuredCodeValue(),
                    GraduationSchoolYear = gp.GetGraduationYear(sp, config.GlobalConfig.TimeConfig.SchoolCalendarConfig.SchoolYear())
                })
            )
            .ToList();

            return config;
        }

        public static GlobalDataGeneratorConfig GetGlobalDataGeneratorConfig()
        {
            return ReadGlobalDataGeneratorConfig(GetSampleDataGeneratorConfig());
        }

        public static GlobalDataGeneratorConfig GetGlobalDataGeneratorConfig(TestSampleDataGeneratorConfig sampleDataGeneratorConfig)
        {
            var config = ReadGlobalDataGeneratorConfig(sampleDataGeneratorConfig);
            return config;
        }

        public static GlobalDataGeneratorContext GetGlobalDataGeneratorContext(GlobalDataGeneratorConfig globalDataGeneratorConfig)
        {
            return new GlobalDataGeneratorContext
            {
                GlobalData = new GlobalData
                {
                    Descriptors = globalDataGeneratorConfig.DescriptorFiles.ToList(),
                    StandardsData = globalDataGeneratorConfig.StandardsFileData,
                    EducationOrganizationData = globalDataGeneratorConfig.EducationOrganizationData,
                    EducationOrgCalendarData = globalDataGeneratorConfig.EducationOrgCalendarData,
                    MasterScheduleData = globalDataGeneratorConfig.MasterScheduleData,
                    StaffAssociationData = GetStaffAssociationData(),
                    GraduationPlans = globalDataGeneratorConfig.GraduationPlans,
                    AssessmentMetadata = globalDataGeneratorConfig.AssessmentMetadataData,
                    CohortData = GetCohortData(),
                    GradebookEntries = GetGradebookEntries(globalDataGeneratorConfig)
                }
            };
        }

        private static List<GradebookEntry> GetGradebookEntries(GlobalDataGeneratorConfig globalDataGeneratorConfig)
        {
            var result = new List<GradebookEntry>
            {
                new GradebookEntry
                {
                    DateAssigned = TestSchoolCalendarConfig.Default.StartDate,
                    GradebookEntryType = GradebookEntryTypeDescriptor.Activity.CodeValue,
                    Description = "Test activity"
                }
            };
            return result;
        } 

        private static CohortData GetCohortData()
        {
            var academicInterventionCohort = new Cohort { CohortIdentifier = "Test Cohort", CohortScope = CohortScopeDescriptor.School.GetStructuredCodeValue(), CohortType = CohortTypeDescriptor.AcademicIntervention.GetStructuredCodeValue() };

            return new CohortData
            {
                Cohorts = new List<Cohort>
                {
                    academicInterventionCohort
                },
                StaffCohortAssociations = new List<StaffCohortAssociation>
                {
                    new StaffCohortAssociation
                    {
                        BeginDate = TestTimeConfig.Default.SchoolCalendarConfig.StartDate,
                        CohortReference = academicInterventionCohort.GetCohortReference(),
                        StaffReference = null
                    }
                }
            };
        }

        private static StaffAssociationData GetStaffAssociationData()
        {
            var staff = new List<Staff>()
            {
                GetStaffMember()
            };

            return new StaffAssociationData
            {
                StaffLeave = new List<StaffLeave>(),
                Staff = staff,
                StaffSchoolAssociation = staff.Select(s => new StaffSchoolAssociation { StaffReference = s.GetStaffReference(), SchoolReference = TestSchoolProfile.Default.GetSchoolReference()}).ToList(),
                StaffEducationOrganizationAssignmentAssociation = new List<StaffEducationOrganizationAssignmentAssociation>(),
                StaffEducationOrganizationEmploymentAssociation = new List<StaffEducationOrganizationEmploymentAssociation>(),
                StaffProgramAssociation = new List<StaffProgramAssociation>(),
                StaffSectionAssociation = new List<StaffSectionAssociation>()
            };
        }

        private static Staff GetStaffMember()
        {
            return new Staff
            {
                StaffUniqueId = Guid.NewGuid().ToString(),
                Name = new Name {FirstName = "Testy", MiddleName = "Test", LastSurname = "McTesterson"},
                HispanicLatinoEthnicity = false,
                Race = new[] {DescriptorHelpers.ToStructuredCodeValueArray<RaceDescriptor>().GetRandomItem(_randomNumberGenerator)},
                Sex = DescriptorHelpers.ToStructuredCodeValueArray<SexDescriptor>().GetRandomItem(_randomNumberGenerator),
            };
        }

        public static TestSampleDataGeneratorConfig GetSampleDataGeneratorConfig()
        {
            var config = new TestSampleDataGeneratorConfig
            {
                BatchSize = null,
                DataFilePath = ".\\DataFiles",
                OutputPath = ".\\",
                SeedFilePath = null,
                OutputMode = OutputMode.Standard,
                TimeConfig = TestTimeConfig.Default,
                CreatePerformanceFile = true,
                DistrictProfiles = new IDistrictProfile[]
                {
                    TestDistrictProfile.Default, 
                },
                StudentProfiles = new IStudentProfile[]
                {
                    TestStudentProfile.Default
                },
                EthnicityMappings = TestEthnicityMapping.Defaults,
                GenderMappings = TestGenderMapping.Defaults,
                StudentPopulationProfiles = new IStudentPopulationProfile[]
                {
                    TestStudentPopulationProfile.Default
                },
                GraduationPlanTemplates = new IGraduationPlanTemplate[]
                {
                    TestGraduationPlanTemplate.Default
                },
                MutatorConfig = TestMutatorConfigurationCollection.Default,
                StudentGradeRanges = TestStudentGradeRange.Defaults
            };

            var testDataFileConfigProvider = new TestDataFileConfigProvider();
            testDataFileConfigProvider.PopulateDataFileConfig(config);

            return config;
        }

        public static TestSchoolProfile GetTestSchoolProfile()
        {
            return TestSchoolProfile.Default;
        }

        public static TestGradeProfile GetTestGradeProfile()
        {
            return TestGradeProfile.Default;
        }
        

        public static NameFileData GetNameFileData()
        {
            var firstNameFiles = new NameFileCollection<IEthnicityMapping, SexDescriptor, FirstNameFile>()
            {
                [TestEthnicityMapping.Defaults.MappingFor(RaceDescriptor.White, false), SexDescriptor.Female] = new FirstNameFile
                {
                    FilePath = "",
                    Ethnicity =  TestEthnicityMapping.Defaults.MappingFor(RaceDescriptor.White, false),
                    SexDescriptor = SexDescriptor.Female,
                    FileRecords = new[]
                    {
                        new NameFileRecord {Frequency = 0, Name = "Test First Name"}
                    }
                }
            };

            var surnameFiles = new NameFileCollection<IEthnicityMapping, SurnameFile>()
            {
                [TestEthnicityMapping.Defaults.MappingFor(RaceDescriptor.White, false)] = new SurnameFile
                {
                    FilePath = "",
                    Ethnicity = TestEthnicityMapping.Defaults.MappingFor(RaceDescriptor.White, false),
                    FileRecords = new[]
                    {
                        new NameFileRecord {Frequency = 0, Name = "Test Surname"}
                    }
                }
            };

            var streetNameFile = new StreetNameFile
            {
                FilePath = "",
                FileRecords = new[] { new NameFileRecord { Frequency = 0, Name = "Test Street" } }
            };

            return new NameFileData
            {
                FirstNameFiles = firstNameFiles,
                SurnameFiles = surnameFiles,
                StreetNameFile = streetNameFile
            };
        }
    }
}
