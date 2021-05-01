using EdFi.SampleDataGenerator.Core.DataGeneration.Common;
using EdFi.SampleDataGenerator.Core.Entities;
using EdFi.SampleDataGenerator.Core.Helpers;
using System.Linq;
using EdFi.SampleDataGenerator.Core.Config;
using EdFi.SampleDataGenerator.Core.DataGeneration.Generators.Student;

namespace EdFi.SampleDataGenerator.Core.DataGeneration.Generators.Parent
{
    public static class ParentGeneratorHelpers
    {
        private static int _parentId = 1;
        public static bool IsParent(this StudentParentAssociation studentParentAssociation)
        {
            return studentParentAssociation.Relation == RelationDescriptor.Father.GetStructuredCodeValue() || studentParentAssociation.Relation == RelationDescriptor.Mother.GetStructuredCodeValue();
        }
        public  static void MakePrimaryContact(this StudentParentAssociation studentParentAssociation)
        {
           studentParentAssociation.UpdateParentPrimaryContact(true);
        }

        public static void MakeSecondaryContact(this StudentParentAssociation studentParentAssociation)
        {
            studentParentAssociation.UpdateParentPrimaryContact(false);
        }

        private static void UpdateParentPrimaryContact(this StudentParentAssociation studentParentAssociation, bool isPrimaryContact)
        {
            studentParentAssociation.PrimaryContactStatus = isPrimaryContact;
            studentParentAssociation.ContactPriority = isPrimaryContact ? 1 : 2;
            studentParentAssociation.PrimaryContactStatusSpecified = true;
        }

        public static ParentProfile GenerateParentProfile(StudentDataGeneratorContext context, IRandomNumberGenerator randomNumberGenerator, StudentDataGeneratorConfig configuration)
        {
            var result = new ParentProfile
            {
                FamilyStructure = FamilyStructure.FamilyStructureOdds.GetRandomItemWithDistribution(randomNumberGenerator)
            };

            if (result.FamilyStructure == FamilyStructureType.MarriedParents)
            {
                const double chancePrimaryParentIsMother = 0.75;
                const double chanceSecondaryParentIsStep = 0.5;

                var primaryParentIsMother = randomNumberGenerator.GetRandomBool(chancePrimaryParentIsMother);
                var secondaryParentIsStep = randomNumberGenerator.GetRandomBool(chanceSecondaryParentIsStep);

                result.Parent1 = new Parent
                {
                    LivesWithStudent = true,
                    RelationDescriptor = primaryParentIsMother ? RelationDescriptor.Mother : RelationDescriptor.Father,
                    Remarried = secondaryParentIsStep
                };

                result.Parent2 = new Parent
                {
                    LivesWithStudent = true,
                    RelationDescriptor = result.Parent1.RelationDescriptor.GetCounterpartRelationType(secondaryParentIsStep),
                    Remarried = secondaryParentIsStep
                };
            }

            else if (result.FamilyStructure == FamilyStructureType.SingleMother || result.FamilyStructure == FamilyStructureType.SingleFather)
            {
                const double chanceForSecondParent = 0.5;
                var hasSecondParent = randomNumberGenerator.GetRandomBool(chanceForSecondParent);

                result.Parent1 = new Parent
                {
                    LivesWithStudent = true,
                    RelationDescriptor = result.FamilyStructure == FamilyStructureType.SingleMother ? RelationDescriptor.Mother : RelationDescriptor.Father,
                    Remarried = false
                };

                if (hasSecondParent)
                {
                    result.Parent2 = new Parent
                    {
                        LivesWithStudent = false,
                        RelationDescriptor = result.Parent1.RelationDescriptor.GetCounterpartRelationType(),
                        Remarried = false
                    };
                }
            }

            else
            {
                const double chanceForSecondParent = 0.75;
                var hasSecondParent = randomNumberGenerator.GetRandomBool(chanceForSecondParent);

                result.Parent1 = new Parent
                {
                    LivesWithStudent = true,
                    RelationDescriptor = FamilyStructure.NonParentRelationshipTypes.GetRandomItemWithDistribution(randomNumberGenerator),
                    Remarried = false
                };

                if (hasSecondParent)
                {
                    result.Parent2 = new Parent
                    {
                        LivesWithStudent = true,
                        RelationDescriptor = result.Parent1.RelationDescriptor.GetCounterpartRelationType(),
                        Remarried = false
                    };
                }
            }

            PopulateParentEntity(context, result.Parent1, randomNumberGenerator, configuration);
            PopulateParentEntity(context, result.Parent2, randomNumberGenerator, configuration);

            return result;
        }

