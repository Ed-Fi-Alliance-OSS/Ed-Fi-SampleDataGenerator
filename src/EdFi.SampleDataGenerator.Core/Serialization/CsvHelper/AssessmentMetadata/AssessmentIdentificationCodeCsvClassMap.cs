using CsvHelper.Configuration;
using EdFi.SampleDataGenerator.Core.Entities;

namespace EdFi.SampleDataGenerator.Core.Serialization.CsvHelper.AssessmentMetadata
{
    public sealed class AssessmentIdentificationCodeCsvClassMap : CsvClassMap<AssessmentIdentificationCode>
    {
        public AssessmentIdentificationCodeCsvClassMap()
        {
            Map(x => x.IdentificationCode);
            Map(x => x.AssessmentIdentificationSystem);
        }
    }
}