using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using EdFi.SampleDataGenerator.Core.Helpers;

namespace EdFi.SampleDataGenerator.Core.Serialization.CsvHelper
{
    public class EnumerationTypeConverter<TEnum> : ITypeConverter
        where TEnum: struct
    {
        public string ConvertToString(object value, ICsvWriterRow row, CsvPropertyMapData propertyMapData)
        {
            var underlyingValue = ObjectArrayConversionHelpers.GetUnderlyingValue(value);
            if (underlyingValue == null) return "";

            return ((TEnum) underlyingValue).ToCodeValue();
        }

        public object ConvertFromString(string text, ICsvReaderRow row, CsvPropertyMapData propertyMapData)
        {
            return EnumHelpers.Parse<TEnum>(text);
        }
    }
}