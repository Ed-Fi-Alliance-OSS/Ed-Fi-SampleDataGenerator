using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using EdFi.SampleDataGenerator.Core.Helpers;

namespace EdFi.SampleDataGenerator.Core.Config
{
    public static class ValidationHelpers
    {
        public static bool IsValidRaceOption(this ISampleDataGeneratorConfig config, string raceValue)
        {
            return config.EthnicityMappings != null &&
                   config.EthnicityMappings.Any(x => x.Ethnicity.Equals(raceValue, StringComparison.OrdinalIgnoreCase));
        }

        public static bool IsValidGenderOption(this ISampleDataGeneratorConfig config, string genderValue)
        {
            return config.GenderMappings != null &&
                   config.GenderMappings.Any(x => x.Gender.Equals(genderValue, StringComparison.OrdinalIgnoreCase));
        }

        public static bool IsValidRaceConfiguration(this IAttributeConfiguration raceBasedAttributeConfiguration, ISampleDataGeneratorConfig globalConfig)
        {
            return raceBasedAttributeConfiguration == null
                   || raceBasedAttributeConfiguration.AttributeGeneratorConfigurationOptions
                       .All(x => globalConfig.IsValidRaceOption(x.Value));
        }
        public static bool ContainValidRacesOnly(IAttributeConfiguration configuration, ISampleDataGeneratorConfig globalConfig) => configuration.IsValidRaceConfiguration(globalConfig);

        public static bool IsValidBooleanConfiguration(this IAttributeConfiguration configuration)
        {
            return configuration == null
                   || configuration.AttributeGeneratorConfigurationOptions
                       .All(x => x.Value.IsValidBoolean());
        }
        public static bool ContainValidBooleansOnly(IAttributeConfiguration configuration) => configuration.IsValidBooleanConfiguration();

        public static char[] FileSystemUnsafeCharacters(this IEnumerable<char> candidates)
        {
            var validCharacter = new Regex(@"^[A-Za-z0-9_\-\. ]$");

            return candidates
                .Distinct()
                .Where(ch => !validCharacter.IsMatch(ch.ToString()))
                .ToArray();
        }
    }
}