using CsvHelper.Configuration;
using EdFi.SampleDataGenerator.Core.Entities;
using EdFi.SampleDataGenerator.Core.Serialization.CsvHelper.EducationOrganization;
using EdFi.SampleDataGenerator.Core.Serialization.CsvHelper.EducationOrgCalendar;

namespace EdFi.SampleDataGenerator.Core.Serialization.CsvHelper.MasterSchedule
{
    public sealed class CourseOfferingIdentityTypeCsvClassMap : CsvClassMap<CourseOfferingIdentityType>
    {
        public CourseOfferingIdentityTypeCsvClassMap()
        {
            Map(x => x.LocalCourseCode);
            References<SessionReferenceTypeCsvClassMap>(x => x.SessionReference);
            References<SchoolReferenceTypeCsvClassMap>(x => x.SchoolReference);
        }
    }
}