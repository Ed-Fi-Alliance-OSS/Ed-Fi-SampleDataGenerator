namespace EdFi.SampleDataGenerator.Core.Config
{
    public interface IStudentPopulationProfile
    {
        string StudentProfileReference { get; }
        int InitialStudentCount { get; }
        int TransfersIn { get; }
        int TransfersOut { get; }
    }
}