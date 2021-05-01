using System.Linq;
using EdFi.SampleDataGenerator.Core.DataGeneration.Generators;
using EdFi.SampleDataGenerator.Core.DataGeneration.Generators.Student;
using EdFi.SampleDataGenerator.Core.DataGeneration.Generators.Student.Attributes;
using EdFi.SampleDataGenerator.Core.Entities;
using EdFi.SampleDataGenerator.Core.UnitTests.Config;
using EdFi.SampleDataGenerator.Core.UnitTests.DataGeneration.Common;
using NUnit.Framework;
using Shouldly;

namespace EdFi.SampleDataGenerator.Core.UnitTests.DataGeneration.Generators.Student.Attributes
{
    [TestFixture]
    public class RaceGeneratorTester : StudentAttributeGeneratorTestBase
    {
        [Test]
        public void ShouldSetWhiteRaceType()
        {
            ValidateGeneratedEthnicity("White", RaceDescriptor.White, OldEthnicityDescriptor.WhiteNotOfHispanicOrigin, false);
        }

        [Test]
        public void ShouldSetHispanicRaceType()
        {
            ValidateGeneratedEthnicity("Hispanic", RaceDescriptor.White, OldEthnicityDescriptor.Hispanic, true);
        }

        [Test]
        public void ShouldSetAsianRaceType()
        {
            ValidateGeneratedEthnicity("Asian", RaceDescriptor.Asian, OldEthnicityDescriptor.AsianOrPacificIslander, false);
        }

        [Test]
        public void ShouldSetBlackRaceType()
        {
            ValidateGeneratedEthnicity("Black", RaceDescriptor.BlackAfricanAmerican, OldEthnicityDescriptor.BlackNotOfHispanicOrigin, false);
        }

        [Test]
        public void ShouldSetAmericanIndianAlaskanNativeRaceType()
        {
            ValidateGeneratedEthnicity("AmericanIndianAlaskanNative", RaceDescriptor.AmericanIndianAlaskaNative, OldEthnicityDescriptor.AmericanIndianOrAlaskanNative, false);
        }

        [Test]
        public void ShouldSetNativeHawaiinPacificIslanderRaceType()
        {
            ValidateGeneratedEthnicity("NativeHawaiinPacificIslander", RaceDescriptor.NativeHawaiianPacificIslander, OldEthnicityDescriptor.AsianOrPacificIslander, false);
        }
        

        private void ValidateGeneratedEthnicity(string inputRace, RaceDescriptor expectedRaceType, OldEthnicityDescriptor expectedOldEthnicityType, bool expectedHispanicLatinoEthnicity)
        {
            var randomNumberGenerator = new TestRandomNumberGenerator { RandomDoubleSequence = new[] { 0.01 } };
            var context = DefaultStudentEntityAttributeGenerationContext;

            var config = GetGeneratorConfig(inputRace);

            var raceGenerator = new RaceGenerator(randomNumberGenerator);
            raceGenerator.Configure(config);

            raceGenerator.Generate(context);

            context.StudentCharacteristics.Race.ShouldBe(expectedRaceType);
            context.StudentCharacteristics.OldEthnicity.ShouldBe(expectedOldEthnicityType);
            context.StudentCharacteristics.HispanicLatinoEthnicity.ShouldBe(expectedHispanicLatinoEthnicity);
        }

        private static StudentDataGeneratorConfig GetGeneratorConfig(string race)
        {
            return new StudentDataGeneratorConfig
            {
                GlobalConfig = new TestSampleDataGeneratorConfig
                {
                    EthnicityMappings = TestEthnicityMapping.Defaults
                },
                StudentProfile = new TestStudentProfile
                {
                    RaceConfiguration = new TestAttributeConfiguration
                    {
                        Name = "Race",
                        AttributeGeneratorConfigurationOptions = new[]
                        {
                            new TestAttributeGeneratorConfigurationOption
                            {
                                Frequency = 1.00,
                                Value = race
                            }
                        }
                    }
                }
            };
        }
    }
}
