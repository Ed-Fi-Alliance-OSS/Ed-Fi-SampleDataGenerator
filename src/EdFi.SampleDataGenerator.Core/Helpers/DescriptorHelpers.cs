using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using EdFi.SampleDataGenerator.Core.Entities;

namespace EdFi.SampleDataGenerator.Core.Helpers
{
    public static class DescriptorHelpers
    {
        public const string DescriptorNamespacePrefix = "http://ed-fi.org/Descriptor";

        private const string DescriptorText = "Descriptor";
        private const string MapText = "Map";
        public const string DescriptorFormatUri = @"uri://ed-fi.org/";

        private static readonly Lazy<IEnumerable<Type>> DescriptorTypeCache = new Lazy<IEnumerable<Type>>(ScanDescriptorTypes);
        public static IEnumerable<Type> DescriptorTypes => DescriptorTypeCache.Value;

        private static readonly Dictionary<string, Dictionary<string, DescriptorType>> DescriptorsDictionary = GetDescriptorNames();

        public static string DescriptorNameFromType(this Type t)
        {
            if (!typeof(DescriptorType).IsAssignableFrom(t)) throw new InvalidOperationException($"{t.Name} is not a DescriptorType");

            return t.Name.Replace(DescriptorText, "");
        }

        public static string DescriptorNameFromFilePath(string filePath)
        {
            return Path.GetFileNameWithoutExtension(filePath)?.Replace(DescriptorText, "");
        }

        public static Type DescriptorTypeFromName(string name)
        {
            var descriptorTypeName = $"{name}{DescriptorText}";
            return DescriptorTypes.FirstOrDefault(t => t.Name.Equals(descriptorTypeName, StringComparison.OrdinalIgnoreCase));
        }

        public static string DescriptorMapTypePropertyNameFromType(this Type t)
        {
            if (!typeof(DescriptorType).IsAssignableFrom(t)) throw new InvalidOperationException($"{t.Name} is not a DescriptorType");

            return t.Name.Replace(DescriptorText, MapText);
        }

        public static IEnumerable<Type> ScanDescriptorTypes()
        {
            var descriptorType = typeof(DescriptorType);
            var descriptorTypes = Assembly.GetAssembly(descriptorType)
                .GetTypes()
                .Where(t => descriptorType.IsAssignableFrom(t)
                        && !t.IsInterface
                        && !t.IsAbstract);

            return descriptorTypes;
        }

        private static Dictionary<string, Dictionary<string, DescriptorType>> GetDescriptorNames()
        {
            var types = DescriptorTypeCache.Value;
            return types.ToDictionary(type => type.Name, GetDescriptorFields);
        }

        private static Dictionary<string, DescriptorType> GetDescriptorFields(Type descriptorType)
        {
            var list = descriptorType.GetFields().Where(a => a.FieldType == descriptorType).ToList();
            return list.ToDictionary(b => b.Name, c => (DescriptorType)c.GetValue(null));
        }

        public static string GetDescriptorNamespace<TDescriptor>()
            where TDescriptor : DescriptorType
        {
            return GetDescriptorNamespace(typeof(TDescriptor).Name);
        }

        public static string GetDescriptorNamespace(string descriptorName)
        {
            return $"{DescriptorNamespacePrefix}/{descriptorName}.xml";
        }

        public static bool IsParseableToDescriptorFromName<TDescriptor>(string name) where TDescriptor : DescriptorType
        {
            var descriptorType = typeof(TDescriptor);

            return DescriptorsDictionary.ContainsKey(descriptorType.Name) &&
                   DescriptorsDictionary[descriptorType.Name].ContainsKey(name);
        }

        public static bool IsParseableToDescriptorFromCodeValue<TDescriptor>(string codeValue) where TDescriptor : DescriptorType
        {
            codeValue = ParseCodeValue(codeValue);

            var descriptorType = typeof(TDescriptor);

            return DescriptorsDictionary.ContainsKey(descriptorType.Name) &&
                   DescriptorsDictionary[descriptorType.Name].Values.Any(x => x.CodeValue == codeValue);
        }

        public static bool TryParseFromCodeValue<TDescriptor>(string value, out TDescriptor descriptor) where TDescriptor : DescriptorType
        {
            value = ParseCodeValue(value);

            var descriptorType = typeof(TDescriptor);
            descriptor = null;

            if (DescriptorsDictionary.ContainsKey(descriptorType.Name))
            {
                descriptor = (TDescriptor)DescriptorsDictionary[descriptorType.Name].Values.FirstOrDefault(x => x.CodeValue == value);
            }
            return descriptor != null;
        }

