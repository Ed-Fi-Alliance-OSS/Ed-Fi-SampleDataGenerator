using System.Collections.Generic;
using EdFi.SampleDataGenerator.Core.Config;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common.Attributes;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common.Entity;
using EdFi.SampleDataGenerator.Core.DataGeneration.Generators;
using EdFi.SampleDataGenerator.Core.DataGeneration.InterchangeEntities;
using EdFi.SampleDataGenerator.Core.DataGeneration.Mutators;

namespace EdFi.SampleDataGenerator.Core.DataGeneration.Common.Mutators
{
    public abstract class GlobalMutator : IGlobalMutator
    {
        public abstract InterchangeEntity InterchangeEntity { get; }
        public abstract IEntity Entity { get; }
        public abstract IEntityField EntityField { get; }
        public abstract string Name { get; }
        public abstract MutationType MutationType { get; }

        protected IDataPeriod DataPeriod { get; set; }
        protected GlobalDataMutatorConfiguration Configuration { get; set; }
        protected IRandomNumberGenerator RandomNumberGenerator { get; set; }

        protected GlobalMutator(IRandomNumberGenerator randomNumberGenerator)
        {
            RandomNumberGenerator = randomNumberGenerator;
        }

        public void Configure(GlobalDataMutatorConfiguration config)
        {
            Configuration = config;
        }

        protected List<T> SelectItemsToMutate<T>(IEnumerable<T> allItems)
        {
            return allItems.SelectItemsWithProbability(RandomNumberGenerator, Configuration.GetMutationProbability(Name));
        }

        public IEnumerable<MutationResult> MutateData(GlobalDataGeneratorContext context, IDataPeriod dataPeriod)
        {
            DataPeriod = dataPeriod;
            return MutateCore(context);
        }

        protected abstract IEnumerable<MutationResult> MutateCore(GlobalDataGeneratorContext context);
    }
}