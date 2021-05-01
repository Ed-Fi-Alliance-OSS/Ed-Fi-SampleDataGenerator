using CsvHelper.Configuration;
using EdFi.SampleDataGenerator.Core.Entities;

namespace EdFi.SampleDataGenerator.Core.Serialization.CsvHelper
{
    public abstract class ReferenceTypeCsvClassMap<TEntity> : CsvClassMap<TEntity>
        where TEntity: ReferenceType
    {
        protected ReferenceTypeCsvClassMap()
        {
            Map(x => x.id);
            Map(x => x.@ref);
        } 
    }
}
