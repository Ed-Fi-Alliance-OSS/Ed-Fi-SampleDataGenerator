using System;
using System.Linq;
using EdFi.SampleDataGenerator.Core.Entities;

namespace EdFi.SampleDataGenerator.Core.Helpers
{
    public static class StudentEducationOrganizationHelper
    {
        public static readonly LanguageUseDescriptor PrimaryLanguageUseType;
        private static readonly string PrimaryLanguageUse;
        public static readonly LanguageDescriptor DefaultLanguage;

        static StudentEducationOrganizationHelper()
        {
            PrimaryLanguageUseType = LanguageUseDescriptor.HomeLanguage;
            PrimaryLanguageUse = PrimaryLanguageUseType.GetStructuredCodeValue();
            DefaultLanguage = LanguageDescriptor.English_eng;
        }

        public static LanguageDescriptor GetPrimaryLanguage(this StudentEducationOrganizationAssociation edOrgAssociation)
        {
            var primaryLanguage = edOrgAssociation.Language?.FirstOrDefault(l => l.LanguageUse != null && l.LanguageUse.Any(lu => lu == PrimaryLanguageUse));
            return primaryLanguage?.Language1.ParseFromStructuredCodeValue<LanguageDescriptor>() ?? DefaultLanguage;
        }

        public static bool SpeaksLanguage(this StudentEducationOrganizationAssociation edOrgAssociation, LanguageDescriptor language)
        {
            return edOrgAssociation.Language?.Any(l => l.Language1 == language.GetStructuredCodeValue()) ?? false;
        }

        public static bool SpeaksLanguageWithUseType(this StudentEducationOrganizationAssociation edOrgAssociation, LanguageDescriptor language, LanguageUseDescriptor languageUse)
        {
            return edOrgAssociation.Language?.Any(l =>
                (l.Language1 == language.GetStructuredCodeValue()) &&
                (l.LanguageUse?.Any(lu => lu == languageUse.GetStructuredCodeValue()) ?? false)) ?? false;
        }

        public static void SetPrimaryLanguage(this StudentEducationOrganizationAssociation edOrgAssociation, LanguageDescriptor language)
        {
            edOrgAssociation.AddLanguage(language, PrimaryLanguageUseType);
        }

        public static void AddLanguage(this StudentEducationOrganizationAssociation edOrgAssociation, LanguageDescriptor language, LanguageUseDescriptor languageUse)
        {
            if (languageUse.CodeValue == PrimaryLanguageUseType.CodeValue &&
                edOrgAssociation.Language != null &&
                edOrgAssociation.Language.Any(studentLanguage => studentLanguage.LanguageUse != null && studentLanguage.LanguageUse.Any(lu => lu == PrimaryLanguageUse)))
            {
                throw new InvalidOperationException("Primary language may only be set one time");
            }

            if (edOrgAssociation.SpeaksLanguageWithUseType(language, languageUse)) return;

            if (edOrgAssociation.SpeaksLanguage(language))
            {
                var languageToUpdate = edOrgAssociation.Language.Single(l => l.Language1 == language.GetStructuredCodeValue());
                languageToUpdate.LanguageUse = languageToUpdate.LanguageUse.CreateCopyAndAppend(languageUse.GetStructuredCodeValue());
            }

            else
            {
                var newLanguage = new Language
                {
                    Language1 = language.GetStructuredCodeValue(),
                    LanguageUse = new[]
                    {
                        languageUse.GetStructuredCodeValue()
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
