using System;
using System.Collections.Generic;
using System.Linq;
using EdFi.SampleDataGenerator.Core.Config;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common;
using NUnit.Framework;
using Shouldly;

namespace EdFi.SampleDataGenerator.Core.UnitTests.DataGeneration.Common
{
    [TestFixture]
    public class RandomHelpersTester
    {
        private class RandomAttributeConfiguration : IAttributeConfiguration
        {
            public string Name { get; set; }
            public IAttributeGeneratorConfigurationOption[] AttributeGeneratorConfigurationOptions { get; set; }
        }

        private class RandomOption : IAttributeGeneratorConfigurationOption
        {
            public string Value { get; set; }
            public double Frequency { get; set; }
        }

        [Test]
        public void ShouldChooseAllOptionsUsingDescendingOrderedFrequency()
        {
            var randomNumberGenerator = new TestRandomNumberGenerator { RandomDoubleSequence = new[] { 0.39, 0.69, 0.89, 0.99 } };
            var options = new[]
            {
                new RandomOption {Frequency = 0.1, Value = "10 percent option"},
                new RandomOption {Frequency = 0.2, Value = "20 percent option"},
                new RandomOption {Frequency = 0.3, Value = "30 percent option"},
                new RandomOption {Frequency = 0.4, Value = "40 percent option"},
            };
            
            options.GetRandomItemWithDistribution(o => o.Frequency, randomNumberGenerator).Value.ShouldBe("40 percent option");
            options.GetRandomItemWithDistribution(o => o.Frequency, randomNumberGenerator).Value.ShouldBe("30 percent option");
            options.GetRandomItemWithDistribution(o => o.Frequency, randomNumberGenerator).Value.ShouldBe("20 percent option");
            options.GetRandomItemWithDistribution(o => o.Frequency, randomNumberGenerator).Value.ShouldBe("10 percent option");
            options.GetRandomItemWithDistribution(o => o.Frequency, randomNumberGenerator).Value.ShouldBe("40 percent option");
        }

        [Test]
        public void ShouldReturnNullWhenNoOptionsAvailable()
        {
            var randomNumberGenerator = new TestRandomNumberGenerator { RandomDoubleSequence = new[] { 0.11 } };
            var options = new RandomOption[] { };

            options.GetRandomItemWithDistribution(o => o.Frequency, randomNumberGenerator).ShouldBeNull();
        }

        [Test]
        public void ShouldReturnNullWhenRandomNumberGreaterThanSumOfOptions()
        {
            var randomNumberGenerator = new TestRandomNumberGenerator { RandomDoubleSequence = new[] { 0.31 } };
            var options = new[]
            {
                new RandomOption {Frequency = 0.1, Value = "10 percent option"},
                new RandomOption {Frequency = 0.2, Value = "20 percent option"},
            };
            
            options.GetRandomItemWithDistribution(o => o.Frequency, randomNumberGenerator).ShouldBeNull();
        }

        [Test]
        public void ShouldReturnValueWhenRandomNumberLessThanSumOfOptions()
        {
            var randomNumberGenerator = new TestRandomNumberGenerator { RandomDoubleSequence = new[] { 0.01 } };
            var options = new[]
            {
                new RandomOption {Frequency = 0.1, Value = "10 percent option"},
            };

            options.GetRandomItemWithDistribution(o => o.Frequency, randomNumberGenerator).ShouldNotBeNull();
        }

        [Test]
        public void ShouldReturnValueIfSumOfFrequenciesWithinTolerance()
        {
            var randomNumberGenerator = new TestRandomNumberGenerator { RandomDoubleSequence = new[] { 1.0 } };
            var options = new[]
            {
                new RandomOption {Frequency = 0.5, Value = "50 percent option"},
                new RandomOption {Frequency = 0.5 - (Constants.FloatingPointMath.Epsilon/10), Value = "Other 50 percent option"},
            };

            options.GetRandomItemWithDistribution(o => o.Frequency, randomNumberGenerator).ShouldNotBeNull();
        }

