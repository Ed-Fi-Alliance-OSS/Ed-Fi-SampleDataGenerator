using System.Linq;
using CsvHelper.Configuration;

namespace EdFi.SampleDataGenerator.Core.Serialization.CsvHelper
{
    public static class CsvPropertyMapHelpers
    {
        public static string GetHeaderName(this CsvPropertyMap propertyMap)
        {
            return propertyMap.Data.Names.FirstOrDefault() ?? propertyMap.Data.Property.Name;
        }

        public static string GetArrayEntryHeaderName(this CsvPropertyMap propertyMap, int index)
        {
            var unindexedHeaderName = propertyMap.GetHeaderName();
            var headerParts = unindexedHeaderName.Split('.');
            headerParts[headerParts.Length - 2] = $"{headerParts[headerParts.Length - 2]}[{index}]";

            return string.Join(".", headerParts);
        }
    }
}
