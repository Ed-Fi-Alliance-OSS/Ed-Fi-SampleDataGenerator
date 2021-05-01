using EdFi.SampleDataGenerator.Core.Config;

namespace EdFi.SampleDataGenerator.Core.UnitTests.Config
{
    public class TestMutatorConfiguration : IMutatorConfiguration
    {
        public double Probability { get; set; }
        public string Name { get; set; }
    }
    public class TestMutatorConfigurationCollection : IMutatorConfigurationCollection
    {
        public IMutatorConfiguration[] Mutators { get; set; }

        public static TestMutatorConfigurationCollection Default => new TestMutatorConfigurationCollection
        {
            Mutators = new IMutatorConfiguration[0]
        };
    }
}
