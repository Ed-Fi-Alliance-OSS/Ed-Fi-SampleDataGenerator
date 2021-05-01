using EdFi.SampleDataGenerator.Core.DataGeneration.Common;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common.Dependencies;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common.Entity;
using EdFi.SampleDataGenerator.Core.DataGeneration.InterchangeEntities;
using EdFi.SampleDataGenerator.Core.Entities;
using EdFi.SampleDataGenerator.Core.Helpers;
using EdFi.SampleDataGenerator.Core.Serialization.Output.Interchanges;

namespace EdFi.EducationOrganizationGenerator.Console.Generators
{
    public class EducationServiceCenterEntityGenerator : EducationOrganizationEntityGenerator
    {
        public EducationServiceCenterEntityGenerator(IRandomNumberGenerator randomNumberGenerator) : base(randomNumberGenerator)
        {
        }

        public override IEntity GeneratesEntity => EducationOrganizationEntity.EducationServiceCenter;
        public override IEntity[] DependsOnEntities => EntityDependencies.None;

        protected override void GenerateCore(EducationOrganizationData context)
        {
            var educationServiceCenter = GenerateEducationServiceCenter();
            context.EducationServiceCenters.Add(educationServiceCenter);
        }

        private EducationServiceCenter GenerateEducationServiceCenter()
        {
            var physicalAddress = Configuration.DistrictProfile.LocationInfo.GeneratePhysicalAddress(RandomNumberGenerator, Configuration.StreetNameFile);
            var stateId = Configuration.DistrictProfile.StateId;
            var statePrefix = stateId.Substring(0, stateId.Length - 2);
            var suffix = stateId.Substring(stateId.Length - 2);

            var escId = suffix == "00"
                ? $"{statePrefix}01"
                : $"{statePrefix}00";

            var regionId = RandomNumberGenerator.Generate(1, 100);

            return new EducationServiceCenter
            {
                id = $"ESC_{escId}",
                Address = new[] { physicalAddress },
                EducationOrganizationCategory = new[] { EducationOrganizationCategoryDescriptor.EducationServiceCenter.GetStructuredCodeValue(), },
                EducationOrganizationIdentificationCode = new[]
                {
                    new EducationOrganizationIdentificationCode
                    {
                        IdentificationCode = escId,
                        EducationOrganizationIdentificationSystem = EducationOrganizationIdentificationSystemDescriptor.NCES.GetStructuredCodeValue(),
                    }
                },
                EducationServiceCenterId = int.Parse(escId),
                InstitutionTelephone = new[]
                {
                    new InstitutionTelephone
                    {
                        InstitutionTelephoneNumberType = InstitutionTelephoneNumberTypeDescriptor.Main.GetStructuredCodeValue(),
                        TelephoneNumber = Configuration.DistrictProfile.LocationInfo.GetPhoneNumberForCity(RandomNumberGenerator, physicalAddress.City)
                    }
                },
                NameOfInstitution = $"Region {regionId} Education Service Center"
            };
        }
    }
}
