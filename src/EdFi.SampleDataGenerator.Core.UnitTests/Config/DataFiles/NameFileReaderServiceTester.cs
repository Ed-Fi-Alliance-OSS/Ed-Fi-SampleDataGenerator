using System.Collections.Generic;
using System.Linq;
using EdFi.SampleDataGenerator.Core.Config;
using EdFi.SampleDataGenerator.Core.Config.DataFiles;
using EdFi.SampleDataGenerator.Core.Entities;
using Moq;
using NUnit.Framework;
using Shouldly;

namespace EdFi.SampleDataGenerator.Core.UnitTests.Config.DataFiles
{
    [TestFixture]
    public class NameFileReaderServiceTester
    {
        [Test]
        public void ShouldReadSurnameFile()
        {
            var testConfig = new TestSampleDataGeneratorConfig
            {
                DataFilePath = "",
                DataFileConfig = new TestDataFileConfig
                {
                    SurnameFiles = new[]
                    {
                        new TestSurnameFileMapping
                        {
                            Ethnicity = "Test",
                            FilePath = "TestFile.txt"
                        },
                    }
                },
                EthnicityMappings = new []
                {
                    new TestEthnicityMapping { EdFiRaceType = "White", Ethnicity = "White", HispanicLatinoEthnicity = false, RaceDescriptor = RaceDescriptor.White }
                }
            };

            var testSurnameFileMapping = new TestSurnameFileMapping {Ethnicity = "White", FilePath = "TestFile.txt"};
            var fileReader = new Mock<INameFileReader>();

            var fileRecords = new List<NameFileRecord> {new NameFileRecord {Name = "FakeName", Frequency = 1.0}};

            fileReader.Setup(r => r.Read(It.IsAny<string>())).Returns(fileRecords);

            var sut = new NameFileReaderService(fileReader.Object);
            var result = sut.ReadSurnameFile(testConfig, testSurnameFileMapping);
            fileReader.Verify(r => r.Read("TestFile.txt"), Times.Once);

            ValidateSurnameFile(result, testConfig.EthnicityMappings.MappingFor(RaceDescriptor.White, false), "TestFile.txt", fileRecords);
        }

        [Test]
        public void ShouldUseEthnicityWhenReadingSurnameFile()
        {
            var testConfig = new TestSampleDataGeneratorConfig
            {
                DataFilePath = "",
                DataFileConfig = new TestDataFileConfig
                {
                    SurnameFiles = new[]
                    {
                        new TestSurnameFileMapping
                        {
                            Ethnicity = "Hispanic",
                            FilePath = "TestFile.txt"
                        },
                        new TestSurnameFileMapping
                        {
                            Ethnicity = "White",
                            FilePath = "OtherTestFile.txt"
                        },
                    }
                },
                EthnicityMappings = new[]
                {
                    new TestEthnicityMapping { EdFiRaceType = "White", Ethnicity = "Hispanic", HispanicLatinoEthnicity = true, RaceDescriptor = RaceDescriptor.White },
                    new TestEthnicityMapping { EdFiRaceType = "White", Ethnicity = "White", HispanicLatinoEthnicity = false, RaceDescriptor = RaceDescriptor.White }
                }
            };

            var testSurnameFileMapping = new TestSurnameFileMapping { Ethnicity = "Hispanic", FilePath = "TestFile.txt" };
            var fileReader = new Mock<INameFileReader>();

            var fileRecords = new List<NameFileRecord> { new NameFileRecord { Name = "FakeName", Frequency = 1.0 } };

            fileReader.Setup(r => r.Read(It.IsAny<string>())).Returns(fileRecords);

            var sut = new NameFileReaderService(fileReader.Object);
            var result = sut.ReadSurnameFile(testConfig, testSurnameFileMapping);
            fileReader.Verify(r => r.Read("TestFile.txt"), Times.Once);
            fileReader.Verify(r => r.Read("OtherTestFile.txt"), Times.Never);

            ValidateSurnameFile(result, testConfig.EthnicityMappings.MappingFor(RaceDescriptor.White, true), "TestFile.txt", fileRecords);
        }

