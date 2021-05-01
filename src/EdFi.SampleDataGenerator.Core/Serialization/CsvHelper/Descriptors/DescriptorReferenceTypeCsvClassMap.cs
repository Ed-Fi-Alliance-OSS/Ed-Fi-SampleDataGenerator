using CsvHelper.Configuration;
using EdFi.SampleDataGenerator.Core.Entities;

namespace EdFi.SampleDataGenerator.Core.Serialization.CsvHelper.Descriptors
{
    public abstract class DescriptorReferenceTypeCsvClassMap<TDescriptorReferenceType> : CsvClassMap<TDescriptorReferenceType>
        where TDescriptorReferenceType: DescriptorReferenceType
    {
        protected DescriptorReferenceTypeCsvClassMap()
        {
            Map(m => m.CodeValue);
            Map(m => m.Namespace);
        } 
    }
}
