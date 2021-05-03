using System.Linq;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common.Dependencies;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common.Entity;
using EdFi.SampleDataGenerator.Core.DataGeneration.InterchangeEntities;
using EdFi.SampleDataGenerator.Core.Entities;
using EdFi.SampleDataGenerator.Core.Helpers;

namespace EdFi.SampleDataGenerator.Core.DataGeneration.Generators.StudentEnrollment
{
    public sealed class StudentEducationOrganizationEntityGenerator : StudentEnrollmentEntityGenerator
    {
        public StudentEducationOrganizationEntityGenerator(IRandomNumberGenerator randomNumberGenerator) : base(randomNumberGenerator)
        {
        }

        public override IEntity GeneratesEntity => StudentEnrollmentEntity.StudentEducationOrganizationAssociation;
        public override IEntity[] DependsOnEntities => EntityDependencies.None;
        public double FreeLunchPercentage = 0.2; //% of students that are free/reduced lunch that actually get free lunch; the remainder get reduced price

        protected override void GenerateCore(StudentDataGeneratorContext context)
        {

            context.StudentCharacteristics.IsHomeless = GetIsHomeless(context);
            context.StudentCharacteristics.IsEconomicDisadvantaged = GetIsEconomicDisadvantaged(context);
            context.StudentCharacteristics.FoodServiceElected = GetIsFoodServiceEligible(context);

            var schoolId = Configuration.SchoolProfile.SchoolId;
            var edOrgAssociation = new StudentEducationOrganizationAssociation
            {
                Race = new[] { context.StudentCharacteristics.Race.GetStructuredCodeValue() },
                OldEthnicity = context.StudentCharacteristics.OldEthnicity.GetStructuredCodeValue(),
                HispanicLatinoEthnicity = context.StudentCharacteristics.HispanicLatinoEthnicity,
                StudentReference = context.Student.GetStudentReference(),
                EducationOrganizationReference = EdFiReferenceTypeHelpers.GetEducationOrganizationReference(schoolId),
                LimitedEnglishProficiency = LimitedEnglishProficiencyDescriptor.NotLimited.GetStructuredCodeValue(),
                Address = GenerateAddress(context),
                ElectronicMail = GenerateElectronicMail(context),
                Sex = context.StudentCharacteristics.Sex.GetStructuredCodeValue(),
                StudentIdentificationCode = GenerateStudentIdentificationCode(context.Student.StudentUniqueId)
            };
            GenerateTelephone(context, edOrgAssociation);
            GenerateLanguage(context, edOrgAssociation);

            context.GeneratedStudentData.StudentEnrollmentData.StudentEducationOrganizationAssociation.Add(edOrgAssociation);
            GenerateStudentCharacteristics(context, edOrgAssociation);
        }

        private void GenerateLanguage(StudentDataGeneratorContext context, StudentEducationOrganizationAssociation edOrgAssociation)
        {
            if (!context.StudentCharacteristics.IsImmigrant) return;

            var language = edOrgAssociation.HispanicLatinoEthnicity ? LanguageDescriptor.Spanish_spa : LanguageDescriptor.French_fre;
            edOrgAssociation.SetPrimaryLanguage(language);
        }

        private StudentIdentificationCode[] GenerateStudentIdentificationCode(string uniqueId)
        {
            return new[]
            {
                new StudentIdentificationCode
                {
                    AssigningOrganizationIdentificationCode = "Local",
                    IdentificationCode = uniqueId,
                    StudentIdentificationSystem = StudentIdentificationSystemDescriptor.Local.GetStructuredCodeValue()
                }
            };
        }

        private void GenerateTelephone(StudentDataGeneratorContext context, StudentEducationOrganizationAssociation edOrgAssociation)
        {
            if (context.StudentCharacteristics.IsHomeless)
            {
                return;
            }

            var city = edOrgAssociation.Address.First().City;

            edOrgAssociation.Telephone = new[]
            {
                Configuration.DistrictProfile.LocationInfo.GenerateTelephoneNumber(RandomNumberGenerator, city)
            };
        }

        private ElectronicMail[] GenerateElectronicMail(StudentDataGeneratorContext context)
        {
            var name = context.Student.Name;

            return new[]
            {
                BiographicalGeneratorHelpers.GeneratePersonalEmailAddress(name, context.GlobalStudentNumber)
            };
        }

