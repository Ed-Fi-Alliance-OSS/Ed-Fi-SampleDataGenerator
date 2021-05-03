using System;
using EdFi.SampleDataGenerator.Core.Entities;
using EdFi.SampleDataGenerator.Core.Helpers;
using NUnit.Framework;
using Shouldly;

namespace EdFi.SampleDataGenerator.Core.UnitTests.Helpers
{
    [TestFixture]
    public class DescriptorHelpersTester
    {
        [Test]
        public void ShouldThrowOnNonDescriptorType()
        {
            Assert.Throws<InvalidOperationException>(() => typeof (string).DescriptorNameFromType());
        }

        [Test]
        public void ShouldGetDescriptorNameForValidDescriptorType()
        {
            var name = typeof (AcademicSubjectDescriptor).DescriptorNameFromType();
            name.ShouldBe("AcademicSubject");
        }

        [Test]
        public void ShouldReturnNullTypeWhenGivenInvalidDescriptorTypeName()
        {
            var type = DescriptorHelpers.DescriptorTypeFromName("fake");
            type.ShouldBeNull();
        }

        [Test]
        public void ShouldGetDescriptorTypeWithValidName()
        {
            var type = DescriptorHelpers.DescriptorTypeFromName("AcademicSubject");
            type.ShouldBe(typeof(AcademicSubjectDescriptor));
        }

        [Test]
        public void ShouldParseDescriptorFromCodeValue()
        {
            var codeValue =  GradeLevelDescriptor.FirstGrade.CodeValue;

            var descriptor = codeValue.ToDescriptorFromCodeValue<GradeLevelDescriptor>();

            descriptor.CodeValue.ShouldBe(GradeLevelDescriptor.FirstGrade.CodeValue);
        }

        [Test]
        public void ShouldParseDescriptorFromName()
        {
            var descriptorName = "FirstGrade";

            var descriptor = descriptorName.ToDescriptorFromName<GradeLevelDescriptor>();

            descriptor.CodeValue.ShouldBe(GradeLevelDescriptor.FirstGrade.CodeValue);
        }

        [Test]
        public void ShouldBeParseableToDescriptorFromName()
        {
            var descriptorName = "FirstGrade";

            DescriptorHelpers.IsParseableToDescriptorFromName<GradeLevelDescriptor>(descriptorName).ShouldBe(true);
        }

        [Test]
        public void ShouldBeParseableToDescriptorFromCodeValue()
        {
            var codeValue = GradeLevelDescriptor.FirstGrade.CodeValue;

            DescriptorHelpers.IsParseableToDescriptorFromCodeValue<GradeLevelDescriptor>(codeValue).ShouldBe(true);
        }

        [Test]
        public void ShouldTryParseFromCodeValue()
        {
            var codeValue = GradeLevelDescriptor.FirstGrade.CodeValue;
            GradeLevelDescriptor gradeLevelDescriptor;

            var result = DescriptorHelpers.TryParseFromCodeValue(codeValue, out gradeLevelDescriptor);

            result.ShouldBe(true);
            gradeLevelDescriptor.CodeValue.ShouldBe(GradeLevelDescriptor.FirstGrade.CodeValue);

        }

        [Test]
        public void ShouldTryParseFromCodeValueIgnoringCase()
        {
            var codeValue = GradeLevelDescriptor.FirstGrade.CodeValue.ToLower();
            GradeLevelDescriptor gradeLevelDescriptor;

            var result = DescriptorHelpers.TryParseFromCodeValue(codeValue, true, out gradeLevelDescriptor);

            result.ShouldBe(true);
            gradeLevelDescriptor.CodeValue.ShouldBe(GradeLevelDescriptor.FirstGrade.CodeValue);

        }

        [Test]
        public void ShouldParseFromStructuredCodeValue()
        {
            var english = LanguageDescriptor.English_eng.GetStructuredCodeValue();
            var spanish = LanguageDescriptor.Spanish_spa.GetStructuredCodeValue();
            var french = LanguageDescriptor.French_fre.GetStructuredCodeValue();
            var firstGrade = GradeLevelDescriptor.FirstGrade.GetStructuredCodeValue();

            english.ParseFromStructuredCodeValue<LanguageDescriptor>().ShouldBe(LanguageDescriptor.English_eng);
            spanish.ParseFromStructuredCodeValue<LanguageDescriptor>().ShouldBe(LanguageDescriptor.Spanish_spa);
            french.ParseFromStructuredCodeValue<LanguageDescriptor>().ShouldBe(LanguageDescriptor.French_fre);

            firstGrade.ParseFromStructuredCodeValue<GradeLevelDescriptor>().ShouldBe(GradeLevelDescriptor.FirstGrade);

            Assert.Throws<Exception>(() => firstGrade.ParseFromStructuredCodeValue<LanguageDescriptor>())
                .Message.ShouldBe("LanguageDescriptor not found for: " + firstGrade);
        }

        [Test]
        public void ShouldFindFirstOrDefaultDescriptorMatchingSomeGivenCondition()
        {
            DescriptorHelpers.FirstOrDefault<LanguageDescriptor>(x => x.CodeValue == "eng")
                .ShouldBe(LanguageDescriptor.English_eng);

            DescriptorHelpers.FirstOrDefault<LanguageDescriptor>(x => x.GetStructuredCodeValue() == "uri://ed-fi.org/LanguageDescriptor#spa")
                .ShouldBe(LanguageDescriptor.Spanish_spa);

            DescriptorHelpers.FirstOrDefault<LanguageDescriptor>(x => x.CodeValue == "TYPO")
                .ShouldBeNull();
        }

        [Test]
        public void IsDescriptorFromCodeValue()
        {
            var codeValue = GradeLevelDescriptor.FirstGrade.CodeValue;
            var codeValueLowerCase = GradeLevelDescriptor.FirstGrade.CodeValue.ToLower();

            codeValue.Is(GradeLevelDescriptor.FirstGrade).ShouldBe(true);
            codeValueLowerCase.Is(GradeLevelDescriptor.FirstGrade).ShouldBe(true);

        }
    }
}
