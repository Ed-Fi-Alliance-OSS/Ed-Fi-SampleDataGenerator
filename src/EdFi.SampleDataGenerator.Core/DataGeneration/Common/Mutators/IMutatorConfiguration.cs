namespace EdFi.SampleDataGenerator.Core.DataGeneration.Common.Mutators
{
    public interface IMutatorConfiguration
    {
        double GetMutationProbability(string mutatorName);
    }
}