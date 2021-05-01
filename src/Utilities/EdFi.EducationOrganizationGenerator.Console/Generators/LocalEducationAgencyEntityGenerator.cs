using System.Linq;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common.Entity;
using EdFi.SampleDataGenerator.Core.DataGeneration.InterchangeEntities;
using EdFi.SampleDataGenerator.Core.Entities;
using EdFi.SampleDataGenerator.Core.Helpers;
using EdFi.SampleDataGenerator.Core.Serialization.Output.Interchanges;

namespace EdFi.EducationOrganizationGenerator.Console.Generators
{
    public class LocalEducationAgencyEntityGenerator : EducationOrganizationEntityGenerator
    {
        public LocalEducationAgencyEntityGenerator(IRandomNumberGenerator randomNumberGenerator) : base(randomNumberGenerator)
        {
        }

        public override IEntity GeneratesEntity => EducationOrganizationEntity.LocalEducationAgency;
        public override IEntity[] DependsOnEntities => new IEntity[] { EducationOrganizationEntity.EducationServiceCenter };

        protected override void GenerateCore(EducationOrganizationData context)
        {
            var localEducationAgency = GenerateLocalEducationAgency(context);
            context.LocalEducationAgencies.Add(localEducationAgency);
        }

        private LocalEducationAgency GenerateLocalEducationAgency(EducationOrganizationData context)
        {
            var physicalAddress = Configuration.DistrictProfile.LocationInfo.GeneratePhysicalAddress(RandomNumberGenerator, Configuration.StreetNameFile);

            return new LocalEducationAgency
            {
                id = $"LEAG_{Configuration.DistrictProfile.StateId}",
                NameOfInstitution = Configuration.DistrictProfile.DistrictName,
                EducationOrganizationCategory = new []{ EducationOrganizationCategoryDescriptor.LocalEducationAgency.GetStructuredCodeValue(), },
                LocalEducationAgencyCategory = LocalEducationAgencyCategoryDescriptor.Independent.GetStructuredCodeValue(),
                LocalEducationAgencyId = int.Parse(Configuration.DistrictProfile.StateId),
                EducationOrganizationIdentificationCode = new []
                {
                    new EducationOrganizationIdentificationCode
                    {
                        IdentificationCode = Configuration.DistrictProfile.StateId,
                        EducationOrganizationIdentificationSystem = EducationOrganizationIdentificationSystemDescriptor.SEA.GetStructuredCodeValue(),
                    }
                },
                Address = new[] { physicalAddress },
                InstitutionTelephone = new []
                {
                    new InstitutionTelephone
                    {
                        InstitutionTelephoneNumberType = InstitutionTelephoneNumberTypeDescriptor.Main.CodeValue,
                        TelephoneNumber = Configuration.DistrictProfile.LocationInfo.GetPhoneNumberForCity(RandomNumberGenerator, physicalAddress.City)
                    }
                },
                EducationServiceCenterReference = new EducationServiceCenterReferenceType
                {
                    EducationServiceCenterIdentity = new EducationServiceCenterIdentityType
                    {
                        EducationServiceCenterId = context.EducationServiceCenters.First().EducationServiceCenterId
                    }
                }
            };
        }
    }
}
