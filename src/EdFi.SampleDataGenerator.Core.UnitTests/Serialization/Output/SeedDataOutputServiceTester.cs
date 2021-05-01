using System;
using System.Collections.Generic;
using EdFi.SampleDataGenerator.Core.Config;
using EdFi.SampleDataGenerator.Core.Config.SeedData;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common;
using EdFi.SampleDataGenerator.Core.Entities;
using EdFi.SampleDataGenerator.Core.Serialization.Output;
using EdFi.SampleDataGenerator.Core.UnitTests.Config;
using Moq;
using NUnit.Framework;

namespace EdFi.SampleDataGenerator.Core.UnitTests.Serialization.Output
{
    [TestFixture]
    public class SeedDataOutputServiceTester
    {
        [Test]
        public void ShouldSerializeWhenFlushIsCalled()
        {
            var seedDataSerializationService = new Mock<ISeedDataSerializationService>();
            seedDataSerializationService.Setup(x => x.Write(It.IsAny<ISampleDataGeneratorConfig>(), It.IsAny<IEnumerable<SeedRecord>>()));

            var seedOutputService = new SeedDataOutputService(seedDataSerializationService.Object);
            var configuration = new TestSampleDataGeneratorConfig
            {
                OutputMode = OutputMode.Seed,
                SeedFilePath = "C:\\Test.csv"
            };

            var seedRecord = CreateSeedRecord();

            seedOutputService.Configure(configuration);
            seedOutputService.WriteToOutput(seedRecord);
            seedDataSerializationService.Verify(x => x.Write(It.IsAny<ISampleDataGeneratorConfig>(), It.IsAny<IEnumerable<SeedRecord>>()), Times.Never);
            
            seedOutputService.FlushOutput();
            seedDataSerializationService.Verify(x => x.Write(It.IsAny<ISampleDataGeneratorConfig>(), It.IsAny<IEnumerable<SeedRecord>>()), Times.Once);
        }

        private static SeedRecord CreateSeedRecord()
        {
            return new SeedRecord
            {
                BirthDate = new DateTime(2012, 03, 08),
                FirstName = "John",
                MiddleName = "R",
                LastName = "Smith",
                Gender = SexDescriptor.Male,
                GradeLevel = GradeLevelDescriptor.SeventhGrade,
                HispanicLatinoEthnicity = false,
                PerformanceIndex = 0.5,
                Race = RaceDescriptor.White,
                SchoolId = 1
            };
        }
    }
}
