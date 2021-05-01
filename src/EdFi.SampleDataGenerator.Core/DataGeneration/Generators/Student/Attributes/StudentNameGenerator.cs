using System.Linq;
using EdFi.SampleDataGenerator.Core.Config;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common.Attributes;
using EdFi.SampleDataGenerator.Core.Entities;
using EdFi.SampleDataGenerator.Core.Helpers;

namespace EdFi.SampleDataGenerator.Core.DataGeneration.Generators.Student.Attributes
{
    public class StudentNameGenerator : SampleDataEntityAttributeGeneratorBase<StudentDataGeneratorContext, StudentDataGeneratorConfig>
    {
        public override IEntityField GeneratesField => StudentField.Name;
        public override IEntityField[] DependsOnFields => new[] { StudentField.Race, StudentField.Sex };

        public StudentNameGenerator(IRandomNumberGenerator randomNumberGenerator) : base(randomNumberGenerator)
        {
        }

        protected override void GenerateFromSeedData(StudentDataGeneratorContext context)
        {
            context.Student.Name = new Name
            {
                FirstName = context.SeedRecord.FirstName,
                MiddleName = context.SeedRecord.MiddleName,
                LastSurname = context.SeedRecord.LastName
            };
        }

        protected override void GenerateCore(StudentDataGeneratorContext context)
        {
            var sex = context.StudentCharacteristics.Sex;
            var race = context.StudentCharacteristics.Race;
            var isHispanicLatinoEthnicity = context.StudentCharacteristics.HispanicLatinoEthnicity;

            var ethnicityMapping = Configuration.GlobalConfig.EthnicityMappings.MappingFor(race, isHispanicLatinoEthnicity);
          
            context.Student.Name = NameGenerator.Generate(Configuration.NameFileData, RandomNumberGenerator, sex, ethnicityMapping);
        }
    }
}