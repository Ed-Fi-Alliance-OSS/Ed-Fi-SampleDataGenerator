using EdFi.SampleDataGenerator.Core.Config;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common.Attributes;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common.Entity;
using EdFi.SampleDataGenerator.Core.DataGeneration.Generators;
using EdFi.SampleDataGenerator.Core.DataGeneration.InterchangeEntities;
using EdFi.SampleDataGenerator.Core.DataGeneration.Mutators;

namespace EdFi.SampleDataGenerator.Core.DataGeneration.Common.Mutators
{
    public interface IStudentMutator
    {
        InterchangeEntity InterchangeEntity { get; }
        IEntity Entity { get; }
        IEntityField EntityField { get; }
        string Name { get; }
        MutationType MutationType { get; }

        void Configure(StudentDataMutatorConfiguration config);
        MutationResult MutateData(StudentDataGeneratorContext context, IDataPeriod dataPeriod);
    }
}