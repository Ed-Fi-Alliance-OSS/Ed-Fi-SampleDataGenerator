using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using EdFi.SampleDataGenerator.Core.Entities;
using EdFi.SampleDataGenerator.Core.Helpers;

namespace EdFi.SampleDataGenerator.Core.Serialization.CsvHelper
{
    public class DescriptorTypeConverter<TDescriptor> : ITypeConverter
        where TDescriptor: DescriptorType
    {
        public string ConvertToString(object value, ICsvWriterRow row, CsvPropertyMapData propertyMapData)
        {
            var underlyingValue = ObjectArrayConversionHelpers.GetUnderlyingValue(value) as TDescriptor;
            if (underlyingValue == null) return "";

            return underlyingValue.CodeValue;
        }

        public object ConvertFromString(string text, ICsvReaderRow row, CsvPropertyMapData propertyMapData)
        {
            TDescriptor descriptor;

            if (DescriptorHelpers.TryParseFromCodeValue<TDescriptor>(text, out descriptor))
                return descriptor;

            return null;
        }
    }
}