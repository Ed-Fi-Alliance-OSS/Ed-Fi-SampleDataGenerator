using EdFi.SampleDataGenerator.Core.Config;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common.Attributes;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common.Entity;
using EdFi.SampleDataGenerator.Core.DataGeneration.Generators;
using EdFi.SampleDataGenerator.Core.DataGeneration.InterchangeEntities;
using EdFi.SampleDataGenerator.Core.DataGeneration.Mutators;

namespace EdFi.SampleDataGenerator.Core.DataGeneration.Common.Mutators
{
    public abstract class StudentMutator : IStudentMutator
    {
        public abstract InterchangeEntity InterchangeEntity { get; }
        public abstract IEntity Entity { get; }
        public abstract IEntityField EntityField { get; }
        public abstract string Name { get; }
        public abstract MutationType MutationType { get; }

        protected IDataPeriod DataPeriod { get; set; }
        protected StudentDataMutatorConfiguration Configuration { get; set; }
        protected IRandomNumberGenerator RandomNumberGenerator { get; set; }

        protected StudentMutator(IRandomNumberGenerator randomNumberGenerator)
        {
            RandomNumberGenerator = randomNumberGenerator;
        }

        public void Configure(StudentDataMutatorConfiguration config)
        {
            Configuration = config;
        }

        protected virtual bool ShouldMutate()
        {
            var probability = Configuration.GetMutationProbability(Name);
            return probability > 0.0 && RandomNumberGenerator.GetRandomBool(probability);
        }

        public MutationResult MutateData(StudentDataGeneratorContext context, IDataPeriod dataPeriod)
        {
            DataPeriod = dataPeriod;
            return ShouldMutate()
                ? MutateCore(context)
                : MutationResult.NoMutation;
        }

        protected abstract MutationResult MutateCore(StudentDataGeneratorContext context);
    }
}