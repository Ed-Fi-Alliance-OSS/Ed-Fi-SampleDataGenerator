using EdFi.SampleDataGenerator.Core.Config.SeedData;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common;
using NUnit.Framework;
using Shouldly;

namespace EdFi.SampleDataGenerator.Core.UnitTests.Config.SeedData
{
    [TestFixture]
    public class SeedDataSerializationServiceTester
    {
        [Test]
        public void ShouldReturnEmptyListOnEmptyPath()
        {
            var config = new TestSampleDataGeneratorConfig();
            var seedSerializationService = new SeedDataSerializationService();
            var result = seedSerializationService.Read(config);

            result.Count.ShouldBe(0);
        }

        [Test]
        public void ShouldReturnEmptyListOnSeedOutputMode()
        {
            var config = new TestSampleDataGeneratorConfig
            {
                OutputMode = OutputMode.Seed,
                SeedFilePath = "C:\\Test.csv"
            };
            var seedSerializationService = new SeedDataSerializationService();
            var result = seedSerializationService.Read(config);

            result.Count.ShouldBe(0);
        }
    }
}