        [Test]
        public void ShouldNotChooseFilteredItems()
        {
            var randomNumberGenerator = new TestRandomNumberGenerator { RandomDoubleSequence = new[] { 0.39, 0.69, 0.89, 0.99 } };
            var options = new[]
            {
                new RandomOption {Frequency = 0.1, Value = "10 percent option"},
                new RandomOption {Frequency = 0.2, Value = "20 percent option"},
                new RandomOption {Frequency = 0.3, Value = "30 percent option"},
                new RandomOption {Frequency = 0.4, Value = "40 percent option"},
            };

            var config = new RandomAttributeConfiguration
            {
                AttributeGeneratorConfigurationOptions = options,
                Name = "Test"
            };

            config.GetRandomItemWhere(o => o.Frequency > 0.1, randomNumberGenerator).Value.ShouldBe("40 percent option");
            config.GetRandomItemWhere(o => o.Frequency > 0.1, randomNumberGenerator).Value.ShouldBe("30 percent option");
            config.GetRandomItemWhere(o => o.Frequency > 0.1, randomNumberGenerator).Value.ShouldBe("20 percent option");
            config.GetRandomItemWhere(o => o.Frequency > 0.1, randomNumberGenerator).ShouldBeNull();
        }

        [Test]
        public void GetNRandomItemsShouldThrowIfNotEnoughItemsAvailable()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new List<int>().GetNRandomItems(new RandomNumberGenerator(), 1));
        }

        [Test]
        public void GetNRandomItemsShouldReturnEmptyListIf0ItemsRequested()
        {
            var sourceList = new[] {1, 2, 3};
            var randomItems = sourceList.GetNRandomItems(new RandomNumberGenerator(), 0);

            randomItems.Count.ShouldBe(0);
        }

        [Test]
        public void GetNRandomItemsShouldReturnExactlyRequestedNumberOfItems()
        {
            const int sourceItemStart = 0;
            const int numberOfItemsInSourceList = 10;
            const int numberOfItemsToChoose = 5;

            var sourceList = Enumerable.Range(sourceItemStart, numberOfItemsInSourceList).ToList();

            //generating all 0's ensures IRandomNumberGenerator.GetRandomBool() method always returns true
            //and therefore return the first N items
            var randomNumberGenerator = new TestRandomNumberGenerator { RandomDoubleSequence = new[] { 0.0 } };

            var randomItems = sourceList.GetNRandomItems(randomNumberGenerator, numberOfItemsToChoose);

            randomItems.Count.ShouldBe(numberOfItemsToChoose);
            for (var i = 0; i < randomItems.Count; ++i)
            {
                randomItems[i].ShouldBe(i);
            }

            //generating the largest possible random double < 1 ensures item selection probability 
            //will always be 0 (because IRandomNumberGenerator.GetRandomBool() will return false)
            //until we are at the last N items, and should therefore select all remaining items
            randomNumberGenerator = new TestRandomNumberGenerator { RandomDoubleSequence = new[] { 0.999999999999  } };

            randomItems = sourceList.GetNRandomItems(randomNumberGenerator, numberOfItemsToChoose);

            randomItems.Count.ShouldBe(numberOfItemsToChoose);
            for (var i = 0; i < randomItems.Count; ++i)
            {
                randomItems[i].ShouldBe(sourceItemStart + numberOfItemsToChoose + i);
            }
        }

        [Test]
        public void SelectItemsWithProbabilityShouldReturnAFractionOfTheOrigialItems()
        {
            const double probabilityOfSelection = 0.1;

            var sourceList = new[] {"A", "B", "C", "D", "E"}.ToList();

            var randomNumberGenerator = new TestRandomNumberGenerator
            {
                //Given our probabilityOfSelection of 0.1:
                //  - Generating 0.0 ensures GetRandomBool will be true, causing the item to be selected.
                //  - Generating 0.2 ensures GetRandomBool will be false, causing the item to be skipped.

                RandomDoubleSequence = new[]
                {
                    0.0, // Select A
                    0.2, // Skip B
                    0.0, // Select C
                    0.2, // Skip D
                    0.2  // Skip E
                }
            };

            sourceList
                .SelectItemsWithProbability(randomNumberGenerator, probabilityOfSelection)
                .ShouldBe(new[] {"A", "C"});
        }
    }
}
