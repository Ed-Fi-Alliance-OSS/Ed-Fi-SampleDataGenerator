using System.Collections.Generic;
using System.Linq;
using EdFi.SampleDataGenerator.Core.Config;
using EdFi.SampleDataGenerator.Core.DataGeneration.Generators;
using EdFi.SampleDataGenerator.Core.DataGeneration.Generators.Student;
using EdFi.SampleDataGenerator.Core.DataGeneration.Generators.StudentEnrollment;
using EdFi.SampleDataGenerator.Core.Entities;
using EdFi.SampleDataGenerator.Core.Helpers;
using EdFi.SampleDataGenerator.Core.Serialization.Output;
using EdFi.SampleDataGenerator.Core.Serialization.Output.Interchanges;
using EdFi.SampleDataGenerator.Core.UnitTests.Config;
using EdFi.SampleDataGenerator.Core.UnitTests.DataGeneration.Common;
using NUnit.Framework;
using Shouldly;

namespace EdFi.SampleDataGenerator.Core.UnitTests.DataGeneration.Generators.StudentEnrollment
{
    [TestFixture]
    public class StudentEducationOrganizationEntityGeneratorTester : GeneratorTestBase
    {
        private static TestStudentProfile HomelessProfile => new TestStudentProfile
        {
            RaceConfiguration = new TestAttributeConfiguration
            {
                Name = "Race",
                AttributeGeneratorConfigurationOptions = new IAttributeGeneratorConfigurationOption[]
               {
                    new TestAttributeGeneratorConfigurationOption
                    {
                        Frequency = 100,
                        Value = "White"
                    }
               }
            },
            SexConfiguration = new TestAttributeConfiguration
            {
                Name = "Sex",
                AttributeGeneratorConfigurationOptions = new IAttributeGeneratorConfigurationOption[]
               {
                    new TestAttributeGeneratorConfigurationOption
                    {
                        Frequency = 100,
                        Value = "Female"
                    }
               }
            },
            EconomicDisadvantageConfiguration = new TestAttributeConfiguration()
            {
                Name = "EconomicDisadvantage",
                AttributeGeneratorConfigurationOptions = new IAttributeGeneratorConfigurationOption[]
               {
                    new TestAttributeGeneratorConfigurationOption
                    {
                        Frequency = 10,
                        Value = "White"
                    }
               }
            },
            Name = "Test Student Profile",
            HomelessStatusConfiguration = new TestAttributeConfiguration
            {
                Name = "HomelessStatus",
                AttributeGeneratorConfigurationOptions = new[]
               {
                    new TestAttributeGeneratorConfigurationOption
                    {
                        Frequency = 100,
                        Value = "White"
                    }
                }
            }
        };
        private static StudentDataGeneratorContext BuildContext()
        {
            return new StudentDataGeneratorContext
            {
                GlobalStudentNumber = 0,
                GeneratedStudentData = new GeneratedStudentData
                {
                    StudentData = new StudentData
                    {
                        Student = new Entities.Student
                        {
                            Name = new Name
                            {
                                FirstName = "A",
                                LastSurname = "B"
                            }
                        },
                    },
                    StudentTranscriptData = new StudentTranscriptData()
                    {
                        StudentTranscriptSessions = new List<StudentTranscriptSession>
                        {
                            new StudentTranscriptSession
                            {
                                CurrentSchoolYear = true,
                                GradeLevel = GradeLevelDescriptor.TwelfthGrade,
                            }
                        }
                    },
                },
                StudentPerformanceProfile = new StudentPerformanceProfile { PerformanceIndex = 0.5 },
                StudentCharacteristics = new StudentCharacteristics
                {
                    Race = RaceDescriptor.White,
                    HispanicLatinoEthnicity = true,
                    Sex = SexDescriptor.Female,
                    OldEthnicity = OldEthnicityDescriptor.Hispanic,
                    IsImmigrant = true
                }
            };
        }

        private static StudentDataGeneratorConfig GetConfiguration()
        {
            var sampleDataGeneratorConfig = GetSampleDataGeneratorConfig();
            var globalDataGeneratorConfig = GetGlobalDataGeneratorConfig();
            var globalData = new GlobalData
            {
                GraduationPlans = globalDataGeneratorConfig.GraduationPlans,
                EducationOrganizationData = globalDataGeneratorConfig.EducationOrganizationData,
                MasterScheduleData = globalDataGeneratorConfig.MasterScheduleData
            };
            return GetStudentGeneratorConfig(globalData, sampleDataGeneratorConfig, globalDataGeneratorConfig);
        }

