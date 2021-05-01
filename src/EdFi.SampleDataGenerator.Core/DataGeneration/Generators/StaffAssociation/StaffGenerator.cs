using System;
using System.Collections.Generic;
using System.Linq;
using EdFi.SampleDataGenerator.Core.Config;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common.Dependencies;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common.Entity;
using EdFi.SampleDataGenerator.Core.DataGeneration.InterchangeEntities;
using EdFi.SampleDataGenerator.Core.Entities;
using EdFi.SampleDataGenerator.Core.Helpers;

namespace EdFi.SampleDataGenerator.Core.DataGeneration.Generators.StaffAssociation
{
    public sealed class StaffGenerator : StaffAssociationEntityGenerator
    {
        private const int MinimumProfessionalWorkingAge = 18;
        private const int MinStaffAge = 25;
        private const int MinStaffAgeHighlyQualified = 33;
        private const int MaxStaffAge = 65;
        private const int MinYearsTeachingExperienceForHighlyQualifiedTeachers = 2;

        private const string LeaAdministratorLoginId = "sysadmin";
        private const string SuperintendentLoginId = "superintendent";

        private readonly Dictionary<PersonalInformationVerificationDescriptor, double> _primaryIdentificationTypes = new Dictionary<PersonalInformationVerificationDescriptor, double>
        {
            {PersonalInformationVerificationDescriptor.DriversLicense, 0.85},
            {PersonalInformationVerificationDescriptor.StateIssuedID, 0.05},
            {PersonalInformationVerificationDescriptor.BirthCertificate, 0.05},
            {PersonalInformationVerificationDescriptor.Passport, 0.05}
        };

        public StaffGenerator(IRandomNumberGenerator randomNumberGenerator)
            : base(randomNumberGenerator)
        {
        }

        public override IEntity GeneratesEntity => StaffAssociationEntity.Staff;
        public override IEntity[] DependsOnEntities => EntityDependencies.Create(StaffAssociationEntity.StaffRequirement);

        protected override void GenerateCore(GlobalDataGeneratorContext context)
        {
            foreach (var staffRequirement in context.GlobalData.StaffAssociationData.StaffRequirements )
            {
                var heritage = GetGeneratedStaffHeritage(context, staffRequirement);
                var staffUniqueId = staffRequirement.StaffReference.StaffIdentity.StaffUniqueId;
                var gender = staffRequirement.IsLeaAdministrator
                        ? GetRandomStaffGender()
                        : GetRandomStaffGender(staffRequirement.GetStaffProfile(Configuration));

                var highlyQualified = staffRequirement.HighlyQualified;
                var minimumAge = highlyQualified ? MinStaffAgeHighlyQualified : MinStaffAge;
                var birthday = RandomNumberGenerator.GetRandomBirthday(minimumAge, MaxStaffAge, Configuration.GlobalConfig.TimeConfig.SchoolCalendarConfig.StartDate);
                var experience = GenerateStaffExperience(birthday, highlyQualified);

                var ethnicityMapping = Configuration.GlobalConfig.EthnicityMappings.MappingFor(heritage.Races.FirstOrDefault(), heritage.HispanicLatinoEthnicity);

                var name = NameGenerator.Generate(Configuration.NameFileData, RandomNumberGenerator, gender, ethnicityMapping);
                name.PersonalIdentificationDocument = GetStaffIdentificationDocuments();
                var loginId = GenerateLoginId(name, staffRequirement);

                var newStaffMember = new Staff
                {
                    HispanicLatinoEthnicity = heritage.HispanicLatinoEthnicity,
                    Name = name,
                    StaffUniqueId = staffUniqueId,
                    StaffIdentificationCode = GetStaffIdentificationCode(staffUniqueId, staffRequirement),
                    HighlyQualifiedTeacher = highlyQualified,
                    HighlyQualifiedTeacherSpecified = true,
                    BirthDate = birthday,
                    BirthDateSpecified = true,
                    Race = heritage.Races.ToStructuredCodeValueArray(),
                    Sex = gender.GetStructuredCodeValue(),
                    YearsOfPriorTeachingExperience = experience.YearsOfTeachingExperience,
                    YearsOfPriorTeachingExperienceSpecified = true,
                    YearsOfPriorProfessionalExperience = experience.TotalYearsOfProfessionalExperience,
                    YearsOfPriorProfessionalExperienceSpecified = true,
                    HighestCompletedLevelOfEducation = GetLevelOfEducation(highlyQualified).GetStructuredCodeValue(),
                    ElectronicMail = GetEmail(loginId, staffRequirement.EducationOrganizationName),
                    LoginId = loginId,
                };

                if (heritage.OldEthnicityDescriptor != null)
                {
                    newStaffMember.OldEthnicity = heritage.OldEthnicityDescriptor.GetStructuredCodeValue();
                }

                context.GlobalData.StaffAssociationData.Staff.Add(newStaffMember);
            }
        }

