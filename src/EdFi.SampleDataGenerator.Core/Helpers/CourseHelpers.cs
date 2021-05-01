using System.Linq;
using EdFi.SampleDataGenerator.Core.Entities;

namespace EdFi.SampleDataGenerator.Core.Helpers
{
    public static class CourseHelpers
    {
        public static bool RestrictedToHighPerformingStudents(this Course course)
        {
            return course.CourseLevelCharacteristic.Any(RestrictedToHighPerformingStudents);
        }

        private static bool RestrictedToHighPerformingStudents(
            this string courseLevelCharacteristic)
        {
            var list = new[]
            {
                CourseLevelCharacteristicDescriptor.Advanced.CodeValue,
                CourseLevelCharacteristicDescriptor.AdvancedPlacement.CodeValue,
                CourseLevelCharacteristicDescriptor.DualCredit.CodeValue,
                CourseLevelCharacteristicDescriptor.Honors.CodeValue,
                CourseLevelCharacteristicDescriptor.InternationalBaccalaureate.CodeValue,
                CourseLevelCharacteristicDescriptor.PreAP.CodeValue,
                CourseLevelCharacteristicDescriptor.PreIB.CodeValue,
                CourseLevelCharacteristicDescriptor.GiftedAndTalented.CodeValue
            };

            return list.Contains(courseLevelCharacteristic);
        }
    }
}