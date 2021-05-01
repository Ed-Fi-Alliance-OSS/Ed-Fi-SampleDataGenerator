using System.Linq;
using EdFi.SampleDataGenerator.Core.Entities;
using EdFi.SampleDataGenerator.Core.Serialization.Output.Interchanges;

namespace EdFi.SampleDataGenerator.Core.Helpers
{
    public static class EducationOrganizationDataExtensions
    {
        public static Course LookupCourse(this EducationOrganizationData educationOrgData, Section section)
        {
            var localCourseCode = section
                .CourseOfferingReference
                .CourseOfferingIdentity
                .LocalCourseCode;

            return educationOrgData.Courses.FirstOrDefault(course => course.CourseIdentificationCode.Any(id => id.IdentificationCode == localCourseCode));
        }
    }
}