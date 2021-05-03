using System.Linq;
using EdFi.SampleDataGenerator.Core.Entities;
using EdFi.SampleDataGenerator.Core.Helpers;
using NUnit.Framework;
using Shouldly;
using static EdFi.SampleDataGenerator.Core.Entities.CourseLevelCharacteristicDescriptor;

namespace EdFi.SampleDataGenerator.Core.UnitTests.Helpers
{
    [TestFixture]
    public class CourseHelpersTester
    {
        [Test]
        public void ShouldRestrictToHighPerformingStudentsWhenAnyCourseLevelCharacteristicAppliesToHighPerformingStudents()
        {
            //Characteristics which indicate high-performing students.
            SampleCourse(Advanced).RestrictedToHighPerformingStudents().ShouldBe(true);
            SampleCourse(AdvancedPlacement).RestrictedToHighPerformingStudents().ShouldBe(true);
            SampleCourse(DualCredit).RestrictedToHighPerformingStudents().ShouldBe(true);
            SampleCourse(Honors).RestrictedToHighPerformingStudents().ShouldBe(true);
            SampleCourse(InternationalBaccalaureate).RestrictedToHighPerformingStudents().ShouldBe(true);
            SampleCourse(PreAP).RestrictedToHighPerformingStudents().ShouldBe(true);
            SampleCourse(PreIB).RestrictedToHighPerformingStudents().ShouldBe(true);
            SampleCourse(GiftedAndTalented).RestrictedToHighPerformingStudents().ShouldBe(true);

            //Characteristics which do not indicate high-performing students.
            SampleCourse(AcceptedAsHighSchoolEquivalent).RestrictedToHighPerformingStudents().ShouldBe(false);
            SampleCourse(Basic).RestrictedToHighPerformingStudents().ShouldBe(false);
            SampleCourse(CollegeLevel).RestrictedToHighPerformingStudents().ShouldBe(false);
            SampleCourse(CoreSubject).RestrictedToHighPerformingStudents().ShouldBe(false);
            SampleCourse(Correspondence).RestrictedToHighPerformingStudents().ShouldBe(false);
            SampleCourse(CareerAndTechnicalEducation).RestrictedToHighPerformingStudents().ShouldBe(false);
            SampleCourse(DistanceLearning).RestrictedToHighPerformingStudents().ShouldBe(false);
            SampleCourse(EnglishLanguageLearner).RestrictedToHighPerformingStudents().ShouldBe(false);
            SampleCourse(General).RestrictedToHighPerformingStudents().ShouldBe(false);
            SampleCourse(GraduationCredit).RestrictedToHighPerformingStudents().ShouldBe(false);
            SampleCourse(Magnet).RestrictedToHighPerformingStudents().ShouldBe(false);
            SampleCourse(Remedial).RestrictedToHighPerformingStudents().ShouldBe(false);
            SampleCourse(StudentsWithDisabilities).RestrictedToHighPerformingStudents().ShouldBe(false);
            SampleCourse(Untracked).RestrictedToHighPerformingStudents().ShouldBe(false);
            SampleCourse(Other).RestrictedToHighPerformingStudents().ShouldBe(false);

            //When *any* course characteristic indicates high-performing students, the course applies to those students:
            SampleCourse(Other, Untracked, DistanceLearning, GiftedAndTalented).RestrictedToHighPerformingStudents().ShouldBe(true);
        }

        private static Course SampleCourse(params CourseLevelCharacteristicDescriptor[] characteristics)
        {
            return new Course
            {
                CourseLevelCharacteristic = characteristics.ToStructuredCodeValueArray()
            };
        }
    }
}