        private Address[] GenerateAddress(StudentDataGeneratorContext context)
        {
            if (context.StudentCharacteristics.IsHomeless)
                return null;

            return new[]
            {
                Configuration.DistrictProfile.LocationInfo.GenerateHomeAddress(RandomNumberGenerator,
                    Configuration.NameFileData.StreetNameFile)
            };
        }

        private bool GetIsHomeless(StudentDataGeneratorContext context)
        {
            var option = Configuration
                .StudentProfile
                .HomelessStatusConfiguration?
                .GetRandomItem(context, Configuration.GlobalConfig.EthnicityMappings, RandomNumberGenerator);

            return option != null;
        }

        private bool GetIsEconomicDisadvantaged(StudentDataGeneratorContext context)
        {
            var configOption = Configuration
                .StudentProfile
                .EconomicDisadvantageConfiguration?
                .GetRandomItem(context, Configuration.GlobalConfig.EthnicityMappings, RandomNumberGenerator);

            return configOption != null;
        }

        private SchoolFoodServiceProgramServiceDescriptor GetIsFoodServiceEligible(StudentDataGeneratorContext context)
        {
            if (context.StudentCharacteristics.IsEconomicDisadvantaged)
            {
                var randomValue = RandomNumberGenerator.GenerateDouble();
                var result = SchoolFoodServiceProgramServiceDescriptor.ReducedPriceLunch;

                if (randomValue >= 1 - FreeLunchPercentage)
                {
                    result = SchoolFoodServiceProgramServiceDescriptor.FreeLunch;
                }

                return result;
            }
            return null;
        }

        private void GenerateStudentCharacteristics(StudentDataGeneratorContext context, StudentEducationOrganizationAssociation edOrgAssociation)
        {
            if (context.StudentCharacteristics.IsHomeless)
                MakeHomeless(edOrgAssociation);

            if (context.StudentCharacteristics.IsEconomicDisadvantaged)
                MakeEconomicDisadvantaged(edOrgAssociation);

            if (context.StudentCharacteristics.IsHomeless || context.StudentCharacteristics.IsEconomicDisadvantaged || context.StudentCharacteristics.IsFoodServiceEligible)
                MakeAtRisk(edOrgAssociation);

            if (context.StudentCharacteristics.IsImmigrant)
                MakeImmigrant(edOrgAssociation);
        }

        private static void MakeHomeless(StudentEducationOrganizationAssociation edOrgAssociation)
        {
            if (edOrgAssociation.HasCharacteristic(StudentCharacteristicDescriptor.Homeless)) return;
            var homelessStatus = new StudentCharacteristic
            {
                StudentCharacteristic1 = StudentCharacteristicDescriptor.Homeless.GetStructuredCodeValue()
            };
            edOrgAssociation.StudentCharacteristic = edOrgAssociation.StudentCharacteristic.CreateCopyAndAppend(homelessStatus);
        }

        private static void MakeAtRisk(StudentEducationOrganizationAssociation edOrgAssociation)
        {
            var atRiskStatus = new StudentIndicator
            {
                Indicator = "True",
                IndicatorName = "At Risk"
            };

            if (edOrgAssociation.HasIndicator(atRiskStatus)) return;
            edOrgAssociation.StudentIndicator = edOrgAssociation.StudentIndicator.CreateCopyAndAppend(atRiskStatus);
        }

        public static void MakeImmigrant(StudentEducationOrganizationAssociation edOrgAssociation)
        {
            if (edOrgAssociation.HasCharacteristic(StudentCharacteristicDescriptor.Immigrant)) return;

            var immigrantCharacteristic = new StudentCharacteristic
            {
                StudentCharacteristic1 = StudentCharacteristicDescriptor.Immigrant.GetStructuredCodeValue()
            };

            edOrgAssociation.StudentCharacteristic = edOrgAssociation.StudentCharacteristic.CreateCopyAndAppend(immigrantCharacteristic);
        }

        public static void MakeEconomicDisadvantaged(StudentEducationOrganizationAssociation edOrgAssociation)
        {
            if (edOrgAssociation.HasCharacteristic(StudentCharacteristicDescriptor.EconomicDisadvantaged)) return;

            var economicDisadvantaged = new StudentCharacteristic
            {
                StudentCharacteristic1 = StudentCharacteristicDescriptor.EconomicDisadvantaged.GetStructuredCodeValue()
            };

            edOrgAssociation.StudentCharacteristic = edOrgAssociation.StudentCharacteristic.CreateCopyAndAppend(economicDisadvantaged);
        }
    }
}
