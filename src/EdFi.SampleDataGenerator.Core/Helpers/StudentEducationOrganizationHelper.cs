using System;
using System.Linq;
using EdFi.SampleDataGenerator.Core.DataGeneration.Generators;
using EdFi.SampleDataGenerator.Core.Entities;

namespace EdFi.SampleDataGenerator.Core.Helpers
{
    public static class StudentEducationOrganizationHelper
    {
        public static LanguageUseType PrimaryLanguageUseType = LanguageUseType.Homelanguage;
        public static LanguageMapType DefaultLanguage = LanguageMapType.English;

        public static LanguageMapType GetPrimaryLanguage(this StudentEducationOrganizationAssociation edOrgAssociation)
        {
            var primaryLanguage = edOrgAssociation.Language?.FirstOrDefault(l => l.LanguageUse != null && l.LanguageUse.Any(lu => lu == PrimaryLanguageUseType));
            return primaryLanguage?.Language1.ToLanguageMapType() ?? DefaultLanguage;
        }

        public static bool SpeaksLanguage(this StudentEducationOrganizationAssociation edOrgAssociation, LanguageMapType language)
        {
            return edOrgAssociation.Language?.Any(l => l.Language1 == language.Value) ?? false;
        }

        public static bool SpeaksLanguageWithUseType(this StudentEducationOrganizationAssociation edOrgAssociation, LanguageMapType language, LanguageUseType languageUse)
        {
            return edOrgAssociation.Language?.Any(l =>
                (l.Language1 == language) &&
                (l.LanguageUse?.Any(lu => lu == languageUse) ?? false)) ?? false;
        }

        public static void SetPrimaryLanguage(this StudentEducationOrganizationAssociation edOrgAssociation, LanguageMapType language)
        {
            edOrgAssociation.AddLanguage(language, PrimaryLanguageUseType);
        }

        public static void AddLanguage(this StudentEducationOrganizationAssociation edOrgAssociation, LanguageMapType language, LanguageUseType languageUse)
        {
            if (languageUse == PrimaryLanguageUseType &&
                edOrgAssociation.Language != null &&
                edOrgAssociation.Language.Any(studentLanguage => studentLanguage.LanguageUse != null && studentLanguage.LanguageUse.Any(lu => lu == PrimaryLanguageUseType)))
            {
                throw new InvalidOperationException("Primary language may only be set one time");
            }

            if (edOrgAssociation.SpeaksLanguageWithUseType(language, languageUse)) return;

            if (edOrgAssociation.SpeaksLanguage(language))
            {
                var languageToUpdate = edOrgAssociation.Language.Single(l => l.Language1 == language.Value);
                languageToUpdate.LanguageUse = languageToUpdate.LanguageUse.CreateCopyAndAppend(languageUse);
            }

            else
            {
                var newLanguage = new Language
                {
                    Language1 = language.Value,
                    LanguageUse = new[]
                    {
                        (string) languageUse
                    }
                };

                edOrgAssociation.Language = edOrgAssociation.Language.CreateCopyAndAppend(newLanguage);
            }
        }

        public static bool HasCharacteristic(this StudentEducationOrganizationAssociation edOrgAssociation, StudentCharacteristicDescriptor descriptor)
        {
            if (edOrgAssociation.StudentCharacteristic == null ||
                edOrgAssociation.StudentCharacteristic.Length == 0)
                return false;

            return edOrgAssociation.StudentCharacteristic.Any(x => x.StudentCharacteristic1 == descriptor.GetStructuredCodeValue());
        }

        public static bool HasIndicator(this StudentEducationOrganizationAssociation edOrgAssociation, StudentIndicator indicator)
        {
            if (edOrgAssociation.StudentIndicator == null ||
                edOrgAssociation.StudentIndicator.Length == 0)
                return false;

            return edOrgAssociation.StudentIndicator.Any(x => x.IndicatorName == indicator.IndicatorName);
        }

        public static void RemoveCharacteristic(this StudentEducationOrganizationAssociation edOrgAssociation, StudentCharacteristicDescriptor descriptor)
        {
            edOrgAssociation.StudentCharacteristic = edOrgAssociation.StudentCharacteristic.Where(x => x.StudentCharacteristic1 != descriptor.GetStructuredCodeValue()).ToArray();
        }
    }
}
