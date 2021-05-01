using CsvHelper.Configuration;
using EdFi.SampleDataGenerator.Core.Entities;

namespace EdFi.SampleDataGenerator.Core.Serialization.CsvHelper.Standards
{
    public sealed class ContentStandardCsvClassMap : CsvClassMap<ContentStandard>
    {
        public ContentStandardCsvClassMap()
        {
            Map(x => x.Title);
        }
    }
}