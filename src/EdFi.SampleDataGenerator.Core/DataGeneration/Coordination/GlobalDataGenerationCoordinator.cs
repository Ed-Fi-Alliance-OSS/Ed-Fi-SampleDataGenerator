using System;
using System.Collections.Generic;
using System.Linq;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common.Interchanges;
using EdFi.SampleDataGenerator.Core.DataGeneration.Generators;
using EdFi.SampleDataGenerator.Core.Serialization.Output;
using EdFi.SampleDataGenerator.Core.Config;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common.Mutators;
using EdFi.SampleDataGenerator.Core.DataGeneration.Mutators;

namespace EdFi.SampleDataGenerator.Core.DataGeneration.Coordination
{
    public class GlobalDataGenerationCoordinator : InterchangeGroupDataGenerator<GlobalDataGeneratorContext, GlobalDataGeneratorConfig>
    {
        private readonly GlobalDataOutputService _globalDataOutputService;

        private static GeneratorFactoryDelegate _generatorFactory =>
                rng => InterchangeDataGeneratorFactory.GetAllGlobalDataGenerators(rng)
                        .OrderBy(g => InterchangeOrder.GetDefaultOrdering().Single(o => o.Interchange == g.InterchangeEntity.Interchange).Order);

        private readonly IBufferedEntityOutputService<MutationLogEntry, MutationLogOutputConfiguration> _mutatorLogOutputService;

        private readonly List<IGlobalMutator> _mutators;

        public delegate IEnumerable<IGlobalMutator> MutatorFactoryDelegate(IRandomNumberGenerator randomNumberGenerator);

        public GlobalDataGenerationCoordinator() : this(new RandomNumberGenerator(), new GlobalDataOutputService(),
            new MutationLogOutputService("GlobalMutatorLog.txt"),
            MutatorFactory.GlobalMutatorFactory)
        {
        }

        public GlobalDataGenerationCoordinator(IRandomNumberGenerator randomNumberGenerator,
            GlobalDataOutputService globalDataOutputService,
            IBufferedEntityOutputService<MutationLogEntry, MutationLogOutputConfiguration> mutatorLogOutputService,
            MutatorFactoryDelegate mutatorFactory)
            : base(randomNumberGenerator, _generatorFactory)
        {
            _globalDataOutputService = globalDataOutputService;
            _mutatorLogOutputService = mutatorLogOutputService;

            _mutators = mutatorFactory?.Invoke(RandomNumberGenerator).ToList() ?? new List<IGlobalMutator>();
        }

        public GlobalDataGeneratorContext Run(GlobalDataGeneratorConfig config)
        {
            if (config == null)
            {
                throw new InvalidOperationException("Global Data Generation Coordinator not properly configured");
            }

            Reconfigure(config);

            var dataPeriods = config.GlobalConfig.TimeConfig.DataClockConfig.DataPeriods.OrderBy(dp => dp.StartDate).ToList();

            var context = new GlobalDataGeneratorContext
            {
                GlobalData = new GlobalData()
            };

            Generate(context);

            for (var dataPeriodIndex = 0; dataPeriodIndex < dataPeriods.Count; dataPeriodIndex++)
            {
                var dataPeriod = dataPeriods[dataPeriodIndex];

                GenerateAdditiveData(context, dataPeriod);

                var dataPeriodEndsDuringSchoolCalendar = dataPeriodIndex < dataPeriods.Count - 1;
                if (dataPeriodEndsDuringSchoolCalendar)
                    RunMutators(context, dataPeriod);

                OutputGeneratedData(context, dataPeriod);
            }

            _mutatorLogOutputService.FlushOutput();

            return context;
        }

        private void Reconfigure(GlobalDataGeneratorConfig config)
        {
            _globalDataOutputService.Configure(new GlobalDataOutputConfiguration
            {
                SampleDataGeneratorConfig = config.GlobalConfig
            });

            Configure(config);
            ConfigureMutatorLogService(config.GlobalConfig);
        }

        private void GenerateAdditiveData(GlobalDataGeneratorContext context, IDataPeriod dataPeriod)
        {
            Generators.ForEach(g => (g as GlobalDataGenerator)?.GenerateAdditiveData(context, dataPeriod));
        }

        private void ConfigureMutatorLogService(ISampleDataGeneratorConfig sampleDataGeneratorConfig)
        {
            var mutatorLogConfiguration = new MutationLogOutputConfiguration
            {
                SampleDataGeneratorConfig = sampleDataGeneratorConfig
            };

            _mutatorLogOutputService.Configure(mutatorLogConfiguration);
        }

        private void OutputGeneratedData(GlobalDataGeneratorContext context, IDataPeriod dataPeriod)
        {
            _globalDataOutputService.WriteToOutput(context.GlobalData, dataPeriod);
        }

        private void RunMutators(GlobalDataGeneratorContext context, IDataPeriod dataPeriod)
        {
            _mutators.ForEach(mutator =>
            {
                mutator.Configure(new GlobalDataMutatorConfiguration { GlobalConfig = Configuration });
                var mutationResults = mutator.MutateData(context, dataPeriod);
                foreach (var mutationResult in mutationResults)
                {
                    if (mutationResult.Mutated)
                    {
                        var logEntry = new MutationLogEntry
                        {
                            Attribute = mutator.EntityField?.FullyQualifiedFieldName,
                            Entity = mutator.Entity?.ClassName,
                            Interchange = mutator.InterchangeEntity?.Interchange.Name,
                            EntityKey = null,
                            MutationType = mutator.MutationType,
                            MutatorName = mutator.GetType().Name,
                            NewValue = mutationResult.NewValue,
                            OldValue = mutationResult.OldValue
                        };

                        _mutatorLogOutputService.WriteToOutput(logEntry);
                    }
                }
            });
        }
    }
}