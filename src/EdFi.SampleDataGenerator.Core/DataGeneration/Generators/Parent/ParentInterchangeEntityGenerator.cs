using EdFi.SampleDataGenerator.Core.DataGeneration.Common;

namespace EdFi.SampleDataGenerator.Core.DataGeneration.Generators.Parent
{
    public abstract class ParentInterchangeEntityGenerator : StudentDataInterchangeEntityGenerator
    {
        protected ParentInterchangeEntityGenerator(IRandomNumberGenerator randomNumberGenerator) : base(randomNumberGenerator)
        {
        }
    }
}
