using CsvHelper.Configuration;
using EdFi.SampleDataGenerator.Core.Entities;

namespace EdFi.SampleDataGenerator.Core.Serialization.CsvHelper
{
    public abstract class ComplexObjectTypeCsvClassMap<TEntity> : CsvClassMap<TEntity>
        where TEntity : ComplexObjectType
    {
        protected ComplexObjectTypeCsvClassMap()
        {
            Map(x => x.id);
        }
    }
}