using System;
using System.Collections.Generic;
using System.Linq;
using EdFi.SampleDataGenerator.Core.Sorting;
using NUnit.Framework;
using Shouldly;

namespace EdFi.SampleDataGenerator.Core.UnitTests.Sorting
{
    public class SortTestItem
    {
        public SortTestItem(string value, params string[] dependencies)
        {
            Value = value;
            DependsOnValues = dependencies;
        }

        public string Value { get; set; }
        public string[] DependsOnValues { get; set; }
    }

    [TestFixture]
    public class TopologicalSortTester
    {
        [Test]
        public void ShouldDetectCycle()
        {
            var unorderedList = new List<SortTestItem>()
            {
                new SortTestItem("A", "B"),
                new SortTestItem("B", "C"),
                new SortTestItem("C", "A")
            };

            Assert.Throws<TopologicalSortException>(() =>
            {
                var result = TopologicalSort.Sort(unorderedList, i => i.DependsOnValues, i => i.Value);
            });
        }

        [Test]
        public void ResultShouldContainAllSourceItems()
        {
            var unorderedList = new List<SortTestItem>()
            {
                new SortTestItem("A", "B"),
                new SortTestItem("B"),
                new SortTestItem("C", "A"),
                new SortTestItem("D"),
                new SortTestItem("E", "D"),
            };

            var result = TopologicalSort.Sort(unorderedList, i => i.DependsOnValues, i => i.Value).ToList();
            result.Count.ShouldBe(unorderedList.Count);
            unorderedList.All(i => result.Contains(i)).ShouldBe(true);
        }

        [Test]
        public void ShouldNotSortDependentItemsFirst()
        {
            var unorderedList = new List<SortTestItem>()
            {
                new SortTestItem("A", "B"),
                new SortTestItem("B"),
                new SortTestItem("C", "A", "B"),
                new SortTestItem("D"),
                new SortTestItem("E", "C", "D"),
            };

            var result = TopologicalSort.Sort(unorderedList, i => i.DependsOnValues, i => i.Value).ToList();

            for (var i = 1; i < result.Count; ++i)
            {
                var item = result[i];
                if (item.DependsOnValues != null && item.DependsOnValues.Length > 0)
                {
                    item.DependsOnValues.All(d => result.FindIndex(x => x.Value == d) <= i).ShouldBeTrue();
                }
            }
        }
    }
}