        private static void PopulateParentEntity(StudentDataGeneratorContext context, Parent parent, IRandomNumberGenerator randomNumberGenerator, StudentDataGeneratorConfig configuration)
        {
            if (parent == null) return;

            var parentUniqueId = $"{_parentId++:D6}";
            var name = context.GenerateParentName(parent, randomNumberGenerator, configuration);
            var loginId = name.GenerateLoginId();
            var address = parent.LivesWithStudent
                ? context.GetStudentEducationOrganization().Address
                : new[] { configuration.DistrictProfile.LocationInfo.GenerateHomeAddress(randomNumberGenerator, configuration.NameFileData.StreetNameFile) };
            var city = address?.First()?.City ?? configuration.DistrictProfile.LocationInfo.Cities.First().Name;

            parent.Entity = new Entities.Parent
            {
                id = $"PRNT_{parentUniqueId}",
                ParentUniqueId = parentUniqueId,
                Address = address,
                ElectronicMail = new[] { BiographicalGeneratorHelpers.GeneratePersonalEmailAddress(loginId) },
                LoginId = loginId,
                Name = name,
                Sex = parent.RelationDescriptor.GetSexDescriptor().GetStructuredCodeValue(),
                Telephone = new[] { configuration.DistrictProfile.LocationInfo.GenerateTelephoneNumber(randomNumberGenerator, city) }
            };
        }

        private static Name GenerateParentName(this StudentDataGeneratorContext context, Parent parent, IRandomNumberGenerator randomNumberGenerator, StudentDataGeneratorConfig configuration)
        {
            var raceType = context.StudentCharacteristics.Race;
            var sexType = parent.RelationDescriptor.GetSexDescriptor();

            var isStepParent = parent.RelationDescriptor == RelationDescriptor.FatherStep ||
                               parent.RelationDescriptor == RelationDescriptor.MotherStep;

            var paternalLineage = randomNumberGenerator.GetRandomBool() &&
                                  parent.RelationDescriptor != RelationDescriptor.Mother &&
                                  !isStepParent;

            var lastNameMatchesStudent = parent.RelationDescriptor == RelationDescriptor.Father ||
                                         (parent.RelationDescriptor == RelationDescriptor.Mother && !parent.Remarried) ||
                                         paternalLineage;

            var isHispanicLatinoEthnicity =
                raceType == RaceDescriptor.White && lastNameMatchesStudent && context.StudentCharacteristics.HispanicLatinoEthnicity;

            var ethnicityMapping = configuration.GlobalConfig.EthnicityMappings.MappingFor(raceType, isHispanicLatinoEthnicity);

            var name = NameGenerator.Generate(configuration.NameFileData, randomNumberGenerator, sexType, ethnicityMapping);
            if (lastNameMatchesStudent)
            {
                name.LastSurname = context.Student.Name.LastSurname;
            }

            return name;
        }

        public static StudentParentAssociation[] GenerateStudentParentAssociations(this Entities.Student student, ParentProfile parents)
        {
            if (parents.Parent2 == null)
            {
                return new[] { GenerateStudentParentAssociation(student, parents.Parent1, true) };
            }

            return new[]
            {
                student.GenerateStudentParentAssociation(parents.Parent1, true),
                student.GenerateStudentParentAssociation(parents.Parent2, false)
            };
        }

        public static StudentParentAssociation GenerateStudentParentAssociation(this Entities.Student student, Parent parent, bool isPrimaryContact)
        {
            return new StudentParentAssociation
            {
                ContactPriority = isPrimaryContact ? 1 : 2,
                ContactPrioritySpecified = true,
                EmergencyContactStatus = true,
                EmergencyContactStatusSpecified = true,
                LivesWith = parent.LivesWithStudent,
                LivesWithSpecified = true,
                ParentReference = parent.Entity.GetParentReference(),
                PrimaryContactStatus = isPrimaryContact,
                PrimaryContactStatusSpecified = true,
                Relation = parent.RelationDescriptor.GetStructuredCodeValue(),
                StudentReference = student.GetStudentReference()
            };
        }
    }
}
