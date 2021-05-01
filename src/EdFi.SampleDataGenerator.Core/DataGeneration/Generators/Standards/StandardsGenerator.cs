using EdFi.SampleDataGenerator.Core.DataGeneration.Common;
using EdFi.SampleDataGenerator.Core.DataGeneration.InterchangeEntities;

namespace EdFi.SampleDataGenerator.Core.DataGeneration.Generators.Standards
{
    public class StandardsGenerator : GlobalDataGenerator
    {
        public override InterchangeEntity InterchangeEntity => InterchangeEntity.Standards;

        public StandardsGenerator(IRandomNumberGenerator randomNumberGenerator) : base(randomNumberGenerator, EmptyGeneratorFactory)
        {
        }

        public override void Generate(GlobalDataGeneratorContext context)
        {
            context.GlobalData.StandardsData = Configuration.StandardsFileData;
        }
    }
}
