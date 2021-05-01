using EdFi.SampleDataGenerator.Core.Entities;

namespace EdFi.SampleDataGenerator.Core.Serialization.CsvHelper.EducationOrganization
{
    public sealed class CourseReferenceTypeCsvClassMap : ReferenceTypeCsvClassMap<CourseReferenceType>
    {
        public CourseReferenceTypeCsvClassMap()
        {
            References<CourseIdentityTypeCsvClassMap>(x => x.CourseIdentity);
            References<CourseLookupTypeCsvClassMap>(x => x.CourseLookup);
        }
    }
}
