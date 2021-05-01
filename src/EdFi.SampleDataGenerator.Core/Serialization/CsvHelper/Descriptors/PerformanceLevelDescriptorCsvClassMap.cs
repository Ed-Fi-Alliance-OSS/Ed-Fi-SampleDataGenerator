using CsvHelper.Configuration;
using EdFi.SampleDataGenerator.Core.Entities;

namespace EdFi.SampleDataGenerator.Core.Serialization.CsvHelper.Descriptors
{
    public sealed partial class PerformanceLevelDescriptorCsvClassMap : CsvClassMap<PerformanceLevelDescriptor>
    {
        public PerformanceLevelDescriptorCsvClassMap()
        {
            Map(m => m.CodeValue);
            Map(m => m.Description);
            Map(m => m.Namespace);
            Map(m => m.ShortDescription);
            Map(m => m.EffectiveBeginDate);
            Map(m => m.EffectiveEndDate);
            ExtensionMappings();
        }
    }
}
