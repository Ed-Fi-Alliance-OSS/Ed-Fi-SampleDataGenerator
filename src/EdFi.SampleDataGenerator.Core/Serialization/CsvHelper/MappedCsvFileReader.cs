using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CsvHelper.Configuration;

namespace EdFi.SampleDataGenerator.Core.Serialization.CsvHelper
{
    public static class MappedCsvFileReader
    {
        public static List<TEntity> ReadEntityFile<TEntity>(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
                return new List<TEntity>();

            using (var textReader = new StreamReader(filePath))
            {
                try
                {
                    return ReadEntityFile<TEntity>(textReader);
                }
                catch (Exception e)
                {
                    throw new Exception($"Error while parsing {typeof(TEntity).Name} out of '{filePath}'.  See InnerException for details.", e);
                }
            }
        }

        public static List<TEntity> ReadEntityFile<TEntity>(TextReader textReader)
        {
            var entityType = typeof(TEntity);
            var csvClassMap = CsvClassMapRegistry.MapFor(entityType);

            if (csvClassMap == null)
                throw new ArgumentException($"No class map found for entity of type {entityType.Name}");

            return ReadEntityFile<TEntity>(textReader, csvClassMap);
        }

        public static List<TEntity> ReadEntityFile<TEntity>(TextReader textReader, CsvClassMap csvClassMap)
        {
            if (csvClassMap == null)
                throw new ArgumentNullException(nameof(csvClassMap), "CsvClassMap may not be null");

            try
            {
                var csvReader = new InterchangeCsvReader(csvClassMap, textReader);
                var data = csvReader.ReadRecords(typeof(TEntity))
                    .Select(x => (TEntity)x)
                    .ToList();

                return data;
            }
            catch (Exception e)
            {
                throw new Exception($"Error while parsing {typeof(TEntity).Name} entities using {csvClassMap.GetType().Name}. See InnerException for details.", e);
            }
        }
    }
}