        private static string GenerateLoginId(Name name, StaffRequirement requirement)
        {
            if (requirement.IsLeaAdministrator)
            {
                if (requirement.StaffClassification == StaffClassificationDescriptor.LEAAdministrator)
                {
                    return LeaAdministratorLoginId;
                }

                if (requirement.StaffClassification == StaffClassificationDescriptor.Superintendent)
                {
                    return SuperintendentLoginId;
                }
            }

            return name.GenerateLoginId();
        }

        /* Where possible, given the constraints of the school configuration, attempts to assign staff of 
        *  Hispanic/Latino ethnicity to roles that require Spanish Language proficiency.
        *  Certain sections serve ESL/Migrant/Spanish Speaking students, and are more likely to require a teacher that can speak Spanish.
        *  Note: The Hispanic/Latino population size is set with School configuration, and the language requirements are pulled from Section data.
        *  These two values are not configured together in any way.  This means that there is no guarantee that there
        *  will be enough Hispanic staff to assign to Spanish language classes. */
        private readonly Dictionary<int, List<StaffHeritage>> _generatedStaffHeritageListByEducationOrganization = new Dictionary<int, List<StaffHeritage>>();
        private StaffHeritage GetGeneratedStaffHeritage(GlobalDataGeneratorContext context, StaffRequirement staffRequirement)
        {
            if (!_generatedStaffHeritageListByEducationOrganization.ContainsKey(staffRequirement.EducationOrganizationId))
            {
                _generatedStaffHeritageListByEducationOrganization[staffRequirement.EducationOrganizationId] = context
                    .GlobalData
                    .StaffAssociationData
                    .StaffRequirements
                    .Where(x => x.EducationOrganizationId == staffRequirement.EducationOrganizationId)
                    .Select(x => x.IsLeaAdministrator ? GetRandomStaffHeritage() : GetRandomStaffHeritage(x.GetStaffProfile(Configuration)))
                    .ToList();
            }

            if (staffRequirement.SpeaksSpanish)
            {
                var randomHispanicHeritage = _generatedStaffHeritageListByEducationOrganization[staffRequirement.EducationOrganizationId].FirstOrDefault(x => !x.IsAssignedToStaffMember && x.HispanicLatinoEthnicity);
                if (randomHispanicHeritage != null)
                {
                    randomHispanicHeritage.IsAssignedToStaffMember = true;
                    return randomHispanicHeritage;
                }
            }

            else
            {
                var randomNonHispanicHeritage = _generatedStaffHeritageListByEducationOrganization[staffRequirement.EducationOrganizationId].FirstOrDefault(x => !x.IsAssignedToStaffMember && !x.HispanicLatinoEthnicity);
                if (randomNonHispanicHeritage != null)
                {
                    randomNonHispanicHeritage.IsAssignedToStaffMember = true;
                    return randomNonHispanicHeritage;
                }
            }

            var defaultHeritage = _generatedStaffHeritageListByEducationOrganization[staffRequirement.EducationOrganizationId].First();
            defaultHeritage.IsAssignedToStaffMember = true;
            return defaultHeritage;
        }
        

        private IdentificationDocument[] GetStaffIdentificationDocuments()
        {
            var identificationDocument = new IdentificationDocument
            {
                PersonalInformationVerification = _primaryIdentificationTypes.GetRandomItemWithDistribution(RandomNumberGenerator).GetStructuredCodeValue(),
                IdentificationDocumentUse = IdentificationDocumentUseDescriptor.PersonalInformationVerification.GetStructuredCodeValue()
            };

            return new[] {identificationDocument};
        }

        private static ElectronicMail[] GetEmail(string loginId, string educationOrganizationName)
        {
            var email = BiographicalGeneratorHelpers.GenerateOrganizationalEmailAddress(loginId, educationOrganizationName);
            return new[] {email};
        }

        private LevelOfEducationDescriptor GetLevelOfEducation(bool highlyQualified)
        {
            if (!highlyQualified)
            {
                return LevelOfEducationDescriptor.BachelorS;
            }

            var highlyQualifiedLevelOfEducationTypes = new List<LevelOfEducationDescriptor>
            {
                LevelOfEducationDescriptor.Doctorate,
                LevelOfEducationDescriptor.MasterS
            };

                return highlyQualifiedLevelOfEducationTypes
                    .GetRandomItem(RandomNumberGenerator);
        }

