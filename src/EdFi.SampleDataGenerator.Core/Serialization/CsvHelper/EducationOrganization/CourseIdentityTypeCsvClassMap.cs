using CsvHelper.Configuration;
using EdFi.SampleDataGenerator.Core.Entities;

namespace EdFi.SampleDataGenerator.Core.Serialization.CsvHelper.EducationOrganization
{
    public sealed class CourseIdentityTypeCsvClassMap : CsvClassMap<CourseIdentityType>
    {
        public CourseIdentityTypeCsvClassMap()
        {
            Map(x => x.CourseCode);
            References<EducationOrganizationReferenceTypeCsvClassMap>(x => x.EducationOrganizationReference);
        }
    }
}