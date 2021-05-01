using EdFi.SampleDataGenerator.Core.Entities;
using EdFi.SampleDataGenerator.Core.Helpers;
using NUnit.Framework;
using Shouldly;

namespace EdFi.SampleDataGenerator.Core.UnitTests.Helpers
{
    [TestFixture]
    public class GradeLevelHelpersTester
    {
        [Test]
        public void ShouldGetNextK12GradeLevelWhenApplicable()
        {
            var allGradeLevels = DescriptorHelpers.GetAll<GradeLevelDescriptor>();

            foreach (var gradeLevel in allGradeLevels)
            {
                var gradeNumber = gradeLevel.GetNumericGradeLevel();

                if (gradeNumber >= 0 && gradeNumber <= 11)
                {
                    GradeLevelDescriptor nextGradeLevel;
                    gradeLevel.TryGetNextK12GradeLevel(out nextGradeLevel).ShouldBeTrue();

                    nextGradeLevel.ShouldNotBeNull();

                    var nextGradeNumber = nextGradeLevel.GetNumericGradeLevel();

                    nextGradeNumber.ShouldBe(gradeNumber + 1);
                }
                else
                {
                    GradeLevelDescriptor nextGradeLevel;
                    gradeLevel.TryGetNextK12GradeLevel(out nextGradeLevel).ShouldBeFalse();

                    nextGradeLevel.ShouldBeNull();
                }
            }
        }
    }
}