        [Test]
        public void ShoulUseStudentCharacteristic()
        {
            var randomNumberGenerator = new TestRandomNumberGenerator { RandomIntSequence = new[] { 0 }, RandomDoubleSequence = new[] { 0.0 } };
            var context = BuildContext();

            var studentEducationGenerator = new StudentEducationOrganizationEntityGenerator(randomNumberGenerator);

            studentEducationGenerator.Configure(GetConfiguration());
            studentEducationGenerator.Generate(context);

            var studentEd = context.GetStudentEducationOrganization();

            studentEd.Race.ShouldNotBeNull();
            studentEd.Race.First().ShouldBe(context.StudentCharacteristics.Race.GetStructuredCodeValue());

            studentEd.OldEthnicity.ShouldNotBeNull();
            studentEd.OldEthnicity.ShouldBe(context.StudentCharacteristics.OldEthnicity.GetStructuredCodeValue());

            studentEd.HispanicLatinoEthnicity.ShouldBe(context.StudentCharacteristics.HispanicLatinoEthnicity);

            var isImmigrant = studentEd.HasCharacteristic(StudentCharacteristicDescriptor.Immigrant);
            isImmigrant.ShouldBe(true);
        }

        [Test]
        public void ShouldGenerateAreaCodeBasedOnCity()
        {
            var randomNumberGenerator = new TestRandomNumberGenerator { RandomIntSequence = new[] { 0 }, RandomDoubleSequence = new[] { 0.0 } };

            var config = GetConfiguration();
            config.DistrictProfile = new TestDistrictProfile
            {
                LocationInfo = new TestLocationInfo
                {
                    Cities = new[]
                    {
                        new TestCity
                        {
                            Name = "Test City Name",
                            AreaCodes = new[]
                            {
                                new TestAreaCode {Value = 555}
                            }
                        },
                        new TestCity
                        {
                            Name = "Other Test City Name",
                            AreaCodes = new[]
                            {
                                new TestAreaCode {Value = 111},
                            }
                        }
                    }
                }
            };

            var context = BuildContext();
            var studentEducationGenerator = new StudentEducationOrganizationEntityGenerator(randomNumberGenerator);
            var configuration = GetConfiguration();
            configuration.StudentProfile = new TestStudentProfile();

            studentEducationGenerator.Configure(configuration);
            studentEducationGenerator.Generate(context);

            var studentEd = context.GetStudentEducationOrganization();

            studentEd.Telephone.Length.ShouldBe(1);

            var telephone = studentEd.Telephone.First();
            telephone.TelephoneNumberType.ShouldBe(TelephoneNumberTypeDescriptor.Mobile.CodeValue);
            telephone.TelephoneNumber.ShouldBe($"(555) 000-0000");
        }

        [Test]
        public void ShouldNotGenerateTelephoneNumberAndAddressIfStudentIsHomeless()
        {
            var randomNumberGenerator = new TestRandomNumberGenerator { RandomIntSequence = new[] { 0 }, RandomDoubleSequence = new[] { 0.0 } };
            var context = BuildContext();

            var configuration = GetConfiguration();
            configuration.StudentProfile = HomelessProfile;
            context.StudentCharacteristics.HispanicLatinoEthnicity = false;

            var studentEducationGenerator = new StudentEducationOrganizationEntityGenerator(randomNumberGenerator);

            studentEducationGenerator.Configure(configuration);
            studentEducationGenerator.Generate(context);
            var studentEd = context.GetStudentEducationOrganization();


            studentEd.Telephone.ShouldBeNull();
            studentEd.Address.ShouldBeNull();
        }

        [Test]
        public void ShouldSetSpanishLanguageForHispanicLatinoImmigrant()
        {
            var randomNumberGenerator = new TestRandomNumberGenerator { RandomIntSequence = new[] { 0 }, RandomDoubleSequence = new[] { 0.0 } };
            var context = BuildContext();
            context.StudentCharacteristics.IsImmigrant = true;
            context.StudentCharacteristics.HispanicLatinoEthnicity = true;

            var studentEducationGenerator = new StudentEducationOrganizationEntityGenerator(randomNumberGenerator);
            studentEducationGenerator.Configure(GetConfiguration());
            studentEducationGenerator.Generate(context);

            var studentEd = context.GetStudentEducationOrganization();

            studentEd.Language.Length.ShouldBe(1);
            studentEd.Language[0].Language1.ShouldBe(LanguageMapType.Spanish.Value);
            studentEd.Language[0].LanguageUse.First().ShouldBe(LanguageUseType.Homelanguage);
        }

