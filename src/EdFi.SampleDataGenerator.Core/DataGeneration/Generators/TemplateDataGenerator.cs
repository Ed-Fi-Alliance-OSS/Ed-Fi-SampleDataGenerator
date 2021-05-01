using EdFi.SampleDataGenerator.Core.DataGeneration.Common;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common.Interchanges;

namespace EdFi.SampleDataGenerator.Core.DataGeneration.Generators
{
    public abstract class TemplateDataGenerator : InterchangeDataGenerator<GlobalDataGeneratorConfig, GlobalDataGeneratorConfig>
    {
        protected TemplateDataGenerator(IRandomNumberGenerator randomNumberGenerator, GeneratorFactoryDelegate generatorFactory) : base(randomNumberGenerator, generatorFactory)
        {
        }
    }
}
