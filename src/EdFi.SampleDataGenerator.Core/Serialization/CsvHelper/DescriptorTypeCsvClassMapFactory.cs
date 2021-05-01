using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using EdFi.SampleDataGenerator.Core.Entities;
using EdFi.SampleDataGenerator.Core.Helpers;

namespace EdFi.SampleDataGenerator.Core.Serialization.CsvHelper
{
    public static class DescriptorTypeCsvClassMapFactory
    {
        public static readonly string[] DefaultPropertiesToMap = 
        {
            "CodeValue",
            "ShortDescription",
            "Description",
            "Namespace"
        };

        private static readonly string[] PropertiesNotMapped =
        {
            "id",
            "EffectiveBeginDate",
            "EffectiveBeginDateSpecified",
            "EffectiveEndDate",
            "EffectiveEndDateSpecified",
            "PriorDescriptor",
            "{DescriptorName}MapSpecified",
        };

        public static string[] GetUnmappedPropertyNames(Type descriptorType)
        {
            var descriptorName = descriptorType.DescriptorNameFromType();
            return PropertiesNotMapped.Select(p => p.Replace("{DescriptorName}", descriptorName)).ToArray();
        }

        public static readonly IEnumerable<Type> ManualDescriptorTypeCsvClassMapMappings = CsvClassMapHelpers.ScanForCsvClassMapTypes()
            .Where(t => typeof(DescriptorType).IsAssignableFrom(t.CsvClassMapMappedType()));

        public static Dictionary<Type, CsvClassMap> GetCsvClassMaps()
        {
            var result = new Dictionary<Type, CsvClassMap>();
            var descriptorTypes = GetAutoMappedDescriptorTypes();

            foreach (var t in descriptorTypes)
            {
                var classMap = GetCsvClassMapFor(t);
                result.Add(t, classMap);
            }

            return result;
        }

        public static CsvClassMap GetCsvClassMapFor(Type t)
        {
            var descriptorType = typeof (DescriptorType);
            if (!descriptorType.IsAssignableFrom(t)) return null;

            var classMapType = typeof (DefaultCsvClassMap<>).MakeGenericType(t);
            var classMap = (CsvClassMap) Activator.CreateInstance(classMapType);

            MapDefaultProperties(t, classMap);

            var mapTypePropertyName = t.DescriptorMapTypePropertyNameFromType();
            TryCreatePropertyMap(t, classMap, mapTypePropertyName);

            return classMap;
        }

        private static void MapDefaultProperties(Type type, CsvClassMap classMap)
        {
            foreach (var property in DefaultPropertiesToMap)
            {
                var map = CreatePropertyMap(type, property);
                if (map != null)
                {
                    classMap.PropertyMaps.Add(map);
                }
            }
        }

        private static bool TryCreatePropertyMap(Type type, CsvClassMap classMap, string propertyName)
        {
            var map = CreatePropertyMap(type, propertyName);
            if (map != null)
            {
                classMap.PropertyMaps.Add(map);
                return true;
            }

            return false;
        }

        private static CsvPropertyMap CreatePropertyMap(Type type, string propertyName)
        {
            var propertyInfo = type.GetProperty(propertyName);
            if (propertyInfo == null) return null;

            if (propertyInfo.PropertyType.IsEnum)
                return CreateEnumerationPropertyMap(propertyInfo);

            var propertyMap = new CsvPropertyMap(propertyInfo);
            propertyMap.Name(propertyName);

            return propertyMap;
        }

        private static CsvPropertyMap CreateEnumerationPropertyMap(PropertyInfo propertyInfo)
        {
            if (propertyInfo != null)
            {
                if (!propertyInfo.PropertyType.IsEnum) return null;

                var propertyMap = new CsvPropertyMap(propertyInfo);
                propertyMap.Name(propertyInfo.Name);
                propertyMap.ConvertEnumerationType();

                return propertyMap;
            }

            return null;
        }
        
        public static IEnumerable<Type> GetAutoMappedDescriptorTypes()
        {
            return DescriptorHelpers.DescriptorTypes.Where(t => !ManualDescriptorTypeCsvClassMapMappings.Select(m => m.CsvClassMapMappedType()).Contains(t));
        }
    }
}
