using System;
using EdFi.SampleDataGenerator.Core.Config.SeedData;

namespace EdFi.SampleDataGenerator.Core.DataGeneration.Common.Attributes
{
    public abstract class SampleDataEntityAttributeGeneratorBase<TContext, TConfig> : EntityAttributeGenerator<TContext, TConfig>
        where TConfig : IHasOutputMode
        where TContext : IHasSeedRecord
    {
        protected SampleDataEntityAttributeGeneratorBase(IRandomNumberGenerator randomNumberGenerator) : base(randomNumberGenerator)
        {
        }

        public sealed override void Generate(TContext context)
        {
            if (Configuration == null)
                throw new InvalidOperationException("The Configure method must be called with a non-null value before this generator can run");

            if (context.HasSeedRecord)
            {
                GenerateFromSeedData(context);
            }

            else
            {
                GenerateCore(context);
            }
        }

        protected virtual void GenerateFromSeedData(TContext context)
        {
            GenerateCore(context);
        }
    }
}