        [Test]
        public void ShouldReadFirstNameFile()
        {
            var testConfig = new TestSampleDataGeneratorConfig
            {
                DataFilePath = "",
                DataFileConfig = new TestDataFileConfig
                {
                    FirstNameFiles = new[]
                    {
                        new TestFirstNameFileMapping
                        {
                            Ethnicity = "Test",
                            FilePath = "TestFile.txt",
                            Gender = "Male"
                        },
                    }
                },
                EthnicityMappings = new[]
                {
                    new TestEthnicityMapping { EdFiRaceType = "White", Ethnicity = "White", HispanicLatinoEthnicity = false, RaceDescriptor = RaceDescriptor.White }
                },
                GenderMappings = new []
                {
                    new TestGenderMapping { EdFiGender = "Male", Gender = "Male", SexDescriptor = SexDescriptor.Male }
                }
            };

            var testFirstNameFileMapping = new TestFirstNameFileMapping { Ethnicity = "White", FilePath = "TestFile.txt", Gender = "Male"};
            var fileReader = new Mock<INameFileReader>();

            var fileRecords = new List<NameFileRecord> { new NameFileRecord { Name = "FakeName", Frequency = 1.0 } };

            fileReader.Setup(r => r.Read(It.IsAny<string>())).Returns(fileRecords);

            var sut = new NameFileReaderService(fileReader.Object);
            var result = sut.ReadFirstNameFile(testConfig, testFirstNameFileMapping);
            fileReader.Verify(r => r.Read("TestFile.txt"), Times.Once);

            ValidateFirstNameFile(result, testConfig.EthnicityMappings.MappingFor(RaceDescriptor.White, false), SexDescriptor.Male, "TestFile.txt", fileRecords);
        }

        [Test]
        public void ShouldUseEthnicityWhenReadingFirstNameFile()
        {
            var testConfig = new TestSampleDataGeneratorConfig
            {
                DataFilePath = "",
                DataFileConfig = new TestDataFileConfig
                {
                    FirstNameFiles = new[]
                    {
                        new TestFirstNameFileMapping
                        {
                            Ethnicity = "Hispanic",
                            FilePath = "TestFile.txt",
                            Gender = "Male",
                        },
                        new TestFirstNameFileMapping
                        {
                            Ethnicity = "White",
                            FilePath = "OtherTestFile.txt",
                            Gender = "Male",
                        },
                    }
                },
                EthnicityMappings = new[]
                {
                    new TestEthnicityMapping {Ethnicity = "Hispanic", EdFiRaceType = RaceDescriptor.White.CodeValue, HispanicLatinoEthnicity = true, RaceDescriptor = RaceDescriptor.White },
                    new TestEthnicityMapping {Ethnicity = "White", EdFiRaceType = RaceDescriptor.White.CodeValue, HispanicLatinoEthnicity = false, RaceDescriptor = RaceDescriptor.White },
                },
                GenderMappings = new[]
                {
                    new TestGenderMapping { EdFiGender = "Male", Gender = "Male", SexDescriptor = SexDescriptor.Male }
                }
            };

            var testFirstNameFileMapping = new TestFirstNameFileMapping { Ethnicity = "Hispanic", FilePath = "TestFile.txt", Gender = "Male" };
            var fileReader = new Mock<INameFileReader>();

            var fileRecords = new List<NameFileRecord> { new NameFileRecord { Name = "FakeName", Frequency = 1.0 } };

            fileReader.Setup(r => r.Read(It.IsAny<string>())).Returns(fileRecords);

            var sut = new NameFileReaderService(fileReader.Object);
            var result = sut.ReadFirstNameFile(testConfig, testFirstNameFileMapping);
            fileReader.Verify(r => r.Read("TestFile.txt"), Times.Once);
            fileReader.Verify(r => r.Read("OtherTestFile.txt"), Times.Never);

            ValidateFirstNameFile(result, testConfig.EthnicityMappings.MappingFor(RaceDescriptor.White, true), SexDescriptor.Male, "TestFile.txt", fileRecords);
        }

