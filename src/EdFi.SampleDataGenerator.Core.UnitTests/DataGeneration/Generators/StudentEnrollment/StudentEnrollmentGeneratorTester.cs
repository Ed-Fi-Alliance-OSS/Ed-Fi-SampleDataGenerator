using System.Collections.Generic;
using System.Linq;
using EdFi.SampleDataGenerator.Core.DataGeneration.Generators;
using EdFi.SampleDataGenerator.Core.DataGeneration.Generators.StudentEnrollment;
using EdFi.SampleDataGenerator.Core.Entities;
using EdFi.SampleDataGenerator.Core.Serialization.Output;
using EdFi.SampleDataGenerator.Core.Serialization.Output.Interchanges;
using EdFi.SampleDataGenerator.Core.UnitTests.DataGeneration.Common;
using NUnit.Framework;
using Shouldly;

namespace EdFi.SampleDataGenerator.Core.UnitTests.DataGeneration.Generators.StudentEnrollment
{
    [TestFixture]
    public class StudentEnrollmentGeneratorTester : GeneratorTestBase
    {
        [Test]
        public void ShouldGenerateStudentEnrollment()
        {
            var randomNumberGenerator = new TestRandomNumberGenerator { RandomIntSequence = new[] { 0 }, RandomDoubleSequence = new[] { 0.0 } };
            var studentGenerator = new StudentEnrollmentInterchangeGenerator(randomNumberGenerator);
            var sampleDataGeneratorConfig = GetSampleDataGeneratorConfig();
            var globalDataGeneratorConfig = GetGlobalDataGeneratorConfig();
            var globalData = new GlobalData
            {
                GraduationPlans = globalDataGeneratorConfig.GraduationPlans,
                EducationOrganizationData = globalDataGeneratorConfig.EducationOrganizationData,
                MasterScheduleData = globalDataGeneratorConfig.MasterScheduleData
            };
            var studentDataGeneratorConfig = GetStudentGeneratorConfig(globalData, sampleDataGeneratorConfig, globalDataGeneratorConfig);

            var context = new StudentDataGeneratorContext
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
                                StudentCourses = globalDataGeneratorConfig.EducationOrganizationData.Courses.Select(c => new StudentCourse { Course = c, CourseGradeAverage = 100 }).ToList(),
                                CurrentSchoolYear = true,
                                GradeLevel = GradeLevelDescriptor.TwelfthGrade,
                                SchoolYear = globalDataGeneratorConfig.EducationOrgCalendarData.Sessions.First().SchoolYear,
                                Session = globalDataGeneratorConfig.EducationOrgCalendarData.Sessions.First(),
                                Term = globalDataGeneratorConfig.EducationOrgCalendarData.Sessions.First().Term
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
                    OldEthnicity = OldEthnicityDescriptor.Hispanic
                }
            };

            studentGenerator.Configure(studentDataGeneratorConfig);
            studentGenerator.Generate(context);

            context.GeneratedStudentData.StudentEnrollmentData.ShouldNotBeNull();
        }
    }
}
