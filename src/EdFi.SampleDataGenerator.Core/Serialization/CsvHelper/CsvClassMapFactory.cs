using System;
using CsvHelper.Configuration;

namespace EdFi.SampleDataGenerator.Core.Serialization.CsvHelper
{
    public static class CsvClassMapFactory
    {
        public static CsvClassMap GetCsvClassMapFor(Type classMapType)
        {
            var instance = (CsvClassMap)Activator.CreateInstance(classMapType);
            instance.RecursivePrefixReferencesMaps();

            return instance;
        }
    }
}
