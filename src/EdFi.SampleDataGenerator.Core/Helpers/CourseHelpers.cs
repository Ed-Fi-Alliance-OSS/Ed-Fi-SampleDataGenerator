using System.Linq;
using EdFi.SampleDataGenerator.Core.Entities;

namespace EdFi.SampleDataGenerator.Core.Helpers
{
    public static class CourseHelpers
    {
        private static readonly CourseLevelCharacteristicDescriptor[] HighPerformanceCourseLevelCharacteristics = {
            CourseLevelCharacteristicDescriptor.Advanced,
            CourseLevelCharacteristicDescriptor.AdvancedPlacement,
            CourseLevelCharacteristicDescriptor.DualCredit,
            CourseLevelCharacteristicDescriptor.Honors,
            CourseLevelCharacteristicDescriptor.InternationalBaccalaureate,
            CourseLevelCharacteristicDescriptor.PreAP,
            CourseLevelCharacteristicDescriptor.PreIB,
            CourseLevelCharacteristicDescriptor.GiftedAndTalented
        };

        public static bool RestrictedToHighPerformingStudents(this Course course)
        {
            return course.CourseLevelCharacteristic.Any(RestrictedToHighPerformingStudents);
        }

        private static bool RestrictedToHighPerformingStudents(this string courseLevelCharacteristic)
        {
            var descriptor = courseLevelCharacteristic.ParseFromStructuredCodeValue<CourseLevelCharacteristicDescriptor>();

            return HighPerformanceCourseLevelCharacteristics.Contains(descriptor);
        }
    }
}