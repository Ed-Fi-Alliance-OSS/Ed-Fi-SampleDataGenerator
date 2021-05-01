using EdFi.SampleDataGenerator.Core.DataGeneration.Common;
using EdFi.SampleDataGenerator.Core.DataGeneration.InterchangeEntities;

namespace EdFi.SampleDataGenerator.Core.DataGeneration.Generators.Descriptors
{
    public class DescriptorsGenerator : GlobalDataGenerator
    {
        public override InterchangeEntity InterchangeEntity => InterchangeEntity.Descriptors;

        public DescriptorsGenerator() : this(new RandomNumberGenerator())
        {
        }

        public DescriptorsGenerator(IRandomNumberGenerator randomNumberGenerator) : base(randomNumberGenerator, EmptyGeneratorFactory)
        {
        }

        public override void Generate(GlobalDataGeneratorContext context)
        {
            context.GlobalData.Descriptors.AddRange(Configuration.DescriptorFiles);
        }
    }
}
