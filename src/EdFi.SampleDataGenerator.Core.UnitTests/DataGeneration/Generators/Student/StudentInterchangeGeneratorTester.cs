using System;
using System.Linq;
using EdFi.SampleDataGenerator.Core.Config.SeedData;
using EdFi.SampleDataGenerator.Core.DataGeneration.Generators;
using EdFi.SampleDataGenerator.Core.DataGeneration.Generators.Student;
using EdFi.SampleDataGenerator.Core.Entities;
using EdFi.SampleDataGenerator.Core.Serialization.Output;
using EdFi.SampleDataGenerator.Core.UnitTests.DataGeneration.Common;
using NUnit.Framework;
using Shouldly;

namespace EdFi.SampleDataGenerator.Core.UnitTests.DataGeneration.Generators.Student
{
    [TestFixture]
    public class StudentInterchangeGeneratorTester : GeneratorTestBase
    {
        [Test]
        public void ShouldSuccessfullyGenerateAStudent()
        {
            var randomNumberGenerator = new TestRandomNumberGenerator {RandomIntSequence = new[] {0}, RandomDoubleSequence = new []{ 0.0 }};
            var studentGenerator = new StudentInterchangeGenerator(randomNumberGenerator);
            var sampleDataGeneratorConfig = GetSampleDataGeneratorConfig();
            var globalDataGeneratorConfig = GetGlobalDataGeneratorConfig();
            var globalDataGeneratorContext = GetGlobalDataGeneratorContext(globalDataGeneratorConfig);
            var studentDataGeneratorConfig = GetStudentGeneratorConfig(globalDataGeneratorContext.GlobalData, sampleDataGeneratorConfig, globalDataGeneratorConfig);

            var context = new StudentDataGeneratorContext
            {
                GlobalStudentNumber = 0,
                GeneratedStudentData = new GeneratedStudentData(),
                StudentPerformanceProfile = new StudentPerformanceProfile(),
                StudentCharacteristics = new StudentCharacteristics()
            };

            studentGenerator.Configure(studentDataGeneratorConfig);
            studentGenerator.Generate(context);

            context.GeneratedStudentData.StudentData.ShouldNotBeNull();
            context.GeneratedStudentData.StudentData.Student.ShouldNotBeNull();
        }

        [Test]
        public void ShouldUseSeedDataWhenPresent()
        {
            var randomNumberGenerator = new TestRandomNumberGenerator { RandomIntSequence = new[] { 0 }, RandomDoubleSequence = new[] { 0.0 } };
            var studentGenerator = new StudentInterchangeGenerator(randomNumberGenerator);
            var sampleDataGeneratorConfig = GetSampleDataGeneratorConfig();
            var globalDataGeneratorConfig = GetGlobalDataGeneratorConfig();
            var globalDataGeneratorContext = GetGlobalDataGeneratorContext(globalDataGeneratorConfig);
            var studentDataGeneratorConfig = GetStudentGeneratorConfig(globalDataGeneratorContext.GlobalData, sampleDataGeneratorConfig, globalDataGeneratorConfig);

            var birthDate = new DateTime(2017, 11, 15);

            var seedRecord = new SeedRecord
            {
                FirstName = "First",
                MiddleName = "Middle",
                LastName = "Last",
                BirthDate = birthDate,
                Gender = SexDescriptor.Male,
                GradeLevel = GradeLevelDescriptor.EighthGrade,
                Race = RaceDescriptor.NativeHawaiianPacificIslander,
                HispanicLatinoEthnicity = true,
                PerformanceIndex = 0.5,
                SchoolId = 1234
            };

            var context = new StudentDataGeneratorContext
            {
                GlobalStudentNumber = 0,
                GeneratedStudentData = new GeneratedStudentData(),
                SeedRecord = seedRecord,
                StudentPerformanceProfile = new StudentPerformanceProfile(),
                StudentCharacteristics = new StudentCharacteristics()
            };

            studentGenerator.Configure(studentDataGeneratorConfig);
            studentGenerator.Generate(context);

            var student = context.Student;
            student.ShouldNotBeNull();

            student.Name.FirstName.ShouldBe("First");
            student.Name.MiddleName.ShouldBe("Middle");
            student.Name.LastSurname.ShouldBe("Last");
            student.BirthData.BirthDate.ShouldBe(birthDate);

            context.StudentPerformanceProfile.PerformanceIndex.ShouldBe(0.5);
        }
    }
}
