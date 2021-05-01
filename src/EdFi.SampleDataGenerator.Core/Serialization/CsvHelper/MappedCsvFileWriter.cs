using System;
using System.Collections.Generic;
using System.IO;
using CsvHelper.Configuration;
using EdFi.SampleDataGenerator.Core.Helpers;

namespace EdFi.SampleDataGenerator.Core.Serialization.CsvHelper
{
    public static class MappedCsvFileWriter
    {
        public static void WriteEntityFile<TEntity>(string filePath, IEnumerable<TEntity> entitiesToOutput)
        {
            try
            {
                using (var sw = new StreamWriter(filePath))
                {
                    WriteEntityFile<TEntity>(sw, entitiesToOutput);
                }
            }
            catch (Exception e)
            {
                throw new Exception($"Error while trying to output {typeof(TEntity).Name} entities to '{filePath}'. See InnerException for details.", e);
            }
        }

        public static void WriteEntityFile<TEntity>(TextWriter textWriter, IEnumerable<TEntity> entitiesToOutput)
        {
            var entityType = typeof(TEntity);
            var csvClassMap = CsvClassMapRegistry.MapFor(entityType);

            if (csvClassMap == null)
                throw new ArgumentException($"No class map found for entity of type {entityType.Name}");

            WriteEntityFile<TEntity>(textWriter, csvClassMap, entitiesToOutput);
        }

        public static void WriteEntityFile<TEntity>(TextWriter textWriter, CsvClassMap csvClassMap, IEnumerable<TEntity> entitiesToOutput)
        {
            if (csvClassMap == null)
                throw new ArgumentNullException(nameof(csvClassMap), "CsvClassMap may not be null");

            try
            {
                var csvWriter = new InterchangeCsvWriter(csvClassMap, textWriter);
                csvWriter.WriteRecords(entitiesToOutput.SafeCast<TEntity, object>());
            }
            catch (Exception e)
            {
                throw new Exception($"Error while writing {typeof(TEntity).Name} entities using {csvClassMap.GetType().Name}. See InnerException for details.", e);
            }
        }
    }
}
