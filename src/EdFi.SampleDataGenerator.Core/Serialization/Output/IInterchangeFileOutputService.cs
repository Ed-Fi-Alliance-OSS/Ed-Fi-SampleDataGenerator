namespace EdFi.SampleDataGenerator.Core.Serialization.Output
{
    public interface IInterchangeFileOutputService
    {
        void WriteOutputToFile<TInterchangeEntity>(string outputFilePath, TInterchangeEntity interchangeEntity);
        void WriteManifestToFile(string outputFilePath, Manifest manifest);
    }
}