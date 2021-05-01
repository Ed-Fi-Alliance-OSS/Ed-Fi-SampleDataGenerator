using EdFi.SampleDataGenerator.Core.Config;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common.Dependencies;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common.Entity;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common.Interchanges;

namespace EdFi.SampleDataGenerator.Core.DataGeneration.Generators
{
    public abstract class GlobalDataGenerator : InterchangeDataGenerator<GlobalDataGeneratorContext, GlobalDataGeneratorConfig>
    {
        protected GlobalDataGenerator(IRandomNumberGenerator randomNumberGenerator, GeneratorFactoryDelegate generatorFactory) : base(randomNumberGenerator, generatorFactory)
        {
        }

        public override IEntity[] GeneratesEntities => InterchangeEntity.Entities;
        public override IEntity[] DependsOnEntities => EntityDependencies.None;

        public void GenerateAdditiveData(GlobalDataGeneratorContext context, IDataPeriod dataPeriod)
        {
            foreach (var interchangeDataGenerator in Generators)
            {
                var additiveGenerator = interchangeDataGenerator as GlobalDataInterchangeEntityGenerator;
                additiveGenerator?.GenerateAdditiveData(context, dataPeriod);
            }
        }
    }
}