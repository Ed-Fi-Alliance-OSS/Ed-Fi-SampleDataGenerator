using CsvHelper.Configuration;
using EdFi.SampleDataGenerator.Core.Entities;

namespace EdFi.SampleDataGenerator.Core.Serialization.CsvHelper.EducationOrganization
{
    public sealed class LearningStandardIdentificationCodeCsvClassMap : CsvClassMap<LearningStandardIdentificationCode>
    {
        public LearningStandardIdentificationCodeCsvClassMap()
        {
            Map(x => x.ContentStandardName);
            Map(x => x.IdentificationCode);
        }
    }
}