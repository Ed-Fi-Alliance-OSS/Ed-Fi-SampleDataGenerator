using System.Collections.Generic;
using System.Linq;
using EdFi.SampleDataGenerator.Core.Helpers;
using NUnit.Framework;
using Shouldly;

namespace EdFi.SampleDataGenerator.Core.UnitTests.Helpers
{
    [TestFixture]
    public class EnumerableHelpersTester
    {
        private class TestBaseClass
        {
        }

        private class TestChildClass : TestBaseClass
        {
        }

        private class OtherTestChildClass : TestBaseClass
        {
        }

        [Test]
        public void SafeCastShouldReturnEmptyEnumerableOnNull()
        {
            List<string> nullList = null;
            var test = nullList.SafeCast<string, object>();

            test.ShouldNotBeNull();
            test.Count().ShouldBe(0);
        }

        

        [Test]
        public void SafeCastShouldActuallyCast()
        {
            var testList = new List<TestChildClass>(new[] {new TestChildClass()});
            
            //won't compile
            //List<TestClass> otherTestList = testList;

            var castedList = testList.SafeCast<TestChildClass, TestBaseClass>();
            IEnumerable<TestBaseClass> test = castedList;
        }

        [Test]
        public void ConcatShouldWork()
        {
            var list1 = new List<string>(new[] { "item 1" });
            var list2 = new List<string>(new[] { "item 2", "item 3" });

            var result = list1.Concat<string, string>(list2);
            result.Count().ShouldBe(3);
            result.ShouldContain("item 1");
            result.ShouldContain("item 2");
            result.ShouldContain("item 3");
        }

        [Test]
        public void ConcatShouldReturnEmptyEnumerablesWithNullInputs()
        {
            var list1 = new List<string>(new [] { "item 1" });

            var testResult1 = list1.Concat<string, string>(null);
            testResult1.ShouldNotBeNull();

            IEnumerable<string> nullEnumerable = null;
            var testResult2 = nullEnumerable.Concat<string, string>(list1);
            testResult2.ShouldNotBeNull();

            var testResult3 = nullEnumerable.Concat<string, string>(nullEnumerable);
            testResult3.ShouldNotBeNull();
        }

        [Test]
        public void ConcatShouldCast()
        {
            var testList = new List<TestChildClass>(new[] { new TestChildClass() });
            var otherList = new List<TestChildClass>(new[] { new TestChildClass() });

            IEnumerable<TestBaseClass> result = testList.Concat<TestChildClass, TestBaseClass>(otherList);
            result.Count().ShouldBe(2);
        }
    }
}
