namespace EdFi.SampleDataGenerator.Core.DataGeneration.Common
{
    public enum OutputMode
    {
        Standard,
        Seed,
    }

    public interface IHasOutputMode
    {
        OutputMode OutputMode { get; }
    }
}
