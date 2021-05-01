using CsvHelper.Configuration;
using EdFi.SampleDataGenerator.Core.Entities;

namespace EdFi.SampleDataGenerator.Core.Serialization.CsvHelper.Descriptors
{
    public sealed partial class CompetencyLevelDescriptorCsvClassMap : CsvClassMap<CompetencyLevelDescriptor>
    {
        public CompetencyLevelDescriptorCsvClassMap()
        {
            Map(m => m.CodeValue);
            Map(m => m.Description);
            Map(m => m.Namespace);
            Map(m => m.ShortDescription);
            Map(m => m.PriorDescriptor);
            Map(m => m.EffectiveBeginDate);
            Map(m => m.EffectiveEndDate);
            ExtensionMappings();
        }
    }

    public sealed class CompetencyLevelDescriptorReferenceTypeCsvClassMap : DescriptorReferenceTypeCsvClassMap<CompetencyLevelDescriptorReferenceType>
    {
    }
}