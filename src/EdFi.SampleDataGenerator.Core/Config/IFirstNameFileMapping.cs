namespace EdFi.SampleDataGenerator.Core.Config
{
    public interface IFirstNameFileMapping : IFileMapping
    {
        string Ethnicity { get; }
        string Gender { get; }
    }
}