        [Test]
        public void ShouldReadStreetNameFile()
        {
            var testConfig = new TestSampleDataGeneratorConfig
            {
                DataFilePath = "",
                DataFileConfig = new TestDataFileConfig
                {
                    StreetNameFile = new TestStreetNameFileMapping
                    {
                        FilePath = "TestFile.txt"
                    }
                }
            };

            var fileReader = new Mock<INameFileReader>();

            var fileRecords = new List<NameFileRecord> { new NameFileRecord { Name = "FakeName", Frequency = 1.0 } };
            fileReader.Setup(r => r.Read(It.IsAny<string>())).Returns(fileRecords);

            var sut = new NameFileReaderService(fileReader.Object);
            var result = sut.ReadStreetNameFile(testConfig);
            fileReader.Verify(r => r.Read("TestFile.txt"), Times.Once);
            
            result.FilePath.ShouldBe("TestFile.txt");
            result.FileRecords.ShouldHaveSameContentAs(fileRecords);
        }

        [Test]
        public void ShouldReadNameFiles()
        {
            var testConfig = new TestSampleDataGeneratorConfig
            {
                DataFilePath = "",
                DataFileConfig = new TestDataFileConfig
                {
                    FirstNameFiles = new[]
                    {
                        new TestFirstNameFileMapping
                        {
                            Ethnicity = "White",
                            FilePath = "TestFirstNameFile1.txt",
                            Gender = "Male"
                        },
                        new TestFirstNameFileMapping
                        {
                            Ethnicity = "Asian",
                            FilePath = "TestFirstNameFile2.txt",
                            Gender = "Female"
                        },
                        new TestFirstNameFileMapping
                        {
                            Ethnicity = "White",
                            FilePath = "TestFirstNameFile3.txt",
                            Gender = "Female"
                        },
                        new TestFirstNameFileMapping
                        {
                            Ethnicity = "Asian",
                            FilePath = "TestFirstNameFile4.txt",
                            Gender = "Male"
                        },
                    },
                    SurnameFiles = new[]
                    {
                        new TestSurnameFileMapping
                        {
                            Ethnicity = "White",
                            FilePath = "TestSurnameFile1.txt"
                        },
                        new TestSurnameFileMapping
                        {
                            Ethnicity = "Asian",
                            FilePath = "TestSurnameFile2.txt"
                        }
                    },
                    StreetNameFile = new TestStreetNameFileMapping
                    {
                        FilePath = "TestStreetNameFile.txt"
                    }
                },
                EthnicityMappings = new[]
                {
                    new TestEthnicityMapping { EdFiRaceType = "White", Ethnicity = "White", HispanicLatinoEthnicity = false, RaceDescriptor = RaceDescriptor.White },
                    new TestEthnicityMapping { EdFiRaceType = "Asian", Ethnicity = "Asian", HispanicLatinoEthnicity = false, RaceDescriptor = RaceDescriptor.Asian }
                },
                GenderMappings = new[]
                {
                    new TestGenderMapping { EdFiGender = "Male", Gender = "Male", SexDescriptor = SexDescriptor.Male },
                    new TestGenderMapping { EdFiGender = "Female", Gender = "Female", SexDescriptor = SexDescriptor.Female }
                }
            };
            
            var fileReader = new Mock<INameFileReader>();

            var fileRecords = new List<NameFileRecord> { new NameFileRecord { Name = "FakeName", Frequency = 1.0 } };
            fileReader.Setup(r => r.Read(It.IsAny<string>())).Returns(fileRecords);

            var sut = new NameFileReaderService(fileReader.Object);
            var result = sut.Read(testConfig);

            fileReader.Verify(r => r.Read("TestFirstNameFile1.txt"), Times.Once);
            fileReader.Verify(r => r.Read("TestFirstNameFile2.txt"), Times.Once);
            fileReader.Verify(r => r.Read("TestFirstNameFile3.txt"), Times.Once);
            fileReader.Verify(r => r.Read("TestFirstNameFile4.txt"), Times.Once);
            fileReader.Verify(r => r.Read("TestSurnameFile1.txt"), Times.Once);
            fileReader.Verify(r => r.Read("TestSurnameFile2.txt"), Times.Once);
            fileReader.Verify(r => r.Read("TestStreetNameFile.txt"), Times.Once);

            result.FirstNameFiles.ShouldNotBeNull();
            result.FirstNameFiles.Count.ShouldBe(4);

            ValidateFileDataFirstNameFile(result, testConfig.EthnicityMappings.MappingFor(RaceDescriptor.White, false), SexDescriptor.Male, "TestFirstNameFile1.txt", fileRecords);
            ValidateFileDataFirstNameFile(result, testConfig.EthnicityMappings.MappingFor(RaceDescriptor.Asian, false), SexDescriptor.Female, "TestFirstNameFile2.txt", fileRecords);
            ValidateFileDataFirstNameFile(result, testConfig.EthnicityMappings.MappingFor(RaceDescriptor.White, false), SexDescriptor.Female, "TestFirstNameFile3.txt", fileRecords);
            ValidateFileDataFirstNameFile(result, testConfig.EthnicityMappings.MappingFor(RaceDescriptor.Asian, false), SexDescriptor.Male, "TestFirstNameFile4.txt", fileRecords);
            
            result.SurnameFiles.ShouldNotBeNull();
            result.SurnameFiles.Count.ShouldBe(2);
            ValidateFileDataSurnameFile(result, testConfig.EthnicityMappings.MappingFor(RaceDescriptor.White, false), "TestSurnameFile1.txt", fileRecords);
            ValidateFileDataSurnameFile(result, testConfig.EthnicityMappings.MappingFor(RaceDescriptor.Asian, false), "TestSurnameFile2.txt", fileRecords);
            
            result.StreetNameFile.ShouldNotBeNull();
            result.StreetNameFile.FilePath.ShouldBe("TestStreetNameFile.txt");
        }

