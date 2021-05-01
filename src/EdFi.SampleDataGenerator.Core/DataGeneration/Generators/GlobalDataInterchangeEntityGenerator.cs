using EdFi.SampleDataGenerator.Core.Config;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common.Entity;

namespace EdFi.SampleDataGenerator.Core.DataGeneration.Generators
{
    public abstract class GlobalDataInterchangeEntityGenerator : InterchangeEntityGenerator<GlobalDataGeneratorContext, GlobalDataGeneratorConfig>
    {
        protected GlobalDataInterchangeEntityGenerator(IRandomNumberGenerator randomNumberGenerator) : base(randomNumberGenerator)
        {
        }

        //Prevent inheritors from interfering with the base implementation.
        public sealed override void Generate(GlobalDataGeneratorContext context)
        {
            base.Generate(context);
        }

        public virtual void GenerateAdditiveData(GlobalDataGeneratorContext context, IDataPeriod dataPeriod)
        {
            //This method exists for child classes to override, and should remain empty.
        }
    }
}