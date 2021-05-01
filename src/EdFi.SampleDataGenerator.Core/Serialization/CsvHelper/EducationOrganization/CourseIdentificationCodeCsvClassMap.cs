using CsvHelper.Configuration;
using EdFi.SampleDataGenerator.Core.Entities;

namespace EdFi.SampleDataGenerator.Core.Serialization.CsvHelper.EducationOrganization
{
    public sealed class CourseIdentificationCodeCsvClassMap : CsvClassMap<CourseIdentificationCode>
    {
        public CourseIdentificationCodeCsvClassMap()
        {
            Map(x => x.AssigningOrganizationIdentificationCode);
            Map(x => x.IdentificationCode);
            Map(x => x.CourseIdentificationSystem);
        }
    }
}