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
    public static class CsvClassMapHelpers
    {
        public static Dictionary<Type, CsvClassMap> GetTypeToCsvClassMapInstanceDictionary(IEnumerable<Type> classMapTypes)
        {
            var result = new Dictionary<Type, CsvClassMap>();
            
            foreach (var classMapType in classMapTypes)
            {
                var mapForType = classMapType.CsvClassMapMappedType();
                if (mapForType == null) continue;

                var instance = CsvClassMapFactory.GetCsvClassMapFor(classMapType);
                result.Add(mapForType, instance);
            }

            return result;
        }

        public static Dictionary<Type, CsvClassMap> GetTypeToCsvClassMapInstanceDictionary()
        {
            return GetTypeToCsvClassMapInstanceDictionary(ScanForCsvClassMapTypes());
        }

        public static Type CsvClassMapMappedType(this Type csvClassMapType)
        {
            var baseType = csvClassMapType.BaseType;

            while (baseType != null)
            {
                if (baseType.IsGenericType &&
                    !baseType.ContainsGenericParameters &&
                    baseType.GetGenericTypeDefinition() == typeof(CsvClassMap<>))
                    return baseType.GenericTypeArguments.Single();

                baseType = baseType.BaseType;
            }

            throw new ArgumentException($"Expected a type implementing {typeof(CsvClassMap<>)}, but received {csvClassMapType.FullName}.");
        }

        public static IEnumerable<Type> ScanForCsvClassMapTypes()
        {
            var standardCsvClassMapsMarkerType = typeof(IMarkerForCsvClassMaps);
            var csvClassMapType = typeof(CsvClassMap);

            var classMapTypes = Assembly.GetAssembly(standardCsvClassMapsMarkerType)
                .GetTypes()
                .Where(t => csvClassMapType.IsAssignableFrom(t)
                        && !t.IsInterface
                        && !t.IsAbstract);

            return classMapTypes;
        }

        private static void RecursivePrefixHelper(CsvPropertyReferenceMap referenceMap, string prefix)
        {
            var newPrefix = $"{prefix}{referenceMap.Data.Property.Name}.";

            referenceMap.Prefix(newPrefix);

            foreach (var childReferenceMap in referenceMap.Data.Mapping.ReferenceMaps)
            {
                RecursivePrefixHelper(childReferenceMap, newPrefix);
            }
        }
        
        public static CsvPropertyReferenceMap RecursivePrefix(this CsvPropertyReferenceMap referenceMap)
        {
            RecursivePrefixHelper(referenceMap, "");
            return referenceMap;
        }

        public static CsvClassMap RecursivePrefixReferencesMaps(this CsvClassMap classMap)
        {
            foreach (var referenceMap in classMap.ReferenceMaps)
            {
                referenceMap.RecursivePrefix();
            }

            return classMap;
        }

        public static CsvPropertyMap ConvertEnumerationType(this CsvPropertyMap propertyMap)
        {
            //in case this is an array type, we need to get the underlying element type
            //in order to create the converter below
            var underlyingType = propertyMap.Data.Property.PropertyType.GetUnderlyingType();
            if (!underlyingType.IsEnum) throw new InvalidOperationException($"'{underlyingType.Name}' is not an enumeration type");

            var enumerationConverterType = typeof(EnumerationTypeConverter<>).MakeGenericType(underlyingType);
            var enumerationConverter = (ITypeConverter)Activator.CreateInstance(enumerationConverterType);

            propertyMap.TypeConverter(enumerationConverter);
            return propertyMap;
        }

        public static CsvPropertyMap ConvertDescriptorType(this CsvPropertyMap propertyMap)
        {
            //in case this is an array type, we need to get the underlying element type
            //in order to create the converter below
            var underlyingType = propertyMap.Data.Property.PropertyType.GetUnderlyingType();
            if (!typeof(DescriptorType).IsAssignableFrom(underlyingType)) throw new InvalidOperationException($"'{underlyingType.Name}' is not a DescriptorType");

            var descriptorConverterType = typeof(DescriptorTypeConverter<>).MakeGenericType(underlyingType);
            var descriptorConverter = (ITypeConverter)Activator.CreateInstance(descriptorConverterType);

            propertyMap.TypeConverter(descriptorConverter);
            return propertyMap;
        }

        public static void SetProperty(this CsvPropertyReferenceMap map, object entityToPopulate, object value)
        {
            map.Data.Property.SetValue(entityToPopulate, value);
            SetIsSpecifiedMember(map.Data.Property, entityToPopulate, value);
        }

        public static void SetProperty(this CsvPropertyMap map, ref object entityToPopulate, object value)
        {
            var propertyType = map.Data.Property.PropertyType;
            
            if (propertyType.IsArray && propertyType.GetUnderlyingType() == value.GetType() && entityToPopulate.GetType() == value.GetType())
            {
                entityToPopulate = value;
            }

            else
            {
                map.Data.Property.SetValue(entityToPopulate, value);
            }
            
            SetIsSpecifiedMember(map.Data.Property, entityToPopulate, value);
        }

        public static void SetIsSpecifiedMember(PropertyInfo property, object entityToPopulate, object value)
        {
            var isSpecifiedMembername = $"{property.Name}Specified";
            var entityType = entityToPopulate.GetType();
            var isSpecifiedProperty = entityType.GetProperty(isSpecifiedMembername);
            if (isSpecifiedProperty != null && isSpecifiedProperty.PropertyType == typeof(bool) && value != null)
            {
                isSpecifiedProperty.SetValue(entityToPopulate, true);
            }
        }

        public static bool? GetIsSpecifiedMemberValue(PropertyInfo property, object entity)
        {
            var isSpecifiedMembername = $"{property.Name}Specified";
            var entityType = entity.GetType();
            var isSpecifiedProperty = entityType.GetProperty(isSpecifiedMembername);

            return isSpecifiedProperty == null || isSpecifiedProperty.PropertyType != typeof (bool)
                ? null
                : (bool?)isSpecifiedProperty.GetValue(entity);
        }
    }
}
