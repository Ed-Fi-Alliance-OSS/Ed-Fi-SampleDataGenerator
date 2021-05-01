using EdFi.SampleDataGenerator.Core.Config;

namespace EdFi.SampleDataGenerator.Core.UnitTests.Config
{
    public class TestSurnameFileMapping : ISurnameFileMapping
    {
        public string FilePath { get; set; }
        public string Ethnicity { get; set; }
    }
}