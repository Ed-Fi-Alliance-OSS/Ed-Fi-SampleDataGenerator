using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml.Serialization;

namespace EdFi.SampleDataGenerator.Core.Helpers
{
    public static class EnumHelpers
    {
        private static readonly Dictionary<Type, Dictionary<string, object>> EnumParseCache = new Dictionary<Type, Dictionary<string, object>>(); 

        public static TEnum Parse<TEnum>(string value) where TEnum: struct
        {
            if (string.IsNullOrEmpty(value))
                throw new ArgumentException("Cannot parse empty string to enumeration value", nameof(value));

            TEnum result;
            if (TryParse(value, out result)) return result;

            throw new ArgumentException($"'{value}' cannot be parsed to an enumeration of type {typeof(TEnum).Name}", nameof(value));
        }

        public static TEnum Parse<TEnum, TInput>(TInput input, Func<TInput, string> getParseValue, Func<TInput, string> getExceptionMessage) where TEnum: struct
        {
            try
            {
                var enumValue = Parse<TEnum>(getParseValue(input));
                return enumValue;
            }
            catch (Exception)
            {
                throw new ArgumentException(getExceptionMessage(input));
            }
        }

        public static bool TryParse<TEnum>(string value, out TEnum result) where TEnum: struct
        {
            var enumType = typeof(TEnum);
            
            if (!EnumParseCache.ContainsKey(enumType))
            {
                EnumParseCache[enumType] = GetEnumXmlValueMap<TEnum>().ToDictionary(x => x.Key, x => (object)x.Value);
            }

            if (EnumParseCache[enumType].ContainsKey(value))
            {
                result = (TEnum)EnumParseCache[enumType][value];
                return true;
            }

            if (Enum.TryParse(value, true, out result))
            {
                return true;
            }

            result = default(TEnum);
            return false;
        }

        public static bool IsParseableToEnum<TEnum>(string value) where TEnum: struct
        {
            TEnum result;
            return TryParse(value, out result);
        }

        public static string ToCodeValue<T>(this T enumerationValue) where T : struct
        {
            var enumType = typeof (T);
            if (!enumType.IsEnum)
                throw new ArgumentException("Not an enumerable type");

            var memberInfo = enumType.GetMember(enumerationValue.ToString());
            var xmlAttribute = memberInfo[0].GetCustomAttribute(typeof(XmlEnumAttribute)) as XmlEnumAttribute;
            if (xmlAttribute != null)
            {
                return xmlAttribute.Name;
            }

            return enumerationValue.ToString();
        }

        public static TEnum[] GetAll<TEnum>() where TEnum : struct
        {
            var enumType = typeof(TEnum);
            if (!enumType.IsEnum)
                throw new ArgumentException("Not an enumerable type");

            return (TEnum[]) Enum.GetValues(typeof (TEnum));
        }

        public static Dictionary<string, T> GetEnumValueMap<T>() where T: struct
        {
            var enumValues = GetAll<T>();
            return enumValues.ToDictionary(enumValue => enumValue.ToString());
        }

        public static Dictionary<string, T> GetEnumXmlValueMap<T>() where T: struct
        {
            var enumValues = GetAll<T>();
            return enumValues.ToDictionary(enumValue => enumValue.ToCodeValue());
        }
    }
}
