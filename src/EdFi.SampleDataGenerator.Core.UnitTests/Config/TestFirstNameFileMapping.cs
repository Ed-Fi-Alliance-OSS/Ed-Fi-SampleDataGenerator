using EdFi.SampleDataGenerator.Core.Config;

namespace EdFi.SampleDataGenerator.Core.UnitTests.Config
{
    public class TestFirstNameFileMapping : IFirstNameFileMapping
    {
        public string Ethnicity { get; set; }
        public string Gender { get; set; }
        public string FilePath { get; set; }
    }
}