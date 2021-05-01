using CsvHelper.Configuration;
using EdFi.SampleDataGenerator.Core.Entities;
using EdFi.SampleDataGenerator.Core.Serialization.CsvHelper.EducationOrganization;
using EdFi.SampleDataGenerator.Core.Serialization.CsvHelper.EducationOrgCalendar;

namespace EdFi.SampleDataGenerator.Core.Serialization.CsvHelper.MasterSchedule
{
    public sealed partial class CourseOfferingCsvClassMap : CsvClassMap<CourseOffering>
    {
        public CourseOfferingCsvClassMap()
        {
            Map(x => x.LocalCourseCode);
            Map(x => x.LocalCourseTitle);
            Map(x => x.InstructionalTimePlanned);
            References<SchoolReferenceTypeCsvClassMap>(x => x.SchoolReference);
            References<SessionReferenceTypeCsvClassMap>(x => x.SessionReference);
            References<CourseReferenceTypeCsvClassMap>(x => x.CourseReference);
            ExtensionMappings();
        }
    }
}