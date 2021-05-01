using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using CsvHelper;
using CsvHelper.Configuration;
using EdFi.SampleDataGenerator.Core.Helpers;

namespace EdFi.SampleDataGenerator.Core.Serialization.CsvHelper
{
    public class InterchangeCsvReader
    {
        private readonly CsvClassMap _csvClassMap;
        private string[] _headerRow = new string[0];
        private Dictionary<string, int> _headerIndex; 
        private readonly CsvParser _csvParser;

        public InterchangeCsvReader(CsvClassMap csvClassMap, TextReader textReader)
        {
            _csvClassMap = csvClassMap;
            _csvParser = new CsvParser(textReader);
        }

        private void ReadHeaderRow()
        {
            _headerRow = _csvParser.Read();
            _headerIndex = BuildHeaderIndex(_headerRow);
        }

        private static Dictionary<string, int> BuildHeaderIndex(string[] headerRow)
        {
            var headers = headerRow.GroupBy(x => x).Select(group => new {Header = group.Key, Count = group.Count()}).ToList();
            if (headers.Any(h => h.Count > 1))
            {
                var duplicateHeaders = string.Join(",", headers.Where(x => x.Count > 1).Select(x => x.Header));
                throw new FormatException($"Duplicate columns detected: {duplicateHeaders}");
            }
            
            return Enumerable.Range(0, headerRow.Length).ToDictionary(x => headerRow[x]);
        }

        private bool CanPossiblyConvert(string columnValue, CsvPropertyMap propertyMap)
        {
            if (string.IsNullOrWhiteSpace(columnValue))
            {
                return propertyMap.Data.Property.PropertyType.CanBeNull();
            }

            return true;
        }

        private bool PopulateEntity(CsvClassMap csvClassMap, string[] record, object entityToPopulate, CsvHeaderPrefixOverride headerPrefixOverride = null)
        {
            var populatedEntityField = false;
            foreach (var propertyMap in csvClassMap.PropertyMaps)
            {
                if (propertyMap.Data.Ignore) continue;
                
                if (PopulateEntityProperty(propertyMap, record, entityToPopulate, headerPrefixOverride))
                    populatedEntityField = true;
            }

            foreach (var referenceMap in csvClassMap.ReferenceMaps)
            {
                var subEntity = CreateReferencedEntity(referenceMap, record, headerPrefixOverride);

                if (subEntity == null || (subEntity.GetType().IsArray && ((Array)subEntity).Length == 0)) continue;

                populatedEntityField = true;
                referenceMap.SetProperty(entityToPopulate, subEntity);
            }

            return populatedEntityField;
        }

        private bool PopulateEntityProperty(CsvPropertyMap propertyMap, string[] record, object parentObjectWithPropertyToPopulate, CsvHeaderPrefixOverride headerPrefixOverride)
        {
            var propertyType = propertyMap.Data.Property.PropertyType;
            var headerName = headerPrefixOverride.OverrideHeaderName(propertyMap.GetHeaderName());

            if (propertyType.IsArray)
            {
                var elementType = propertyType.GetElementType();

                var elementListType = typeof(List<>).MakeGenericType(elementType);
                IList elementList = (IList)Activator.CreateInstance(elementListType);

                var index = 0;
                var element = elementType == typeof(string) ? "" :  Activator.CreateInstance(elementType);
                if (PopulateProperty(propertyMap, record, ref element, headerName))
                {
                    ++index;
                    elementList.Add(element);
                }

                do
                {
                    var arrayHeaderName = $"{headerName}[{index}]";
                    element = elementType == typeof(string) ? "" : Activator.CreateInstance(elementType);
                    if (PopulateProperty(propertyMap, record, ref element, arrayHeaderName))
                    {
                        ++index;
                        elementList.Add(element);
                    }

                    else
                    {
                        break;
                    }

                } while (true);

                if (elementList.Count == 0) return false;

                var entityArray = Array.CreateInstance(elementType, elementList.Count);
                elementList.CopyTo(entityArray, 0);

                propertyMap.SetProperty(ref parentObjectWithPropertyToPopulate, entityArray);

                return true;
            }

            return PopulateProperty(propertyMap, record, ref parentObjectWithPropertyToPopulate, headerName);
        }

