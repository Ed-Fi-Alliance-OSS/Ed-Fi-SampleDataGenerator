using System;
using System.Collections.Generic;
using System.Linq;
using EdFi.SampleDataGenerator.Core.Config;
using EdFi.SampleDataGenerator.Core.DataGeneration.Generators;

namespace EdFi.SampleDataGenerator.Core.DataGeneration.Common
{
    public static class RandomHelpers
    {
        public static TItem GetRandomItem<TItem>(this TItem[] items, IRandomNumberGenerator randomNumberGenerator)
        {
            var index = randomNumberGenerator.Generate(0, items.Length);
            return items[index];
        }

        public static TItem GetRandomItem<TItem>(this List<TItem> items, IRandomNumberGenerator randomNumberGenerator)
        {
            var index = randomNumberGenerator.Generate(0, items.Count);
            return items[index];
        }

        public static bool GetRandomBool(this IRandomNumberGenerator randomNumberGenerator, double trueProbability = .5)
        {
            return randomNumberGenerator.GenerateDouble() < trueProbability;
        }

        public static DateTime GetRandomBirthday(this IRandomNumberGenerator randomNumberGenerator, int minAgeInYears, int maxAgeInYears, DateTime referenceDate)
        {
            var minDate = referenceDate.AddYears(maxAgeInYears * -1);
            var maxDate = referenceDate.AddYears(minAgeInYears * -1);
            return randomNumberGenerator.GetRandomDay(minDate, maxDate);
        }

        public static DateTime GetRandomDay(this IRandomNumberGenerator randomNumberGenerator, DateTime minDate, DateTime maxDate)
        {
            var dayDifference = (maxDate - minDate).Days;
            return minDate.AddDays(randomNumberGenerator.Generate(0, dayDifference));
        }

        public static double GenerateGaussian(this IRandomNumberGenerator randomNumberGenerator, double mu = 0, double sigma = 1)
        {
            //Box-Muller transformation - translates 2 uniformly distributed random variables to a bivariate normal distribution
            var u1 = 1.0 - randomNumberGenerator.GenerateDouble();
            var u2 = 1.0 - randomNumberGenerator.GenerateDouble();

            var randStdNormal = Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Sin(2.0 * Math.PI * u2);
            var randNormal = mu + sigma * randStdNormal;

            return randNormal;
        }

        public static double GeneratePoissonDelay(this IRandomNumberGenerator randomNumberGenerator, double lambda)
        {
            if (lambda < Double.Epsilon) return double.MaxValue;
            return Math.Log(1.0 - randomNumberGenerator.GenerateDouble()) / (-lambda);
        }

        public static int GeneratePoissonDelayInt(this IRandomNumberGenerator randomNumberGenerator, double lambda)
        {
            var eventOccurrence = Math.Round(randomNumberGenerator.GeneratePoissonDelay(lambda), MidpointRounding.AwayFromZero);

            //Ensure we never return 0, which doesn't make sense as a "delay" value.
            //We don't want multiple events for the same time period
            return eventOccurrence >= int.MaxValue
                ? int.MaxValue
                : Math.Max((int)eventOccurrence, 1);
        }

        public static TItem GetRandomItemWithDistribution<TItem>(this Dictionary<TItem, double> itemsByDistributionFrequency, IRandomNumberGenerator randomNumberGenerator)
        {
            return GetRandomItemWithDistribution(itemsByDistributionFrequency.Keys, x => itemsByDistributionFrequency[x], randomNumberGenerator);
        }

        public static List<TItem> GetNRandomItems<TItem>(this IEnumerable<TItem> items, IRandomNumberGenerator randomNumberGenerator, int numberOfItems)
        {
            if (numberOfItems < 0)
                throw new ArgumentOutOfRangeException(nameof(numberOfItems));

            if (numberOfItems == 0)
                return new List<TItem>();

            var itemList = items.ToList();

            if (itemList.Count < numberOfItems)
                throw new ArgumentOutOfRangeException(nameof(numberOfItems), "Source list contains fewer than the required number of items.");

            var selectedItems = new List<TItem>();

            for (var i = 0; i < itemList.Count; ++i)
            {
                var selectionProbability = (numberOfItems - selectedItems.Count) / (double) (itemList.Count - i);
                if (randomNumberGenerator.GetRandomBool(selectionProbability))
                {
                    selectedItems.Add(itemList[i]);
                }

                if (selectedItems.Count == numberOfItems)
                    break;
            }

            return selectedItems;
        }

