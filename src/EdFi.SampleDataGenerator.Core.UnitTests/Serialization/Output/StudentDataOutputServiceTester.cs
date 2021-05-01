using System;
using System.Collections.Generic;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common.Interchanges;
using EdFi.SampleDataGenerator.Core.Entities;
using EdFi.SampleDataGenerator.Core.Serialization.Output;
using EdFi.SampleDataGenerator.Core.Serialization.Output.Interchanges;
using EdFi.SampleDataGenerator.Core.UnitTests.Config;
using Moq;
using NUnit.Framework;
using Shouldly;

namespace EdFi.SampleDataGenerator.Core.UnitTests.Serialization.Output
{
    [TestFixture]
    public class StudentDataOutputServiceTester
    {
        [Test]
        public void ShouldSerializeWhenBufferIsFull()
        {
            var outputCountsByType = new Dictionary<Type, int>();

            var interchangeFileOutputService = new Mock<IInterchangeFileOutputService>();
            interchangeFileOutputService
                .Setup(x => x.WriteOutputToFile(It.IsAny<string>(), It.IsAny<object>()))
                .Callback<string, object>((file, outputObj) =>
                {
                    var outputObjectType = outputObj.GetType();
                    var currentCount = 0;

                    if (outputCountsByType.ContainsKey(outputObjectType))
                    {
                        currentCount = outputCountsByType[outputObjectType];
                    }

                    outputCountsByType[outputObjectType] = currentCount + 1;
                });

            var studentOutputService = new StudentDataOutputService(interchangeFileOutputService.Object);
            var configuration = new StudentDataOutputConfiguration
            {
                SampleDataGeneratorConfig = new TestSampleDataGeneratorConfig
                {
                    BatchSize = 1,
                    OutputPath = "C:\\Test"
                },
                SchoolProfile = new TestSchoolProfile
                {
                    InitialStudentCount = 100,
                    SchoolName = "TestSchool"
                },
                DataPeriod = new TestDataPeriod
                {
                    StartDate = new DateTime(),
                    EndDate = new DateTime(),
                    Name = "TestPeriod"
                }
            };

            var generatedStudentData = new GeneratedStudentData
            {
                StudentData = new StudentData {Student = new Student()},
                StudentEnrollmentData =
                    new StudentEnrollmentData {StudentSchoolAssociation = new StudentSchoolAssociation()}
            };

            studentOutputService.Configure(configuration);
            studentOutputService.WriteToOutput(generatedStudentData, 0);
            studentOutputService.WriteToOutput(generatedStudentData, 0);
            studentOutputService.WriteToOutput(generatedStudentData, 0);

            outputCountsByType[typeof(InterchangeStudent)].ShouldBe(3);
            outputCountsByType[typeof(InterchangeStudentEnrollment)].ShouldBe(3);
        }

        [Test]
        public void ShouldSerializeWhenFlushIsCalled()
        {
            var outputCountsByType = new Dictionary<Type, int>();

            var interchangeFileOutputService = new Mock<IInterchangeFileOutputService>();
            interchangeFileOutputService
                .Setup(x => x.WriteOutputToFile(It.IsAny<string>(), It.IsAny<object>()))
                .Callback<string, object>((file, outputObj) =>
                {
                    var outputObjectType = outputObj.GetType();
                    var currentCount = 0;

                    if (outputCountsByType.ContainsKey(outputObjectType))
                    {
                        currentCount = outputCountsByType[outputObjectType];
                    }

                    outputCountsByType[outputObjectType] = currentCount + 1;
                });

            var studentOutputService = new StudentDataOutputService(interchangeFileOutputService.Object);
            var configuration = new StudentDataOutputConfiguration
            {
                SampleDataGeneratorConfig = new TestSampleDataGeneratorConfig
                {
                    BatchSize = 100,
                    OutputPath = "C:\\Test"
                },
                SchoolProfile = new TestSchoolProfile
                {
                    InitialStudentCount = 100,
                    SchoolName = "TestSchool"
                },
                DataPeriod = new TestDataPeriod
                {
                    StartDate = new DateTime(),
                    EndDate = new DateTime(),
                    Name = "TestPeriod"
                }
            };

            var generatedStudentData = new GeneratedStudentData
            {
                StudentData = new StudentData {Student = new Student()},
                StudentEnrollmentData =
                    new StudentEnrollmentData {StudentSchoolAssociation = new StudentSchoolAssociation()}
            };

            studentOutputService.Configure(configuration);
            studentOutputService.WriteToOutput(generatedStudentData, 0);

            interchangeFileOutputService.Verify(x => x.WriteOutputToFile(It.IsAny<string>(), It.IsAny<object>()), Times.Never);
            studentOutputService.FlushOutput();

            outputCountsByType[typeof(InterchangeStudent)].ShouldBe(1);
            outputCountsByType[typeof(InterchangeStudentEnrollment)].ShouldBe(1);
        }

        [Test]
        public void ShouldNotIncludeBatchIdInFileNameWhenBatchingDisabled()
        {
            var studentOutputService = new StudentDataOutputService();

            var configuration = new StudentDataOutputConfiguration
            {
                SampleDataGeneratorConfig = new TestSampleDataGeneratorConfig
                {
                    BatchSize = 0,
                    OutputPath = ""
                },
                SchoolProfile = new TestSchoolProfile()
                {
                    SchoolName = "TestSchool"
                },
                DataPeriod = new TestDataPeriod
                {
                    StartDate = new DateTime(),
                    EndDate = new DateTime(),
                    Name = "TestPeriod"
                }
            };

            studentOutputService.Configure(configuration);
            var fileName = studentOutputService.GetOutputFilePath(Interchange.Student);
            fileName.ShouldBe("Student-TestSchool-TestPeriod.xml");
        }

        [Test]
        public void ShouldIncludeBatchIdInFilenameWhenBatchingEnabled()
        {
            var studentOutputService = new StudentDataOutputService();

            var configuration = new StudentDataOutputConfiguration
            {
                SampleDataGeneratorConfig = new TestSampleDataGeneratorConfig
                {
                    BatchSize = 1,
                    OutputPath = ""
                },
                SchoolProfile = new TestSchoolProfile()
                {
                    SchoolName = "TestSchool",
                    InitialStudentCount = 2
                },
                DataPeriod = new TestDataPeriod
                {
                    StartDate = new DateTime(),
                    EndDate = new DateTime(),
                    Name = "TestPeriod"
                }
            };

            studentOutputService.Configure(configuration);
            var fileName = studentOutputService.GetOutputFilePath(Interchange.Student);
            fileName.ShouldBe("Student-TestSchool-TestPeriod-0001.xml");
        }
    }
}