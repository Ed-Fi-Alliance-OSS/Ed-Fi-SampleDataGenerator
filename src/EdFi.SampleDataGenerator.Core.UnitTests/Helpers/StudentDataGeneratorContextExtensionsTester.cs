using System;
using EdFi.SampleDataGenerator.Core.DataGeneration.Generators;
using EdFi.SampleDataGenerator.Core.Entities;
using EdFi.SampleDataGenerator.Core.Helpers;
using EdFi.SampleDataGenerator.Core.Serialization.Output;
using EdFi.SampleDataGenerator.Core.Serialization.Output.Interchanges;
using EdFi.SampleDataGenerator.Core.UnitTests.Config;
using NUnit.Framework;
using Shouldly;

namespace EdFi.SampleDataGenerator.Core.UnitTests.Helpers
{
    [TestFixture]
    public class StudentDataGeneratorContextExtensionsTester
    {
        [Test]
        public void ShouldCreateSeedRecordFromContext()
        {
            var student = new Student
            {
                Name = new Name
                {
                    FirstName = "John",
                    MiddleName = "R",
                    LastSurname = "Smith"
                },
                BirthData = new BirthData
                {
                    BirthDate = new DateTime(2012, 03, 08)
                }
            };

            var context = new StudentDataGeneratorContext
            {
                GeneratedStudentData = new GeneratedStudentData
                {
                    StudentData = new StudentData
                    {
                        Student = student
                    },
                    StudentEnrollmentData = new StudentEnrollmentData
                    {
                        StudentSchoolAssociation = new StudentSchoolAssociation
                        {
                            SchoolReference = new SchoolReferenceType
                            {
                                SchoolIdentity = new SchoolIdentityType
                                {
                                    SchoolId = 1
                                }
                            }
                        }
                    }
                },
                StudentPerformanceProfile = new StudentPerformanceProfile
                {
                    PerformanceIndex = 0.5
                },
                StudentCharacteristics = new StudentCharacteristics
                {
                    HispanicLatinoEthnicity = true,
                    Race = RaceDescriptor.Other,
                    Sex = SexDescriptor.Male
                }
            };

            var schoolProfile = new TestSchoolProfile
            {
                SchoolId = 1
            };

            var gradeProfile = new TestGradeProfile
            {
                GradeName = GradeLevelDescriptor.NinthGrade.CodeValue
            };
            

            var seedRecord = context.GetSeedRecord(schoolProfile, gradeProfile);

            seedRecord.SchoolId.ShouldBe(1);
            seedRecord.PerformanceIndex.ShouldBe(0.5);
            seedRecord.GradeLevel.ShouldBe(GradeLevelDescriptor.NinthGrade);
            seedRecord.FirstName.ShouldBe(student.Name.FirstName);
            seedRecord.MiddleName.ShouldBe(student.Name.MiddleName);
            seedRecord.LastName.ShouldBe(student.Name.LastSurname);
            seedRecord.BirthDate.ShouldBe(student.BirthData.BirthDate);
            seedRecord.Race.ShouldBe(RaceDescriptor.Other);
            seedRecord.Gender.ShouldBe(SexDescriptor.Male);
            seedRecord.HispanicLatinoEthnicity.ShouldBeTrue();
        }
    }
}
