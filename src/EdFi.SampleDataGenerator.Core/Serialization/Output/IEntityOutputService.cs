namespace EdFi.SampleDataGenerator.Core.Serialization.Output
{
    public interface IEntityOutputService<in TRecord, in TConfiguration>
    {
        void Configure(TConfiguration configuration);
        void WriteToOutput(TRecord record);
    }
}
