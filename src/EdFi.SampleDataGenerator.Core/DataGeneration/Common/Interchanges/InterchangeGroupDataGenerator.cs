using System.Collections.Generic;
using System.Linq;

namespace EdFi.SampleDataGenerator.Core.DataGeneration.Common.Interchanges
{
    public interface IInterchangeGroupDataGenerator<in TContext, in TConfig> : IGenerator<TContext, TConfig>
    {
    }

    public abstract class InterchangeGroupDataGenerator<TContext, TConfig> : IInterchangeGroupDataGenerator<TContext, TConfig>
    {
        protected TConfig Configuration { get; private set; }
        protected IRandomNumberGenerator RandomNumberGenerator { get; private set; }

        protected readonly List<IInterchangeDataGenerator<TContext, TConfig>> Generators;

        public delegate IEnumerable<IInterchangeDataGenerator<TContext, TConfig>> GeneratorFactoryDelegate(IRandomNumberGenerator randomNumberGenerator); 

        protected InterchangeGroupDataGenerator(IRandomNumberGenerator randomNumberGenerator, GeneratorFactoryDelegate generatorFactory)
        {
            RandomNumberGenerator = randomNumberGenerator;
            Generators = generatorFactory?.Invoke(randomNumberGenerator).OrderBy(g => InterchangeOrder.GetDefaultOrdering().Single(o => o.Interchange == g.InterchangeEntity.Interchange).Order).ToList() ?? new List<IInterchangeDataGenerator<TContext, TConfig>>();
        }

        public void Configure(TConfig configuration)
        {
            Configuration = configuration;
            OnConfigureGenerators();
        }

        protected virtual void OnConfigureGenerators()
        {
            foreach (var interchangeDataGenerator in Generators)
            {
                interchangeDataGenerator.Configure(Configuration);
            }
        }

        public virtual void Generate(TContext context)
        {
            foreach (var interchangeDataGenerator in Generators)
            {
                interchangeDataGenerator.Generate(context);
            }
        }

        public virtual void LogStats()
        {
            foreach (var interchangeDataGenerator in Generators)
            {
                interchangeDataGenerator.LogStats();
            }
        }
    }
}