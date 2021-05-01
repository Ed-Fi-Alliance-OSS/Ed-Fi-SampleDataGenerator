namespace EdFi.SampleDataGenerator.Core.Config
{
    public interface IDescriptorFileMapping : IFileMapping
    {
        string DescriptorName { get; set; }
    }
}