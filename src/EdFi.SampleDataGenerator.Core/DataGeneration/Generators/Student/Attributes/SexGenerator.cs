using EdFi.SampleDataGenerator.Core.Config;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common.Attributes;

namespace EdFi.SampleDataGenerator.Core.DataGeneration.Generators.Student.Attributes
{
    public class SexGenerator : SampleDataEntityAttributeGeneratorBase<StudentDataGeneratorContext, StudentDataGeneratorConfig>
    {
        public override IEntityField GeneratesField => StudentField.Sex;
        public override IEntityField[] DependsOnFields => NoDependencies;

        public SexGenerator(IRandomNumberGenerator randomNumberGenerator) : base(randomNumberGenerator)
        {
        }

        protected override void GenerateFromSeedData(StudentDataGeneratorContext context)
        {
            context.StudentCharacteristics.Sex = context.SeedRecord.Gender;
        }

        protected override void GenerateCore(StudentDataGeneratorContext context)
        {
            var option = Configuration.StudentProfile.SexConfiguration.GetRandomItem(RandomNumberGenerator);
            context.StudentCharacteristics.Sex = Configuration.GlobalConfig.GenderMappings.SexDescriptorFor(option.Value);

            LogStat(option);
        }
    }
}