using System.Collections.Generic;
using EdFi.SampleDataGenerator.Core.Config;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common.Attributes;
using EdFi.SampleDataGenerator.Core.Entities;
using EdFi.SampleDataGenerator.Core.Helpers;

namespace EdFi.SampleDataGenerator.Core.DataGeneration.Generators.Student.Attributes
{
    public class StudentIdentificationDocumentGenerator : SampleDataEntityAttributeGeneratorBase<StudentDataGeneratorContext, StudentDataGeneratorConfig>
    {
        public override IEntityField GeneratesField => StudentField.PersonalIdenficationDocument;

        public override IEntityField[] DependsOnFields => new[] {  StudentField.Name };

        private readonly List<RandomOption<PersonalInformationVerificationDescriptor>> _immigrantOptions = new List<RandomOption<PersonalInformationVerificationDescriptor>>
        {
            new RandomOption<PersonalInformationVerificationDescriptor>(PersonalInformationVerificationDescriptor.EntryInFamilyBible, 0.2),
            new RandomOption<PersonalInformationVerificationDescriptor>(PersonalInformationVerificationDescriptor.ImmigrationDocumentVisa, 0.8)
        };

        private readonly List<RandomOption<PersonalInformationVerificationDescriptor>> _upperClassmenOptions = new List<RandomOption<PersonalInformationVerificationDescriptor>>
        {
            new RandomOption<PersonalInformationVerificationDescriptor>(PersonalInformationVerificationDescriptor.DriversLicense, 0.5),
            new RandomOption<PersonalInformationVerificationDescriptor>(PersonalInformationVerificationDescriptor.BirthCertificate, 0.5)
        };

        public StudentIdentificationDocumentGenerator(IRandomNumberGenerator randomNumberGenerator) : base(randomNumberGenerator)
        {
        }

        protected override void GenerateCore(StudentDataGeneratorContext context)
        {
            var student = context.Student;
            PersonalInformationVerificationDescriptor verificationType;

            if (context.StudentCharacteristics.IsImmigrant)
            {
                verificationType = _immigrantOptions.GetRandomItemWithDistribution(RandomNumberGenerator).Value;
            }

            else if (Configuration.GradeProfile.GetGradeLevel() == GradeLevelDescriptor.EleventhGrade ||
                     Configuration.GradeProfile.GetGradeLevel() == GradeLevelDescriptor.TwelfthGrade)
            {
                verificationType = _upperClassmenOptions.GetRandomItemWithDistribution(RandomNumberGenerator).Value;
            }

            else
            {
                verificationType = PersonalInformationVerificationDescriptor.BirthCertificate;
            }

            student.Name.PersonalIdentificationDocument = new[]
            {
                new IdentificationDocument
                {
                    PersonalInformationVerification = verificationType.GetStructuredCodeValue(),
                    IdentificationDocumentUse = IdentificationDocumentUseDescriptor.PersonalInformationVerification.GetStructuredCodeValue()
                }
            };

            LogStat(verificationType.CodeValue);
        }
    }
}
