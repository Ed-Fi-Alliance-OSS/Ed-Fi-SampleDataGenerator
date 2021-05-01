using System.Linq;
using EdFi.SampleDataGenerator.Core.Helpers;
using NUnit.Framework;
using Shouldly;

namespace EdFi.SampleDataGenerator.Core.UnitTests.Helpers
{
    [TestFixture]
    public class PropertyTests
    {
        private class Sample
        {
            public int Integer { get; set; }
            public int? NullableInteger { get; set; }
            public Sample Parent { get; set; }
        }

        [Test]
        public void ShouldGetPropertyInfoFromLambdaExpression()
        {
            var integer = Property.From<Sample>(x => x.Integer);
            integer.Name.ShouldBe("Integer");
            integer.PropertyType.ShouldBe(typeof(int));

            var nullableInteger = Property.From<Sample>(x => x.NullableInteger);
            nullableInteger.Name.ShouldBe("NullableInteger");
            nullableInteger.PropertyType.ShouldBe(typeof(int?));

            var parent = Property.From<Sample>(x => x.Parent);
            parent.Name.ShouldBe("Parent");
            parent.PropertyType.ShouldBe(typeof(Sample));

            //Property.From<T> returns the deepest property from the expression.
            var nested = Property.From<Sample>(x => x.Parent.Parent.Integer);
            nested.Name.ShouldBe("Integer");
            nested.PropertyType.ShouldBe(typeof(int));
        }

        [Test]
        public void ShouldGetFullNameFromPropertyChainLambdaExpression()
        {
            var integer = Property.Path<Sample>(x => x.Integer).Single();
            integer.Name.ShouldBe("Integer");
            integer.PropertyType.ShouldBe(typeof(int));

            var nullableInteger = Property.Path<Sample>(x => x.NullableInteger).Single();
            nullableInteger.Name.ShouldBe("NullableInteger");
            nullableInteger.PropertyType.ShouldBe(typeof(int?));

            var parent = Property.Path<Sample>(x => x.Parent).Single();
            parent.Name.ShouldBe("Parent");
            parent.PropertyType.ShouldBe(typeof(Sample));

            var nested = Property.Path<Sample>(x => x.Parent.Integer);
            nested.Count.ShouldBe(2);
            nested[0].Name.ShouldBe("Parent");
            nested[0].PropertyType.ShouldBe(typeof(Sample));
            nested[1].Name.ShouldBe("Integer");
            nested[1].PropertyType.ShouldBe(typeof(int));

            var deeplyNested = Property.Path<Sample>(x => x.Parent.Parent.Integer);
            deeplyNested.Count.ShouldBe(3);
            deeplyNested[0].Name.ShouldBe("Parent");
            deeplyNested[0].PropertyType.ShouldBe(typeof(Sample));
            deeplyNested[1].Name.ShouldBe("Parent");
            deeplyNested[1].PropertyType.ShouldBe(typeof(Sample));
            deeplyNested[2].Name.ShouldBe("Integer");
            deeplyNested[2].PropertyType.ShouldBe(typeof(int));
        }
    }
}