using System.Reflection;
using EdFi.MasterScheduleGenerator.Console.Configuration;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common.Entity;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common.Interchanges;
using EdFi.SampleDataGenerator.Core.DataGeneration.InterchangeEntities;
using EdFi.SampleDataGenerator.Core.Serialization.Output.Interchanges;

namespace EdFi.MasterScheduleGenerator.Console.Generators
{
    public class MasterScheduleInterchangeGenerator : InterchangeDataGenerator<MasterScheduleData, MasterScheduleGeneratorConfig>
    {
        private static readonly GeneratorFactoryDelegate GeneratorFactory = rng => InterchangeEntityGeneratorFactory.DefaultGeneratorFactory<MasterScheduleData, MasterScheduleGeneratorConfig, MasterScheduleEntityGenerator>(rng, Assembly.GetExecutingAssembly());

        public MasterScheduleInterchangeGenerator() : this(new RandomNumberGenerator(), GeneratorFactory)
        {
        }

        public MasterScheduleInterchangeGenerator(IRandomNumberGenerator randomNumberGenerator, GeneratorFactoryDelegate generatorFactory) : base(randomNumberGenerator, generatorFactory)
        {
        }

        public override InterchangeEntity InterchangeEntity => InterchangeEntity.MasterSchedule;
    }
}