        public static List<TValue> GetNRandomItems<TKey, TValue>(this IEnumerable<IGrouping<TKey, TValue>> groupings, IRandomNumberGenerator randomNumberGenerator, int numberOfItems)
        {
            var result = new List<TValue>();
            var groupList = groupings.ToList();
            var i = 0;

            foreach (var group in groupList)
            {
                var selectionProbability = (numberOfItems - result.Count) / (double)(groupList.Count - i);
                if (randomNumberGenerator.GetRandomBool(selectionProbability))
                {
                    var selectedItem = group.ToList().GetRandomItem(randomNumberGenerator);
                    result.Add(selectedItem);
                }

                ++i;
            }

            return result;
        }

        public static TItem GetRandomItemWithDistribution<TItem>(this IEnumerable<TItem> items, Func<TItem, double> distributionFrequency, IRandomNumberGenerator randomNumberGenerator)
        {
            var options = items.OrderByDescending(distributionFrequency).ToList();

            var randomSelection = randomNumberGenerator.GenerateDouble();
            var cumulativeChance = 0.0;

            for (var i = 0; i < options.Count; ++i)
            {
                cumulativeChance += distributionFrequency(options[i]);
                if (randomSelection < cumulativeChance)
                    return options[i];
            }

            //safety valve to ensure we don't return a null
            //because of rounding error. in practice, this introduces
            //a bias at the upper end of the distribution
            //but we're not trying to be exact here
            return options.Count > 0 && cumulativeChance + Constants.FloatingPointMath.Epsilon > randomSelection
                ? options[options.Count - 1]
                : default(TItem);
        }

        public static RandomOption<TItem> GetRandomItemWithDistribution<TItem>(this IEnumerable<RandomOption<TItem>> items, IRandomNumberGenerator randomNumberGenerator)
        {
            return GetRandomItemWithDistribution(items, i => i.Probability, randomNumberGenerator);
        }

        public static IAttributeGeneratorConfigurationOption GetRandomItem(this IAttributeConfiguration config, IRandomNumberGenerator randomNumberGenerator)
        {
            return GetRandomItemWithDistribution(config.AttributeGeneratorConfigurationOptions, o => o.Frequency, randomNumberGenerator);
        }

        public static IAttributeGeneratorConfigurationOption GetRandomItemWhere(this IAttributeConfiguration config, Func<IAttributeGeneratorConfigurationOption, bool> where, IRandomNumberGenerator randomNumberGenerator)
        {
            return GetRandomItemWithDistribution(config.AttributeGeneratorConfigurationOptions.Where(where), o => o.Frequency, randomNumberGenerator);
        }

        public static IAttributeGeneratorConfigurationOption GetRandomItem(this IAttributeConfiguration ethnicityBasedOptionConfig, StudentDataGeneratorContext context, IEthnicityMapping[] ethnicityMappings, IRandomNumberGenerator randomNumberGenerator)
        {
            return GetRandomItemWithDistribution(ethnicityBasedOptionConfig.AttributeGeneratorConfigurationOptions.Where(MatchesStudentEthnicity(context, ethnicityMappings)), o => o.Frequency, randomNumberGenerator);
        }

        private static Func<IAttributeGeneratorConfigurationOption, bool> MatchesStudentEthnicity(StudentDataGeneratorContext context, IEthnicityMapping[] ethnicityMappings)
        {
            return o =>
            {
                var mappingForThisStudent = ethnicityMappings.MappingFor(context);
                return ethnicityMappings.MappingFor(o.Value) == mappingForThisStudent;
            };
        }

        public static TValue GetValueWithProbability<TValue>(this IRandomNumberGenerator randomNumberGenerator, double percentProbability, TValue valueOnSuccess, TValue valueOnFailure)
        {
            var chance = randomNumberGenerator.GenerateDouble();
            return chance < percentProbability
                ? valueOnSuccess
                : valueOnFailure;
        }

        public static List<TItem> SelectItemsWithProbability<TItem>(this IEnumerable<TItem> items, IRandomNumberGenerator randomNumberGenerator, double probabilityOfSelection)
        {
            return items.Where(x => randomNumberGenerator.GetRandomBool(probabilityOfSelection)).ToList();
        }
    }
}