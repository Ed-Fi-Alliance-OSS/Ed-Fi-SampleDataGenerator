using System;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common;
using EdFi.SampleDataGenerator.Core.DataGeneration.Generators;
using EdFi.SampleDataGenerator.Core.DataGeneration.Generators.Student.Attributes;
using EdFi.SampleDataGenerator.Core.UnitTests.Config;
using EdFi.SampleDataGenerator.Core.UnitTests.DataGeneration.Common;
using NUnit.Framework;
using Shouldly;

namespace EdFi.SampleDataGenerator.Core.UnitTests.DataGeneration.Generators.Student.Attributes
{
    [TestFixture]
    public class BirthDataGeneratorTester : StudentAttributeGeneratorTestBase
    {
        protected const int SchoolStartYear = 2016;
        protected StudentDataGeneratorConfig StudentGeneratorConfig => new StudentDataGeneratorConfig
        {
            GlobalConfig = new TestSampleDataGeneratorConfig
            {
                TimeConfig = new TestTimeConfig
                {
                    SchoolCalendarConfig = new TestSchoolCalendarConfig
                    {
                        StartDate = new DateTime(2016, 8, 31),
                        EndDate = new DateTime(2017, 5, 31)
                    }
                }
            },
            GradeProfile = new TestGradeProfile
            {
                GradeName = "Twelfth grade"
            }
        };

        [Test]
        public void ShouldExecute()
        {
            var randomNumberGenerator = new RandomNumberGenerator();

            var birthDataGenerator = new BirthDataGenerator(randomNumberGenerator);
            birthDataGenerator.Configure(StudentGeneratorConfig);

            var context = DefaultStudentEntityAttributeGenerationContext;
            birthDataGenerator.Generate(context);

            context.GeneratedStudentData.StudentData.Student.BirthData.ShouldNotBeNull();
        }

        [Test]
        public void ShouldCalculateBirthDateForNonHeldBackStudentWithNoRandomOffset()
        {
            var randomNumberGenerator = new TestRandomNumberGenerator {RandomIntSequence = new[] { 0 }, RandomDoubleSequence = new [] { 1.0 }};
            
            var birthDataGenerator = new BirthDataGenerator(randomNumberGenerator);
            birthDataGenerator.Configure(StudentGeneratorConfig);

            var context = DefaultStudentEntityAttributeGenerationContext;
            birthDataGenerator.Generate(context);

            context.GeneratedStudentData.StudentData.Student.BirthData.ShouldNotBeNull();
            var generatedDate = context.GeneratedStudentData.StudentData.Student.BirthData.BirthDate;

            //School starts on 8/31/2016
            //Average senior is assumed to be 17 on this date
            //"Random" values above should ensure a student was not held back, and that their birthday is actually on 9/1/1999 (after adjusting for 4 leap days in their lifetime)
            var expectedDate = new DateTime(1999, 9, 1);
            generatedDate.Date.ShouldBe(expectedDate);
        }

        [Test]
        public void ShouldCalculateBirthDateForHeldBackStudentWithNoRandomOffset()
        {
            var randomNumberGenerator = new TestRandomNumberGenerator { RandomIntSequence = new[] { 0 }, RandomDoubleSequence = new[] { BirthDataGenerator.HeldBackChance - 0.001 } };

            var birthDataGenerator = new BirthDataGenerator(randomNumberGenerator);
            birthDataGenerator.Configure(StudentGeneratorConfig);

            var context = DefaultStudentEntityAttributeGenerationContext;
            birthDataGenerator.Generate(context);

            context.GeneratedStudentData.StudentData.Student.BirthData.ShouldNotBeNull();
            var generatedDate = context.GeneratedStudentData.StudentData.Student.BirthData.BirthDate;

            //School starts on 8/31/2016
            //Average held-back senior is assumed to be 18 on this date
            //"Random" values above should ensure a student WAS held back, and that their birthday is actually on 9/1/1998 (after adjusting for 4 leap days in their lifetime)
            var expectedDate = new DateTime(1998, 9, 1);
            generatedDate.Date.ShouldBe(expectedDate);
        }

        [Test]
        public void ShouldProduceCorrectBirthdateForYoungestStudent()
        {
            var randomNumberGenerator = new TestRandomNumberGenerator { RandomIntSequence = new[] { BirthDataGenerator.MinDayOffset }, RandomDoubleSequence = new[] { 1.0 } };

            var birthDataGenerator = new BirthDataGenerator(randomNumberGenerator);
            birthDataGenerator.Configure(StudentGeneratorConfig);

            var context = DefaultStudentEntityAttributeGenerationContext;
            birthDataGenerator.Generate(context);

            context.GeneratedStudentData.StudentData.Student.BirthData.ShouldNotBeNull();
            var generatedDate = context.GeneratedStudentData.StudentData.Student.BirthData.BirthDate;

            //Youngest student was NOT held back and has Minimum "random" day offset applied to their birthdate
            //School starts on 8/31/2016
            //Average senior is assumed to be 17 on this date
            //"Random" values above should ensure a student was not held back, and that their birthday is 30 days prior to 9/1/1999 (after adjusting for 4 leap days in their lifetime)
            var expectedDate = new DateTime(1999, 9, 1);
            expectedDate = expectedDate.AddDays(-BirthDataGenerator.MinDayOffset);
            
            generatedDate.Date.ShouldBe(expectedDate);
        }

        [Test]
        public void ShouldProduceCorrectBirthdateForOldestStudent()
        {
            var randomNumberGenerator = new TestRandomNumberGenerator { RandomIntSequence = new[] { BirthDataGenerator.MaxDayOffset }, RandomDoubleSequence = new[] { BirthDataGenerator.HeldBackChance - 0.001 } };

            var birthDataGenerator = new BirthDataGenerator(randomNumberGenerator);
            birthDataGenerator.Configure(StudentGeneratorConfig);

            var context = DefaultStudentEntityAttributeGenerationContext;
            birthDataGenerator.Generate(context);

            context.GeneratedStudentData.StudentData.Student.BirthData.ShouldNotBeNull();
            var generatedDate = context.GeneratedStudentData.StudentData.Student.BirthData.BirthDate;

            //Oldest student WAS held back and has Maximum "random" day offset applied to their birthdate
            //School starts on 8/31/2016
            //Average held-back senior is assumed to be 18 on this date
            //"Random" values above should ensure a student WAS held back, and that their birthday is 365 days after 9/1/1999 (after adjusting for 4 leap days in their lifetime)
            var expectedDate = new DateTime(1998, 9, 1);
            expectedDate = expectedDate.AddDays(-BirthDataGenerator.MaxDayOffset);

            generatedDate.Date.ShouldBe(expectedDate);
        }
    }
}
