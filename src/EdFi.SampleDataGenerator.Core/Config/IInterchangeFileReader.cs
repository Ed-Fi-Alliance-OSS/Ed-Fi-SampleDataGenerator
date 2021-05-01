namespace EdFi.SampleDataGenerator.Core.Config
{
    public interface IInterchangeFileReader<out TResult>
    {
        TResult Read(ISampleDataGeneratorConfig config);
    }
}
