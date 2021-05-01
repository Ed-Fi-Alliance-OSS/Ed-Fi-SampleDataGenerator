using System;
using EdFi.SampleDataGenerator.Core.Config;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common.Entity;

namespace EdFi.SampleDataGenerator.Core.DataGeneration.Generators
{
    public abstract class StudentDataInterchangeEntityGenerator : InterchangeEntityGenerator<StudentDataGeneratorContext, StudentDataGeneratorConfig>
    {
        protected StudentDataInterchangeEntityGenerator(IRandomNumberGenerator randomNumberGenerator) : base(randomNumberGenerator)
        {
        }

        public sealed override void Generate(StudentDataGeneratorContext context)
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

            RunChildGenerators(context);
        }

        protected virtual void GenerateFromSeedData(StudentDataGeneratorContext context)
        {
            GenerateCore(context);
        }

        public virtual void GenerateAdditiveData(StudentDataGeneratorContext context, IDataPeriod dataPeriod)
        {
            //This method exists for child classes to override, and should remain empty.
        }
    }
}