        private static StaffIdentificationCode[] GetStaffIdentificationCode(string staffUniqueId, StaffRequirement staffRequirement)
        {
            var identificationCode = new StaffIdentificationCode
            {
                AssigningOrganizationIdentificationCode = staffRequirement.EducationOrganizationId.ToString(),
                IdentificationCode = staffUniqueId,
                StaffIdentificationSystem = staffRequirement.IsLeaAdministrator 
                    ? StaffIdentificationSystemDescriptor.District.GetStructuredCodeValue()
                    : StaffIdentificationSystemDescriptor.School.GetStructuredCodeValue()
            };
            return new[] {identificationCode};
        }

        private StaffHeritage GetRandomStaffHeritage(IStaffProfile profile)
        {
            var randomRace = profile.StaffRaceConfiguration.GetRandomItem(RandomNumberGenerator).Value;
            return GetHeritage(randomRace);
        }

        private StaffHeritage GetRandomStaffHeritage()
        {
            var raceString = Configuration.GlobalConfig.EthnicityMappings.GetRandomItem(RandomNumberGenerator).Ethnicity;
            return GetHeritage(raceString);
        }

        private StaffHeritage GetHeritage(string raceString)
        {
            var raceTypes = Configuration.GlobalConfig.EthnicityMappings
                .Where(x => x.Ethnicity == raceString)
                .Select(x => x.GetRaceDescriptor())
                .ToArray();

            var isHispanicLatinoEthnicity = Configuration.GlobalConfig.EthnicityMappings
                .Where(x => x.HispanicLatinoEthnicity)
                .Select(x => x.Ethnicity)
                .Contains(raceString);

            var oldEthnicity = Configuration.GlobalConfig.EthnicityMappings
                .Where(x => x.Ethnicity == raceString)
                .ToArray()
                .OldEthnicity(isHispanicLatinoEthnicity);

            if (!raceTypes.Any())
            {
                raceTypes = new[] {RaceDescriptor.ChooseNotToRespond};
            }

            return new StaffHeritage
            {
                Races = raceTypes,
                HispanicLatinoEthnicity = isHispanicLatinoEthnicity,
                OldEthnicityDescriptor = oldEthnicity
            };
        }

        private SexDescriptor GetRandomStaffGender(IStaffProfile profile)
        {
            var staffGender = profile.StaffSexConfiguration.GetRandomItem(RandomNumberGenerator).Value;
            return GetGender(staffGender);
        }

        private SexDescriptor GetRandomStaffGender()
        {
            return Configuration.GlobalConfig.GenderMappings.GetRandomItem(RandomNumberGenerator).GetSexDescriptor();
        }

        private SexDescriptor GetGender(string genderString)
        {
            var matchingGenders = Configuration.GlobalConfig.GenderMappings
                .Where(x => x.Gender == genderString)
                .Select(x => x.GetSexDescriptor())
                .ToArray();

            return matchingGenders.Any() ? matchingGenders.First() : SexDescriptor.NotSelected;
        }

        private class StaffHeritage
        {
            public RaceDescriptor[] Races { get; set; }
            public bool HispanicLatinoEthnicity { get; set; }
            public OldEthnicityDescriptor OldEthnicityDescriptor { get; set; }
            public bool IsAssignedToStaffMember { get; set; }
        }

        private StaffExperience GenerateStaffExperience (DateTime birthday, bool highlyQualifiedTeacher)
        {
            var referenceDate = Configuration.GlobalConfig.TimeConfig.SchoolCalendarConfig.StartDate;

            var dateStartedTeaching = RandomNumberGenerator.GetRandomDay(birthday.AddYears(MinStaffAge), referenceDate);
            var yearsOfTeachingExperience = (int)((referenceDate - dateStartedTeaching).TotalDays / 365);

            if (highlyQualifiedTeacher && yearsOfTeachingExperience < MinYearsTeachingExperienceForHighlyQualifiedTeachers)
            {
                dateStartedTeaching = new DateTime(referenceDate.Year - MinYearsTeachingExperienceForHighlyQualifiedTeachers, 1, 1);
                yearsOfTeachingExperience = MinYearsTeachingExperienceForHighlyQualifiedTeachers;
            }

            var dateOfFirstProfessionalExperience = RandomNumberGenerator.GetRandomDay(birthday.AddYears(MinimumProfessionalWorkingAge), dateStartedTeaching);
            var yearsOfProfessionalExperience = (int)((referenceDate - dateOfFirstProfessionalExperience).TotalDays / 365);

            return new StaffExperience
            {
                YearsOfTeachingExperience = yearsOfTeachingExperience,
                TotalYearsOfProfessionalExperience = yearsOfProfessionalExperience
            };
        }

        private class StaffExperience
        {
            public int TotalYearsOfProfessionalExperience { get; set; }
            public int YearsOfTeachingExperience { get; set; }
        }
    }
}
