using EdFi.MasterScheduleGenerator.Console.Configuration;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common.Entity;
using EdFi.SampleDataGenerator.Core.Serialization.Output.Interchanges;

namespace EdFi.MasterScheduleGenerator.Console.Generators
{
    public abstract class MasterScheduleEntityGenerator : InterchangeEntityGenerator<MasterScheduleData, MasterScheduleGeneratorConfig>
    {
        protected MasterScheduleEntityGenerator(IRandomNumberGenerator randomNumberGenerator) : base(randomNumberGenerator)
        {
        }

        //Prevent inheritors from interfering with the base implementation.
        public sealed override void Generate(MasterScheduleData context)
        {
            base.Generate(context);
        }
    }
}