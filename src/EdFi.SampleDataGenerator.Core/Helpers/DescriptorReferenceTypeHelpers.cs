using System;
using System.Linq;
using EdFi.SampleDataGenerator.Core.Entities;

namespace EdFi.SampleDataGenerator.Core.Helpers
{
    public static class DescriptorReferenceTypeHelpers
    {
        private const string ReferenceTypeText = "ReferenceType";

        public static bool Is<TDescriptorReferenceType, TDescriptor>(this TDescriptorReferenceType descriptorReferenceType, TDescriptor descriptorValue)
            where TDescriptorReferenceType : DescriptorReferenceType
            where TDescriptor : DescriptorType
        {
            return descriptorReferenceType.CodeValue.Equals(descriptorValue.CodeValue,
                StringComparison.OrdinalIgnoreCase);
        }

        public static TEnum ToType<TEnum>(this DescriptorReferenceType descriptor)
            where TEnum : struct
        {
            return EnumHelpers.Parse<TEnum>(descriptor.CodeValue);
        }

        public static TEnum ToType<TEnum>(this string descriptor)
            where TEnum : struct
        {
            return EnumHelpers.Parse<TEnum>(descriptor);
        }

        public static string GetDescriptorName<TDescriptorReferenceType>()
            where TDescriptorReferenceType : DescriptorReferenceType
        {
            return typeof(TDescriptorReferenceType).Name.Replace(ReferenceTypeText, "");
        }

        public static string GetDescriptorName<TDescriptorReferenceType>(this TDescriptorReferenceType descriptorReference)
            where TDescriptorReferenceType : DescriptorReferenceType
        {
            return GetDescriptorName<TDescriptorReferenceType>();
        }

        public static string GetDescriptorNamespace<TDescriptorReferenceType>()
            where TDescriptorReferenceType : DescriptorReferenceType
        {
            var descriptorName = GetDescriptorName<TDescriptorReferenceType>();
            return DescriptorHelpers.GetDescriptorNamespace(descriptorName);
        }

        public static TDescriptorReferenceType GetDescriptorReferenceType<TDescriptorReferenceType, TDescriptor>(this TDescriptor descriptor)
            where TDescriptorReferenceType : DescriptorReferenceType, new()
            where TDescriptor : DescriptorType
        {
            return new TDescriptorReferenceType
            {
                CodeValue = descriptor.CodeValue,
                Namespace = GetDescriptorNamespace<TDescriptorReferenceType>()
            };
        }

        public static string[] GetCodeValueArray<TDescriptorReferenceType>(this TDescriptorReferenceType[] descriptorReferenceTypes)
            where TDescriptorReferenceType : DescriptorReferenceType
        {
            return descriptorReferenceTypes.Select(x => x.CodeValue).ToArray();
        }

        public static LanguageMapType ToLanguageMapType(this string value)
        {
            switch (value)
            {
                case "English":
                    return LanguageMapType.English;
                case "Spanish":
                    return LanguageMapType.Spanish;
                case "Other":
                    return LanguageMapType.Other;
            }

            throw new Exception("LanguageMapType not found");
        }
    }
}
