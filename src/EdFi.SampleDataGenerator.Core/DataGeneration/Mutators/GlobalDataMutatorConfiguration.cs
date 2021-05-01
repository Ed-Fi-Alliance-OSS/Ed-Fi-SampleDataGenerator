using EdFi.SampleDataGenerator.Core.Config;
using EdFi.SampleDataGenerator.Core.DataGeneration.Generators;
using IMutatorConfiguration = EdFi.SampleDataGenerator.Core.DataGeneration.Common.Mutators.IMutatorConfiguration;

namespace EdFi.SampleDataGenerator.Core.DataGeneration.Mutators
{
    public class GlobalDataMutatorConfiguration : IMutatorConfiguration
    {
        public GlobalDataGeneratorConfig GlobalConfig { get; set; }

        public double GetMutationProbability(string mutatorName)
        {
            return GlobalConfig.GlobalConfig.GetMutationProbability(mutatorName);
        }
    }
}