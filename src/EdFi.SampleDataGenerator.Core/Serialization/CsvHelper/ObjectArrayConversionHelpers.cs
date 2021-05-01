using System;

namespace EdFi.SampleDataGenerator.Core.Serialization.CsvHelper
{
    public static class ObjectArrayConversionHelpers
    {
        public static object GetUnderlyingValue(object value)
        {
            if (value == null) return null;
            if (!value.GetType().IsArray) return value;

            var arrayValue = (Array)value;
            return arrayValue.Length == 0
                ? null
                : arrayValue.GetValue(0);
        }
    }
}
