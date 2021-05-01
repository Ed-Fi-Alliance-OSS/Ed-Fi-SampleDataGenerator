using EdFi.SampleDataGenerator.Core.DataGeneration.Generators;

namespace EdFi.SampleDataGenerator.Core.DataGeneration.Common.Attributes
{
    public abstract class GlobalAttributeGenerator : EntityAttributeGenerator<GlobalDataGeneratorContext, GlobalDataGeneratorConfig>
    {
        protected GlobalAttributeGenerator(IRandomNumberGenerator randomNumberGenerator) : base(randomNumberGenerator)
        {
        }
    }
}