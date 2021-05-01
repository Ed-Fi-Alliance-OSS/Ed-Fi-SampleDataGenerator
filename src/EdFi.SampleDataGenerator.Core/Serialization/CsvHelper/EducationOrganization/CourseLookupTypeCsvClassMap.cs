using CsvHelper.Configuration;
using EdFi.SampleDataGenerator.Core.Entities;

namespace EdFi.SampleDataGenerator.Core.Serialization.CsvHelper.EducationOrganization
{
    public sealed class CourseLookupTypeCsvClassMap : CsvClassMap<CourseLookupType>
    {
        public CourseLookupTypeCsvClassMap()
        {
            Map(x => x.CourseCode);
            Map(x => x.CourseTitle);
            References<CourseIdentificationCodeCsvClassMap>(x => x.CourseIdentificationCode);
        }
    }
}