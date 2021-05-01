using System.Collections.Generic;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common.Entity;
using System.Linq;
using EdFi.SampleDataGenerator.Core.DataGeneration.InterchangeEntities;

namespace EdFi.SampleDataGenerator.Core.DataGeneration.Common.Interchanges
{
    public interface IInterchangeDataGenerator<in TContext, in TConfig> : IGenerator<TContext, TConfig>
    {
        InterchangeEntity InterchangeEntity { get; }
        Interchange[] DependsOnInterchanges { get; }
        IEntity[] GeneratesEntities { get; }
        IEntity[] DependsOnEntities { get; }
    }

    public abstract class InterchangeDataGenerator<TContext, TConfig> : IInterchangeDataGenerator<TContext, TConfig>
    {
        public abstract InterchangeEntity InterchangeEntity { get; }
        public Interchange[] DependsOnInterchanges => DependsOnEntities.Select(e => e.Interchange).Distinct().ToArray();
        public virtual IEntity[] GeneratesEntities => Generators.Select(g => g.GeneratesEntity).ToArray();
        public virtual IEntity[] DependsOnEntities => Generators.SelectMany(g => g.DependsOnEntities)
            .Except(GeneratesEntities)
            .Distinct()
            .ToArray();

        protected readonly List<IInterchangeEntityGenerator<TContext, TConfig>> Generators;

        public delegate IEnumerable<IInterchangeEntityGenerator<TContext, TConfig>> GeneratorFactoryDelegate(IRandomNumberGenerator randomNumberGenerator);

        protected static IEnumerable<IInterchangeEntityGenerator<TContext, TConfig>> DefaultGeneratorFactory<TGeneratorBase>(IRandomNumberGenerator randomNumberGenerator)
            where TGeneratorBase : IInterchangeEntityGenerator<TContext, TConfig>
        {
            return InterchangeEntityGeneratorFactory.DefaultGeneratorFactory<TContext, TConfig, TGeneratorBase>(randomNumberGenerator);
        }

        protected static GeneratorFactoryDelegate EmptyGeneratorFactory => rng => Enumerable.Empty<IInterchangeEntityGenerator<TContext, TConfig>>();

        protected IRandomNumberGenerator RandomNumberGenerator { get; private set; }
        protected TConfig Configuration { get; private set; }

        protected InterchangeDataGenerator(IRandomNumberGenerator randomNumberGenerator, GeneratorFactoryDelegate generatorFactory)
        {
            RandomNumberGenerator = randomNumberGenerator;
            Generators = generatorFactory?.Invoke(randomNumberGenerator).SortByDependencies() ?? new List<IInterchangeEntityGenerator<TContext, TConfig>>();
        }

        public void Configure(TConfig configuration)
        {
            Configuration = configuration;
            OnConfigure();
        }
        
        protected virtual void OnConfigure()
        {
            foreach (var interchangeEntityGenerator in Generators)
            {
                interchangeEntityGenerator.Configure(Configuration);
            }
        }

        public virtual void Generate(TContext context)
        {
            foreach (var interchangeEntityGenerator in Generators)
            {
                interchangeEntityGenerator.Generate(context);
            }
        }

        public virtual void LogStats()
        {
            foreach (var interchangeEntityGenerator in Generators)
            {
                interchangeEntityGenerator.LogStats();
            }
        }
    }
}
