using System.Collections.Generic;
using EdFi.SampleDataGenerator.Core.Config;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common.Attributes;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common.Entity;
using EdFi.SampleDataGenerator.Core.DataGeneration.Generators;
using EdFi.SampleDataGenerator.Core.DataGeneration.InterchangeEntities;
using EdFi.SampleDataGenerator.Core.DataGeneration.Mutators;

namespace EdFi.SampleDataGenerator.Core.DataGeneration.Common.Mutators
{
    public interface IGlobalMutator
    {
        InterchangeEntity InterchangeEntity { get; }
        IEntity Entity { get; }
        IEntityField EntityField { get; }
        string Name { get; }
        MutationType MutationType { get; }

        void Configure(GlobalDataMutatorConfiguration config);
        IEnumerable<MutationResult> MutateData(GlobalDataGeneratorContext context, IDataPeriod dataPeriod);
    }
}