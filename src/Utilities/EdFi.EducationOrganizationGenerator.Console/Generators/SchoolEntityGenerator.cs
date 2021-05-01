using System.Collections.Generic;
using System.Linq;
using EdFi.EducationOrganizationGenerator.Console.Configuration;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common.Entity;
using EdFi.SampleDataGenerator.Core.DataGeneration.InterchangeEntities;
using EdFi.SampleDataGenerator.Core.Entities;
using EdFi.SampleDataGenerator.Core.Helpers;
using EdFi.SampleDataGenerator.Core.Serialization.Output.Interchanges;

namespace EdFi.EducationOrganizationGenerator.Console.Generators
{
    public class SchoolEntityGenerator : EducationOrganizationEntityGenerator
    {
        public SchoolEntityGenerator(IRandomNumberGenerator randomNumberGenerator) : base(randomNumberGenerator)
        {
        }

        public override IEntity GeneratesEntity => EducationOrganizationEntity.School;
        public override IEntity[] DependsOnEntities => new IEntity[] { EducationOrganizationEntity.LocalEducationAgency };

        private readonly HashSet<string> _previouslyGeneratedSchoolNames = new HashSet<string>(); 

        protected override void GenerateCore(EducationOrganizationData context)
        {
            var schoolNumber = 1;
            var stateId = Configuration.DistrictProfile.StateId;

            foreach (var schoolProfile in Configuration.DistrictProfile.SchoolProfiles)
            {
                var gradeLevelReferences = schoolProfile.GradeProfiles.Select(sp => sp.GradeLevel).ToArray();

                for (var i = 0; i < schoolProfile.Count; ++i)
                {
                    var stateOrganizationId = $"{stateId}{schoolNumber:D3}";
                    var schoolName = GetNewSchoolName(schoolProfile);
                    var physicalAddress = Configuration.DistrictProfile.LocationInfo.GeneratePhysicalAddress(RandomNumberGenerator, Configuration.StreetNameFile);

                    var school = new School
                    {
                        id = $"SCOL_{stateOrganizationId}",
                        NameOfInstitution = schoolName,
                        ShortNameOfInstitution = GetSchoolShortName(schoolName),
                        OperationalStatus = OperationalStatusDescriptor.Active.GetStructuredCodeValue(),
                        Address = new []{ physicalAddress },
                        EducationOrganizationCategory = new []{ EducationOrganizationCategoryDescriptor.School.GetStructuredCodeValue(), },
                        CharterStatus = CharterStatusDescriptor.NotACharterSchool.GetStructuredCodeValue(),
                        SchoolCategory = new [] { schoolProfile.SchoolType },
                        SchoolType = SchoolTypeDescriptor.Regular.GetStructuredCodeValue(),
                        SchoolId = int.Parse(stateOrganizationId),
                        TitleIPartASchoolDesignation = TitleIPartASchoolDesignationDescriptor.NotATitleISchool.GetStructuredCodeValue(),
                        EducationOrganizationIdentificationCode = new []
                        {
                            new EducationOrganizationIdentificationCode
                            {
                                IdentificationCode = stateOrganizationId,
                                EducationOrganizationIdentificationSystem = EducationOrganizationIdentificationSystemDescriptor.SEA.GetStructuredCodeValue()
                            }
                        },
                        InstitutionTelephone = new []
                        {
                            new InstitutionTelephone
                            {
                                InstitutionTelephoneNumberType = InstitutionTelephoneNumberTypeDescriptor.Main.GetStructuredCodeValue(),
                                TelephoneNumber = Configuration.DistrictProfile.LocationInfo.GetPhoneNumberForCity(RandomNumberGenerator, physicalAddress.City)
                            }
                        },
                        AdministrativeFundingControl = AdministrativeFundingControlDescriptor.PublicSchool.GetStructuredCodeValue(),
                        LocalEducationAgencyReference = context.LocalEducationAgencies.First().GetLocalEducationOrganizationAgencyReference(),
                        GradeLevel = gradeLevelReferences
                    };

                    context.Schools.Add(school);

                    ++schoolNumber;
                }
            }
        }

        private string GetNewSchoolName(SchoolProfile schoolProfile)
        {
            string result = "";
            do
            {
                var baseName = Configuration.SchoolBaseNames.GetRandomItem(RandomNumberGenerator);
                var schoolType = schoolProfile.SchoolType;

                result = $"{baseName} {schoolType}";
            } while (_previouslyGeneratedSchoolNames.Contains(result) || _previouslyGeneratedSchoolNames.Count >= Configuration.SchoolBaseNames.Count);

            _previouslyGeneratedSchoolNames.Add(result);
            return result;
        }

        private string GetSchoolShortName(string name)
        {
            return string.Join("", name.Split(' ').Select(s => s.First())).ToUpperInvariant();
        }
    }
}
