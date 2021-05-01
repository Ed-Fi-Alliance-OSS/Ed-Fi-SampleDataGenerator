using System.Linq;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common.Interchanges;
using EdFi.SampleDataGenerator.Core.DataGeneration.Generators;

namespace EdFi.SampleDataGenerator.Core.DataGeneration.Coordination
{
    public class TemplatedDataGenerationCoordinator : InterchangeGroupDataGenerator<GlobalDataGeneratorConfig, GlobalDataGeneratorConfig>
    {
        private static GeneratorFactoryDelegate GeneratorFactory => 
            rng => InterchangeDataGeneratorFactory.GetAllTemplateDataGenerators(rng)
                        .OrderBy(g => InterchangeOrder.GetDefaultOrdering().Single(o => o.Interchange == g.InterchangeEntity.Interchange).Order);

        public TemplatedDataGenerationCoordinator() : this(new RandomNumberGenerator())
        {
        }

        public TemplatedDataGenerationCoordinator(IRandomNumberGenerator randomNumberGenerator) : base(randomNumberGenerator, GeneratorFactory)
        {
        }

        public void Run(GlobalDataGeneratorConfig config)
        {
            Configure(config);

            foreach (var generator in Generators)
            {
                generator.Generate(config);
            }
        }
    }
}
