using System.Collections.Generic;
using System.Linq;
using EdFi.SampleDataGenerator.Core.Config;
using log4net;

namespace EdFi.SampleDataGenerator.Core.DataGeneration.Common.Attributes
{
    public abstract class EntityAttributeGenerator<TContext, TConfig> : IEntityAttributeGenerator<TContext, TConfig>
    {
        private static readonly ILog _logger = LogManager.GetLogger(typeof(EntityAttributeGenerator<,>));

        protected IRandomNumberGenerator RandomNumberGenerator { get; private set; }
        private readonly Dictionary<string, int> _generationStats = new Dictionary<string, int>();

        protected EntityAttributeGenerator(IRandomNumberGenerator randomNumberGenerator)
        {
            RandomNumberGenerator = randomNumberGenerator;
        }

        public string FullyQualifiedFieldName => GeneratesField.FullyQualifiedFieldName;
        public string FieldName => GeneratesField.FieldName;

        public abstract IEntityField GeneratesField { get; }

        protected static IEntityField[] NoDependencies => Enumerable.Empty<IEntityField>().ToArray();
        public abstract IEntityField[] DependsOnFields { get; }

        public TConfig Configuration { get; private set; }

        public void Configure(TConfig configuration)
        {
            Configuration = configuration;
            OnConfigure();
        }

        public virtual void Generate(TContext context)
        {
            GenerateCore(context);
        }

        protected abstract void GenerateCore(TContext context);

        protected virtual void OnConfigure()
        {
        }

        protected void LogStat(string generatedValue)
        {
            if (_generationStats.ContainsKey(generatedValue))
            {
                _generationStats[generatedValue] += 1;
            }

            else
            {
                _generationStats[generatedValue] = 1;
            }
        }

        protected void LogStat(IAttributeGeneratorConfigurationOption option)
        {
            LogStat(option.Value);
        }

        public virtual void LogStats()
        {
            if (_generationStats.Keys.Count == 0)
                return;

            var statsList = _generationStats.ToList().OrderBy(i => i.Key);

            var header = string.Join(",", statsList.Select(i => i.Key));
            var values = string.Join(",", statsList.Select(i => i.Value.ToString()));

            _logger.Debug($"{GeneratesField.FullyQualifiedFieldName}: {header}");
            _logger.Debug(values);

            _generationStats.Clear();
        }
    }
}