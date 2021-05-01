using System;
using System.Collections.Generic;
using System.Linq;
using EdFi.SampleDataGenerator.Core.Serialization.CsvHelper;

namespace EdFi.SampleDataGenerator.Core.Config.DataFiles
{
    public abstract class InterchangeFileReaderBase<TResult> : IInterchangeFileReader<TResult>
    {
        public abstract TResult Read(ISampleDataGeneratorConfig config);
        protected abstract Func<ISampleDataGeneratorConfig, IInterchangeEntityFileMapping[]> GetFileMappingsFunc { get; } 

        protected List<TEntity> ReadEntityFile<TEntity>(ISampleDataGeneratorConfig config)
        {
            var fileMappings = GetFileMappingsFunc(config);
            var entityType = typeof (TEntity);
            var filePath = fileMappings?.SingleOrDefault(f => f.EntityType == entityType)?.FilePath;

            return MappedCsvFileReader.ReadEntityFile<TEntity>(filePath);
        }
    }
}