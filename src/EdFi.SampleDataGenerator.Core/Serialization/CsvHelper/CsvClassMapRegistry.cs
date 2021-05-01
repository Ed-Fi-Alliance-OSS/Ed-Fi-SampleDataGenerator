using System;
using System.Collections.Generic;
using System.Linq;
using CsvHelper.Configuration;

namespace EdFi.SampleDataGenerator.Core.Serialization.CsvHelper
{
    public static class CsvClassMapRegistry
    {
        private static readonly Lazy<Dictionary<Type, CsvClassMap>> _csvClassMaps = new Lazy<Dictionary<Type, CsvClassMap>>(GetCsvClassMapDictionary);
        public static CsvClassMap[] CsvClassMaps => _csvClassMaps.Value.Values.ToArray();
        
        public static CsvClassMap MapFor(Type t)
        {
            return _csvClassMaps.Value.ContainsKey(t) ? _csvClassMaps.Value[t] : null;
        }

        private static Dictionary<Type, CsvClassMap> GetCsvClassMapDictionary()
        {
            var result = CsvClassMapHelpers.GetTypeToCsvClassMapInstanceDictionary();

            var descriptorMapTypes = DescriptorTypeCsvClassMapFactory.GetCsvClassMaps();
            descriptorMapTypes.ToList().ForEach(x => result.Add(x.Key, x.Value));

            return result;
        }
    }
}
