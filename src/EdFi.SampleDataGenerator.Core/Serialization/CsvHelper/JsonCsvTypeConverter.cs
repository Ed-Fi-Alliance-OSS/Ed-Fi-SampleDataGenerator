using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using Newtonsoft.Json;

namespace EdFi.SampleDataGenerator.Core.Serialization.CsvHelper
{
    public class JsonCsvTypeConverter : ITypeConverter
    {
        public string ConvertToString(object value, ICsvWriterRow row, CsvPropertyMapData propertyMapData)
        {
            return JsonConvert.SerializeObject(value);
        }

        public object ConvertFromString(string text, ICsvReaderRow row, CsvPropertyMapData propertyMapData)
        {
            return JsonConvert.DeserializeObject(text);
        }
    }
}