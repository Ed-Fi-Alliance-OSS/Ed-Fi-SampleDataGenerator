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
            SampleCourse(Advanced.CodeValue).RestrictedToHighPerformingStudents().ShouldBe(true);
            SampleCourse(AdvancedPlacement.CodeValue).RestrictedToHighPerformingStudents().ShouldBe(true);
            SampleCourse(DualCredit.CodeValue).RestrictedToHighPerformingStudents().ShouldBe(true);
            SampleCourse(Honors.CodeValue).RestrictedToHighPerformingStudents().ShouldBe(true);
            SampleCourse(InternationalBaccalaureate.CodeValue).RestrictedToHighPerformingStudents().ShouldBe(true);
            SampleCourse(PreAP.CodeValue).RestrictedToHighPerformingStudents().ShouldBe(true);
            SampleCourse(PreIB.CodeValue).RestrictedToHighPerformingStudents().ShouldBe(true);
            SampleCourse(GiftedAndTalented.CodeValue).RestrictedToHighPerformingStudents().ShouldBe(true);

            //Characteristics which do not indicate high-performing students.
            SampleCourse(AcceptedAsHighSchoolEquivalent.CodeValue).RestrictedToHighPerformingStudents().ShouldBe(false);
            SampleCourse(Basic.CodeValue).RestrictedToHighPerformingStudents().ShouldBe(false);
            SampleCourse(CollegeLevel.CodeValue).RestrictedToHighPerformingStudents().ShouldBe(false);
            SampleCourse(CoreSubject.CodeValue).RestrictedToHighPerformingStudents().ShouldBe(false);
            SampleCourse(Correspondence.CodeValue).RestrictedToHighPerformingStudents().ShouldBe(false);
            SampleCourse(CareerAndTechnicalEducation.CodeValue).RestrictedToHighPerformingStudents().ShouldBe(false);
            SampleCourse(DistanceLearning.CodeValue).RestrictedToHighPerformingStudents().ShouldBe(false);
            SampleCourse(EnglishLanguageLearner.CodeValue).RestrictedToHighPerformingStudents().ShouldBe(false);
            SampleCourse(General.CodeValue).RestrictedToHighPerformingStudents().ShouldBe(false);
            SampleCourse(GraduationCredit.CodeValue).RestrictedToHighPerformingStudents().ShouldBe(false);
            SampleCourse(Magnet.CodeValue).RestrictedToHighPerformingStudents().ShouldBe(false);
            SampleCourse(Remedial.CodeValue).RestrictedToHighPerformingStudents().ShouldBe(false);
            SampleCourse(StudentsWithDisabilities.CodeValue).RestrictedToHighPerformingStudents().ShouldBe(false);
            SampleCourse(Untracked.CodeValue).RestrictedToHighPerformingStudents().ShouldBe(false);
            SampleCourse(Other.CodeValue).RestrictedToHighPerformingStudents().ShouldBe(false);

            //When *any* course characteristic indicates high-performing students, the course applies to those students:
            SampleCourse(Other.CodeValue, Untracked.CodeValue, DistanceLearning.CodeValue, GiftedAndTalented.CodeValue).RestrictedToHighPerformingStudents().ShouldBe(true);
        }

        private static Course SampleCourse(params string[] characteristics)
        {
            return new Course {CourseLevelCharacteristic = characteristics};
        }
    }
}
