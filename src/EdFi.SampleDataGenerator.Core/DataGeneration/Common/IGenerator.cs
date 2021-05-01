namespace EdFi.SampleDataGenerator.Core.DataGeneration.Common
{
    public interface IGenerator<in TContext, in TConfiguration>
    {
        void Configure(TConfiguration config);
        void LogStats();
        void Generate(TContext context);
    }
}