        private void ValidateFirstNameFile(FirstNameFile result, IEthnicityMapping ethnicity, SexDescriptor sexDescriptor, string filePath, IEnumerable<NameFileRecord> fileRecords)
        {
            result.FilePath.ShouldBeSameAs(filePath);
            result.Ethnicity.ShouldBe(ethnicity);
            result.SexDescriptor.ShouldBe(sexDescriptor);
            result.FileRecords.ShouldHaveSameContentAs(fileRecords);
        }

        private void ValidateFileDataFirstNameFile(NameFileData result, IEthnicityMapping ethnicity, SexDescriptor sexDescriptor, string filePath, IEnumerable<NameFileRecord> fileRecords)
        {
            ValidateFirstNameFile(result.FirstNameFiles[ethnicity, sexDescriptor], ethnicity, sexDescriptor, filePath, fileRecords);
        }

        private void ValidateSurnameFile(SurnameFile result, IEthnicityMapping ethnicity, string filePath, IEnumerable<NameFileRecord> fileRecords)
        {
            result.FilePath.ShouldBe(filePath);
            result.FileRecords.ShouldHaveSameContentAs(fileRecords);
            result.Ethnicity.ShouldBe(ethnicity);
        }

        private void ValidateFileDataSurnameFile(NameFileData result, IEthnicityMapping ethnicity, string filePath, IEnumerable<NameFileRecord> fileRecords)
        {
            ValidateSurnameFile(result.SurnameFiles[ethnicity], ethnicity, filePath, fileRecords);
        }
    }
}
