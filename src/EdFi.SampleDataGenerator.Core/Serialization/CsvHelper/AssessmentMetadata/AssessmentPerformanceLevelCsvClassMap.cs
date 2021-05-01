using CsvHelper.Configuration;
using EdFi.SampleDataGenerator.Core.Entities;

namespace EdFi.SampleDataGenerator.Core.Serialization.CsvHelper.AssessmentMetadata
{
    public sealed class AssessmentPerformanceLevelCsvClassMap : CsvClassMap<AssessmentPerformanceLevel>
    {
        public AssessmentPerformanceLevelCsvClassMap()
        {
            Map(x => x.MinimumScore);
            Map(x => x.MaximumScore);
            Map(x => x.AssessmentReportingMethod);
            Map(x => x.PerformanceLevel);
        }
    }
}