        private bool PopulateProperty(CsvPropertyMap propertyMap, string[] record, ref object parentObjectWithPropertyToPoulate, string headerName)
        {
            if (_headerIndex.ContainsKey(headerName))
            {
                var headerIndex = _headerIndex[headerName];
                if (headerIndex >= record.Length) return false;

                var columnValue = record[headerIndex];
                propertyMap.TypeConverterOption.CultureInfo(CultureInfo.CurrentCulture);

                //sometimes CSV files will leave a column blank for a value type (say, an integer Id) field
                //in these cases, trying to convert an empty string to a value type will fail, so we'll
                //just skip these and allow the entity to maintain the default value for the current property
                if (!CanPossiblyConvert(columnValue, propertyMap)) return false;

                var propertyValue = ConvertFromString(propertyMap, columnValue, headerName);
                if (propertyValue == null) return false;

                propertyMap.SetProperty(ref parentObjectWithPropertyToPoulate, propertyValue);

                return true;
            }

            return false;
        }

        private object CreateReferencedEntity(CsvPropertyReferenceMap referenceMap, string[] record, CsvHeaderPrefixOverride headerPrefixOverride)
        {
            var subEntityType = referenceMap.Data.Property.PropertyType;

            if (subEntityType.IsArray)
            {
                subEntityType = subEntityType.GetElementType();

                var entityListType = typeof (List<>).MakeGenericType(subEntityType);
                IList entityList = (IList)Activator.CreateInstance(entityListType);

                var index = 0;
                var subEntity = Activator.CreateInstance(subEntityType);
                if (PopulateEntity(referenceMap.Data.Mapping, record, subEntity))
                {
                    ++index;
                    entityList.Add(subEntity);
                }

                do
                {
                    subEntity = Activator.CreateInstance(subEntityType);
                    var newHeaderPrefixOverride = new CsvHeaderPrefixOverride
                    {
                        HeaderPrefix = referenceMap.Data.Prefix,
                        PrefixOverride = headerPrefixOverride.GetNewArrayPrefixOverride(referenceMap.Data.Prefix, index)
                    };

                    if (PopulateEntity(referenceMap.Data.Mapping, record, subEntity, newHeaderPrefixOverride))
                    {
                        ++index;
                        entityList.Add(subEntity);
                    }

                    else
                    {
                        break;
                    }

                } while (true);

                if (entityList.Count > 0)
                {
                    var entityArray = Array.CreateInstance(subEntityType, entityList.Count);
                    entityList.CopyTo(entityArray, 0);

                    return entityArray;
                }

                return null;
            }

            else
            {
                var subEntity = Activator.CreateInstance(subEntityType);
                if (PopulateEntity(referenceMap.Data.Mapping, record, subEntity, headerPrefixOverride))
                {
                    return subEntity;
                }

                return null;
            }
        }

        private static object ConvertFromString(CsvPropertyMap propertyMap, string columnValue, string headerName)
        {
            var propertyType = propertyMap.Data.Property.PropertyType;

            try
            {
                if (propertyType.IsArray && propertyType.FullName == "System.String[]")
                {
                    return columnValue;
                }
                var result = propertyType.CanBeNull() && string.IsNullOrEmpty(columnValue)
                    ? null
                    : propertyMap.Data.TypeConverter.ConvertFromString(columnValue, null, propertyMap.Data);

                return result;
            }

            catch (Exception e)
            {
                throw new CsvException($"Failed to convert '{columnValue}' to type {propertyType.Name} in column '{headerName}'", e);
            }
        }

        private object CreateEntity(Type entityType, string[] record)
        {
            var entity = Activator.CreateInstance(entityType);
            PopulateEntity(_csvClassMap, record, entity);

            return entity;
        }

        public IEnumerable<object> ReadRecords(Type recordType)
        {
            ReadHeaderRow();

            var finishedReading = false;
            do
            {
                var record = _csvParser.Read();
                if (record == null) yield break;
                
                var entity = CreateEntity(recordType, record);
                yield return entity;

                finishedReading = record == null;

            } while (!finishedReading);
        }
    }
}
