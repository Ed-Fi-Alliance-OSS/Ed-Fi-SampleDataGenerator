using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CsvHelper;
using CsvHelper.Configuration;

namespace EdFi.SampleDataGenerator.Core.Serialization.CsvHelper
{
    public class InterchangeCsvWriter
    {
        private readonly CsvClassMap _csvClassMap;
        private readonly CsvSerializer _serializer;

        public string Delimiter { get; set; } = ",";
        public char QuoteChar { get; set; } = '"';
        public string QuoteString => QuoteChar.ToString();
        public string DoubleQuoteString => $"{QuoteChar}{QuoteChar}";

        public InterchangeCsvWriter(CsvClassMap csvClassMap, TextWriter textWriter)
        {
            _csvClassMap = csvClassMap;
            _serializer = new CsvSerializer(textWriter);
        }

        private string[] WriteHeader()
        {
            var header = new List<string>();
            BuildHeader(_csvClassMap, header);

            var result = header.ToArray();
            _serializer.Write(result);

            return result;
        }

        private static void BuildHeader(CsvClassMap classMap, List<string> header)
        {
            foreach (var propertyMap in classMap.PropertyMaps)
            {
                AddHeaderForProperty(propertyMap, header);
            }

            foreach (var referenceMap in classMap.ReferenceMaps)
            {
                BuildHeader(referenceMap.Data.Mapping, header);
            }
        }

        private static void AddHeaderForProperty(CsvPropertyMap propertyMap, ICollection<string> header)
        {
            if (propertyMap.Data.Ignore) return;
            header.Add(propertyMap.GetHeaderName());
        }

        private void BuildRow(CsvClassMap classMap, object record, List<string> row)
        {
            foreach (var propertyMap in classMap.PropertyMaps)
            {
                if (propertyMap.Data.Ignore) continue;

                var columValue = GetOutputValueForColumn(propertyMap, record);
                row.Add(columValue);
            }

            foreach (var referenceMap in classMap.ReferenceMaps)
            {
                var underlyingValue = ObjectArrayConversionHelpers.GetUnderlyingValue(record);

                if (underlyingValue == null)
                {
                    BuildRow(referenceMap.Data.Mapping, null, row);
                }

                else
                {
                    var subEntity = referenceMap.Data.Property.GetValue(underlyingValue);
                    BuildRow(referenceMap.Data.Mapping, subEntity, row);
                }
            }
        }

        private string[] WriteRecord(object record)
        {
            var row = new List<string>();
            BuildRow(_csvClassMap, record, row);

            var result = row.ToArray();
            _serializer.Write(result);

            return result;
        }

        private string GetOutputValueForColumn(CsvPropertyMap propertyMap, object record)
        {
            if (record == null) return "";

            var isSpecified = CsvClassMapHelpers.GetIsSpecifiedMemberValue(propertyMap.Data.Property, record);
            if (isSpecified.HasValue && isSpecified.Value == false)
                return "";

            string columnValue;

            if (propertyMap.Data.Property.PropertyType.IsArray)
            {
                var outputProperty = ((IEnumerable)propertyMap.Data.Property.GetValue(record))?.Cast<object>().FirstOrDefault();
                var outputPropertyType = outputProperty?.GetType();
                if ((outputPropertyType != null && outputPropertyType.IsPrimitive) || outputPropertyType == typeof(string))
                {
                    columnValue = outputProperty.ToString();
                }
                else
                {
                    columnValue = propertyMap.Data.TypeConverter.ConvertToString(outputProperty, null, propertyMap.Data);
                }
            }
            else
            {
                var outputProperty = record.GetType().IsArray
                        ? propertyMap.Data.Property.GetValue(((IEnumerable)record).Cast<object>().FirstOrDefault())
                        : propertyMap.Data.Property.GetValue(record);

                columnValue = propertyMap.Data.TypeConverter.ConvertToString(outputProperty, null, propertyMap.Data);
            }

            if (!string.IsNullOrEmpty(columnValue))
            {
                columnValue = columnValue.Trim();
                columnValue = columnValue.Replace(QuoteString, DoubleQuoteString);
            }

            if (ShouldQuote(columnValue))
            {
                columnValue = QuoteChar + columnValue + QuoteChar;
            }

            return columnValue;
        }

        private bool ShouldQuote(string value)
        {
            return !string.IsNullOrEmpty(value) &&
                (value.Contains(QuoteString) // Contains quote
                   || value[0] == ' ' // Starts with a space
                   || value[value.Length - 1] == ' ' // Ends with a space
                   || (Delimiter.Length > 0 && value.Contains(Delimiter)) // Contains delimiter
                );
        }

        public void WriteRecords(IEnumerable<object> entitiesToOutput)
        {
            WriteHeader();
            foreach (var entity in entitiesToOutput)
            {
                WriteRecord(entity);
            }
        }
    }
}
