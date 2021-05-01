namespace EdFi.SampleDataGenerator.Core.Serialization.CsvHelper
{
    public class CsvHeaderPrefixOverride
    {
        public string HeaderPrefix { get; set; }
        public string PrefixOverride { get; set; }
    }

    public static class CsvHeaderPrefixOverrideHelpers
    {
        public static string OverrideHeaderName(this CsvHeaderPrefixOverride csvHeaderPrefixOverride, string headerName)
        {
            return csvHeaderPrefixOverride == null 
                ? headerName 
                : headerName.Replace(csvHeaderPrefixOverride.HeaderPrefix, csvHeaderPrefixOverride.PrefixOverride);
        }

        public static string GetNewArrayPrefixOverride(this CsvHeaderPrefixOverride csvHeaderPrefixOverride, string headerName, int arrayIndex)
        {
            var overrideHeaderName = csvHeaderPrefixOverride.OverrideHeaderName(headerName).TrimEnd('.');
            return $"{overrideHeaderName}[{arrayIndex}].";
        }
    }
}