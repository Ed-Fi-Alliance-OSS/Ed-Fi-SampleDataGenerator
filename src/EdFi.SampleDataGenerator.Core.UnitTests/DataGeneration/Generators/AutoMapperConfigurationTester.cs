using AutoMapper;
using EdFi.SampleDataGenerator.Core.AutoMapper;
using NUnit.Framework;

namespace EdFi.SampleDataGenerator.Core.UnitTests.DataGeneration.Generators
{
    [TestFixture]
    public class AutoMapperConfigurationTester
    {
        [Test]
        public void MappingConfiguratioShouldBeValid()
        {
            new MapperConfiguration(cfg => cfg.AddProfile<AutoMapperCoreProfile>())
                .AssertConfigurationIsValid();
        }
    }
}