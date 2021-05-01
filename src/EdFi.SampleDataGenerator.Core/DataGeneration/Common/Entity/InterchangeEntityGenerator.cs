using System.Collections.Generic;
using System.Linq;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common.Attributes;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common.Interchanges;

namespace EdFi.SampleDataGenerator.Core.DataGeneration.Common.Entity
{
    public interface IInterchangeEntityGenerator<in TContext, in TConfig> : IGenerator<TContext, TConfig>
    {
        Interchange Interchange { get; }
        IEntity GeneratesEntity { get; }
        IEntity[] DependsOnEntities { get; }
        IEntity[] InternalEntityDependencies { get; }
        IEntity[] ExternalEntityDependencies { get; }
    }

    public abstract class InterchangeEntityGenerator<TContext, TConfig> : IInterchangeEntityGenerator<TContext, TConfig>
    {
        public Interchange Interchange => GeneratesEntity.Interchange;
        public abstract IEntity GeneratesEntity { get; }
        public abstract IEntity[] DependsOnEntities { get; }
        public IEntity[] InternalEntityDependencies => DependsOnEntities.Where(e => e.Interchange == Interchange).ToArray();
        public IEntity[] ExternalEntityDependencies => DependsOnEntities.Where(e => e.Interchange != Interchange).ToArray();

        private List<IEntityAttributeGenerator<TContext, TConfig>> Generators { get; }

        private IEnumerable<IEntityAttributeGenerator<TContext, TConfig>> ScanningGeneratorFactory(IRandomNumberGenerator randomNumberGenerator)
        {
            var entityBeingGenerated = GeneratesEntity;

            return EntityAttributeGeneratorFactory.BuildAllAttributeGenerators<TContext, TConfig>(randomNumberGenerator)
                .Where(x => x.GeneratesField.Entity == entityBeingGenerated)
                .ToArray();
        }

        protected IRandomNumberGenerator RandomNumberGenerator { get; }
        protected TConfig Configuration { get; private set; }

        protected InterchangeEntityGenerator(IRandomNumberGenerator randomNumberGenerator)
        {
            RandomNumberGenerator = randomNumberGenerator;
            Generators = ScanningGeneratorFactory(randomNumberGenerator).SortByDependencies().ToList();
        }

        public void Configure(TConfig configuration)
        {
            Configuration = configuration;
            OnConfigure();
            ConfigureChildGenerators();
        }

        protected virtual void OnConfigure()
        {
            //This method exists for child classes to override, and should remain empty.
        }

        private void ConfigureChildGenerators()
        {
            foreach (var entityAttributeGenerator in Generators)
            {
                entityAttributeGenerator.Configure(Configuration);
            }
        }

        public virtual void Generate(TContext context)
        {
            GenerateCore(context);
            RunChildGenerators(context);
        }

        protected virtual void GenerateCore(TContext context)
        {
            //This method exists for child classes to override, and should remain empty.
        }

        protected void RunChildGenerators(TContext context)
        {
            foreach (var entityAttributeGenerator in Generators)
            {
                entityAttributeGenerator.Generate(context);
            }
        }

        public void LogStats()
        {
            foreach (var entityAttributeGenerator in Generators)
            {
                entityAttributeGenerator.LogStats();
            }

            OnLogStats();
        }

        protected virtual void OnLogStats()
        {
            //This method exists for child classes to override, and should remain empty.
        }
    }
}