        [Test]
        public void ShouldSetOtherLanguageForNonHispanicLatinoImmigrant()
        {
            var randomNumberGenerator = new TestRandomNumberGenerator { RandomIntSequence = new[] { 0 }, RandomDoubleSequence = new[] { 0.0 } };
            var context = BuildContext();
            context.StudentCharacteristics.IsImmigrant = true;
            context.StudentCharacteristics.HispanicLatinoEthnicity = false;

            var studentEducationGenerator = new StudentEducationOrganizationEntityGenerator(randomNumberGenerator);
            studentEducationGenerator.Configure(GetConfiguration());
            studentEducationGenerator.Generate(context);

            var studentEd = context.GetStudentEducationOrganization();

            studentEd.Language.Length.ShouldBe(1);
            studentEd.Language[0].Language1.ShouldBe(LanguageMapType.Other.Value);
            studentEd.Language[0].LanguageUse.First().ShouldBe(LanguageUseType.Homelanguage.Value);
        }

        [Test]
        public void ShouldDoNothingForNonImmigrant()
        {
            var randomNumberGenerator = new TestRandomNumberGenerator { RandomIntSequence = new[] { 0 }, RandomDoubleSequence = new[] { 0.0 } };
            var context = BuildContext();
            context.StudentCharacteristics.IsImmigrant = false;
            var configuration = GetConfiguration();
            configuration.StudentProfile = new TestStudentProfile();

            var studentEducationGenerator = new StudentEducationOrganizationEntityGenerator(randomNumberGenerator);
            studentEducationGenerator.Configure(configuration);
            studentEducationGenerator.Generate(context);

            var studentEd = context.GetStudentEducationOrganization();

            studentEd.Language.ShouldBeNull();
        }
        [Test]
        public void ShouldDoNothingIfEconomicDisadvantageNotConfigured()
        {
            var randomNumberGenerator = new TestRandomNumberGenerator { RandomIntSequence = new[] { 0 }, RandomDoubleSequence = new[] { 0.0 } };

            var config = GetConfiguration();
            config.StudentProfile = new TestStudentProfile
            {
                RaceConfiguration = new TestAttributeConfiguration
                {
                    Name = "Race",
                    AttributeGeneratorConfigurationOptions = new[]
                    {
                        new TestAttributeGeneratorConfigurationOption
                        {
                            Frequency = 1.00,
                            Value = "White"
                        }
                    }
                },
                EconomicDisadvantageConfiguration = null
            };
            ValidateGeneratedEconomicDisadvantage(randomNumberGenerator, config, false);
        }

        [Test]
        public void ShouldSetEconomicDisadvantagedFlagIfRandomlySelected()
        {
            var randomNumberGenerator = new TestRandomNumberGenerator { RandomIntSequence = new[] { 0 }, RandomDoubleSequence = new[] { 0.1 } };
            var config = GetConfiguration();
            config.StudentProfile = new TestStudentProfile
            {
                RaceConfiguration = new TestAttributeConfiguration
                {
                    Name = "Race",
                    AttributeGeneratorConfigurationOptions = new[]
                    {
                        new TestAttributeGeneratorConfigurationOption
                        {
                            Frequency = 1.00,
                            Value = "White"
                        }
                    }
                },
                EconomicDisadvantageConfiguration = new TestAttributeConfiguration
                {
                    Name = "EconomicDisadvantaged",
                    AttributeGeneratorConfigurationOptions = new[]
                    {
                        new TestAttributeGeneratorConfigurationOption
                        {
                            Frequency = 1,
                            Value = "White"
                        }
                    }
                }
            };

            ValidateGeneratedEconomicDisadvantage(randomNumberGenerator, config, true);
        }

        private static void ValidateGeneratedEconomicDisadvantage(TestRandomNumberGenerator randomNumberGenerator, StudentDataGeneratorConfig config, bool expectedValue)
        {
            var context = BuildContext();
            context.StudentCharacteristics.Race = RaceDescriptor.White;
            context.StudentCharacteristics.HispanicLatinoEthnicity = false;

            var studentEducationGenerator = new StudentEducationOrganizationEntityGenerator(randomNumberGenerator);
            studentEducationGenerator.Configure(config);
            studentEducationGenerator.Generate(context);

            var result = context.GetStudentEducationOrganization().HasCharacteristic(StudentCharacteristicDescriptor.EconomicDisadvantaged);
            result.ShouldBe(expectedValue);
        } 
    }
}
