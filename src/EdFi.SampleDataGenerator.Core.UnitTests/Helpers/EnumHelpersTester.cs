using EdFi.SampleDataGenerator.Core.Helpers;
using NUnit.Framework;
using Shouldly;

namespace EdFi.SampleDataGenerator.Core.UnitTests.Helpers
{
    public enum TestEnum
    {
        Free,

        [System.Xml.Serialization.XmlEnumAttribute("Full price")]
        Fullprice,
    }

    [TestFixture]
    public class EnumHelpersTester
    {
        [Test]
        public void ToEnumStringShouldUseEnumNameAbsentAttribute()
        {
            TestEnum free = TestEnum.Free;

            TestEnum.Free.ToCodeValue().ShouldBe("Free");
            free.ToCodeValue().ShouldBe("Free");
        }

        [Test]
        public void ToEnumStringShouldUseXmlAttributeValueIfPresentOnInstance()
        {
            TestEnum fullPrice = TestEnum.Fullprice;
            fullPrice.ToCodeValue().ShouldBe("Full price");
        }

        [Test]
        public void ToEnumStringShouldUseXmlAttributeValueIfPresentOnValue()
        {
            TestEnum.Fullprice.ToCodeValue().ShouldBe("Full price");
        }

        [Test]
        public void ShouldBuildXmlEnumMapCorrectly()
        {
            var map = EnumHelpers.GetEnumXmlValueMap<TestEnum>();
            map.Count.ShouldBe(2);
            map["Free"].ShouldBe(TestEnum.Free);
            map["Full price"].ShouldBe(TestEnum.Fullprice);
        }

        [Test]
        public void ShouldBuildEnumMapCorrectly()
        {
            var map = EnumHelpers.GetEnumValueMap<TestEnum>();
            map.Count.ShouldBe(2);
            map["Free"].ShouldBe(TestEnum.Free);
            map["Fullprice"].ShouldBe(TestEnum.Fullprice);
        }
    }
}
