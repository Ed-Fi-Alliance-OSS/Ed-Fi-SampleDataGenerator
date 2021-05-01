using CsvHelper.Configuration;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common.Mutators;

namespace EdFi.SampleDataGenerator.Core.Serialization.CsvHelper.Mutators
{
    public sealed class MutationLogEntryCsvClassMap : CsvClassMap<MutationLogEntry>
    {
        public MutationLogEntryCsvClassMap()
        {
            AutoMap();
            Map(x => x.MutationType);
            Map(x => x.OldValue).TypeConverter<JsonCsvTypeConverter>();
            Map(x => x.NewValue).TypeConverter<JsonCsvTypeConverter>();
        }
    }
}