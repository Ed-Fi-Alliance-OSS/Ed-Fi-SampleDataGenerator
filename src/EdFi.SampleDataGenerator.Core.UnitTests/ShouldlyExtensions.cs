using System.Collections.Generic;
using System.Linq;
using Shouldly;

namespace EdFi.SampleDataGenerator.Core.UnitTests
{
    public static class ShouldlyExtensions
    {
        public static void ShouldHaveSameContentAs<T>(this IEnumerable<T> expected, IEnumerable<T> actual)
        {
            expected.ShouldNotBeNull();
            actual.ShouldNotBeNull();
            expected.Count().ShouldBe(actual.Count());
            expected.ShouldAllBe(r => actual.Contains(r));
        }
    }
}
