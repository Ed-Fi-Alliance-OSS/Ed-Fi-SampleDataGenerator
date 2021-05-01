using EdFi.SampleDataGenerator.Core.Config.DataFiles;

namespace EdFi.SampleDataGenerator.Core.DataGeneration.Common
{
    public static class NameFileExtensions
    {
        public static NameFileRecord GetRandomRecord(this NameFile nameFile, IRandomNumberGenerator randomNumberGenerator)
        {
            return nameFile.FileRecords.GetRandomItem(randomNumberGenerator);
        }
    }
}