        public static bool TryParseFromCodeValue<TDescriptor>(string value, bool ignoreCase, out TDescriptor descriptor) where TDescriptor : DescriptorType
        {
            value = ParseCodeValue(value);

            var descriptorType = typeof(TDescriptor);
            descriptor = null;

            if (DescriptorsDictionary.ContainsKey(descriptorType.Name))
            {
                descriptor = (TDescriptor)DescriptorsDictionary[descriptorType.Name].Values.FirstOrDefault(x => x.CodeValue.Equals(value, ignoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal));
            }
            return descriptor != null;
        }

        public static TDescriptor ParseFromCodeValue<TDescriptor, TInput>(TInput input, Func<TInput, string> getParseValue, Func<TInput, string> getExceptionMessage) where TDescriptor : DescriptorType
        {
            try
            {
                var value = ParseCodeValue(getParseValue(input));
                var descriptorType = typeof(TDescriptor);
                var field = DescriptorsDictionary[descriptorType.Name].Values.First(x => x.CodeValue.Equals(value, StringComparison.OrdinalIgnoreCase));
                return (TDescriptor)field;
            }
            catch (Exception)
            {
                throw new ArgumentException(getExceptionMessage(input));
            }
        }

        public static TDescriptorReferenceType GetDescriptorReference<TDescriptorReferenceType, TDescriptor>(this TDescriptor descriptor)
            where TDescriptorReferenceType : DescriptorReferenceType, new()
            where TDescriptor : DescriptorType
        {

            return new TDescriptorReferenceType
            {
                CodeValue = descriptor.CodeValue,
                Namespace = DescriptorReferenceTypeHelpers.GetDescriptorNamespace<TDescriptorReferenceType>()
            };
        }

        public static string[] ToCodeValueArray<TDescriptor>()
            where TDescriptor : DescriptorType
        {
            var descriptorType = typeof(TDescriptor);
            return DescriptorsDictionary[descriptorType.Name].Select(x => x.Value.CodeValue).ToArray();
        }

        public static string[] ToCodeValueArray<TDescriptor>(this TDescriptor[] descriptors)
            where TDescriptor : DescriptorType
        {
            return descriptors.Select(x => x.CodeValue).ToArray();
        }
        public static string[] ToStructuredCodeValueArray<TDescriptor>(this TDescriptor[] descriptors)
            where TDescriptor : DescriptorType
        {
            return descriptors.Select(x => x.GetStructuredCodeValue()).ToArray();
        }

        public static string[] ToStructuredCodeValueFormatArray<TDescriptor>(this TDescriptor[] descriptors)
            where TDescriptor : DescriptorType
        {
            return descriptors.Select(x => x.GetStructuredCodeValue()).ToArray();
        }

        public static TDescriptor ToDescriptorFromName<TDescriptor>(this string name)
            where TDescriptor : DescriptorType
        {
            var descriptorType = typeof(TDescriptor);
            var field = DescriptorsDictionary[descriptorType.Name][name];
            return (TDescriptor)field;
        }

        public static TDescriptor ToDescriptorFromCodeValue<TDescriptor>(this string codeValue)
            where TDescriptor : DescriptorType
        {
            codeValue = ParseCodeValue(codeValue);
            var descriptorType = typeof(TDescriptor);
            var field = DescriptorsDictionary[descriptorType.Name].Values.First(x => x.CodeValue == codeValue);
            return (TDescriptor)field;
        }

        public static TDescriptor[] GetAll<TDescriptor>() where TDescriptor : DescriptorType
        {
            var descriptorType = typeof(TDescriptor);
            if (DescriptorsDictionary.ContainsKey(descriptorType.Name))
                return DescriptorsDictionary[descriptorType.Name].Values.Cast<TDescriptor>().ToArray();

            throw new Exception("Descriptor Type not found");
        }

        public static bool Is<TDescriptor>(this string value, TDescriptor descriptor)
            where TDescriptor : DescriptorType
        {
            value = ParseCodeValue(value);
            return value.Equals(descriptor.CodeValue, StringComparison.InvariantCultureIgnoreCase);
        }

        public static string GetStructuredCodeValue<TDescriptor>(this TDescriptor descriptor)
            where TDescriptor : DescriptorType
        {
            return $"{descriptor.Namespace}#{descriptor.CodeValue}";
        }

        private static string ParseCodeValue(string text)
        {
            if (text.StartsWith(DescriptorFormatUri))
            {
                var fields = text.Split('#');
                if (fields.Length == 2)
                {
                    return fields[1];
                }
            }
            return text;
        }

        public static string ParseToCodeValue(this string value)
        {
            return ParseCodeValue(value);
        }
    }
}
