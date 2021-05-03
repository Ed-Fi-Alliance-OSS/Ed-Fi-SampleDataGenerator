using System;
using System.Collections.Generic;
using System.Linq;
using EdFi.SampleDataGenerator.Core.DataGeneration.Generators;
using EdFi.SampleDataGenerator.Core.Entities;
using EdFi.SampleDataGenerator.Core.Helpers;
using EdFi.SampleDataGenerator.Core.Serialization.Output;
using EdFi.SampleDataGenerator.Core.Serialization.Output.Interchanges;
using NUnit.Framework;
using Shouldly;

namespace EdFi.SampleDataGenerator.Core.UnitTests.Helpers
{
    [TestFixture]
    public class LanguageHelperTester
    {
        private static StudentDataGeneratorContext BuildContext()
        {
            return new StudentDataGeneratorContext
            {
                GeneratedStudentData = new GeneratedStudentData
                {
                    StudentEnrollmentData = new StudentEnrollmentData
                    {
                        StudentEducationOrganizationAssociation = new List<StudentEducationOrganizationAssociation>
                        {
                            new StudentEducationOrganizationAssociation()
                            {
                                StudentCharacteristic = new StudentCharacteristic[0],
                                StudentIndicator = new StudentIndicator[0],
                            }
                        }
                    }
                },
                GlobalStudentNumber = 1,
                StudentCharacteristics = new StudentCharacteristics()
            };
        }

        [Test]
        public void ShouldDetectLanguage()
        {
            var studentEd = new StudentEducationOrganizationAssociation();
            studentEd.AddLanguage(LanguageDescriptor.Spanish_spa, LanguageUseDescriptor.HomeLanguage);

            studentEd.SpeaksLanguage(LanguageDescriptor.Spanish_spa).ShouldBeTrue();
            studentEd.SpeaksLanguageWithUseType(LanguageDescriptor.Spanish_spa, LanguageUseDescriptor.HomeLanguage).ShouldBe(true);
        }

        [Test]
        public void ShouldNotDetectLanguageIfAnotherLanguageNotSet()
        {
             var studentEd = new StudentEducationOrganizationAssociation();
            studentEd.AddLanguage(LanguageDescriptor.English_eng, LanguageUseDescriptor.HomeLanguage);

            studentEd.SpeaksLanguage(LanguageDescriptor.Spanish_spa).ShouldBeFalse();
        }

        [Test]
        public void ShouldNotDetectLanguageIfNotSet()
        {
             var studentEd = new StudentEducationOrganizationAssociation();
            studentEd.SpeaksLanguage(LanguageDescriptor.Spanish_spa).ShouldBeFalse();
        }

        [Test]
        public void ShouldAddLanguageUseType()
        {
            var studentEd = new StudentEducationOrganizationAssociation
            {
                Language = new[]
                {
                    new Language
                    {
                        Language1 = LanguageDescriptor.English_eng.GetStructuredCodeValue(),
                        LanguageUse = new[]
                        {
                            LanguageUseDescriptor.CorrespondenceLanguage.GetStructuredCodeValue()
                        }
                    }
                }
            };

            studentEd.AddLanguage(LanguageDescriptor.English_eng, LanguageUseDescriptor.HomeLanguage);
            studentEd.Language.Length.ShouldBe(1);
            studentEd.Language[0].Language1.ShouldBe(LanguageDescriptor.English_eng.GetStructuredCodeValue());
            studentEd.Language[0].LanguageUse.Length.ShouldBe(2);
            studentEd.Language[0].LanguageUse.Contains(LanguageUseDescriptor.CorrespondenceLanguage.GetStructuredCodeValue()).ShouldBeTrue();
            studentEd.Language[0].LanguageUse.Contains(LanguageUseDescriptor.HomeLanguage.GetStructuredCodeValue()).ShouldBeTrue();
        }

        [Test]
        public void ShouldAddLanguage()
        {
             var studentEd = new StudentEducationOrganizationAssociation();

            studentEd.AddLanguage(LanguageDescriptor.English_eng, LanguageUseDescriptor.HomeLanguage);
            studentEd.Language.Length.ShouldBe(1);
            studentEd.Language[0].Language1.ShouldBe(LanguageDescriptor.English_eng.GetStructuredCodeValue());
            studentEd.Language[0].LanguageUse.Length.ShouldBe(1);
            studentEd.Language[0].LanguageUse.Contains(LanguageUseDescriptor.HomeLanguage.GetStructuredCodeValue()).ShouldBeTrue();
        }

        [Test]
        public void ShouldNotOverwriteExistingLanguage()
        {
            var studentEd = new StudentEducationOrganizationAssociation
            {
                Language = new[]
                {
                    new Language
                    {
                        Language1 = LanguageDescriptor.English_eng.GetStructuredCodeValue(),
                        LanguageUse = new[]
                        {
                            LanguageUseDescriptor.CorrespondenceLanguage.GetStructuredCodeValue(),
                        }
                    }
                }
            };
            studentEd.AddLanguage(LanguageDescriptor.Spanish_spa, LanguageUseDescriptor.HomeLanguage);
            studentEd.Language.Length.ShouldBe(2);
            studentEd.Language.Count(l => l.Language1 == LanguageDescriptor.Spanish_spa.GetStructuredCodeValue() && l.LanguageUse[0] == LanguageUseDescriptor.HomeLanguage.GetStructuredCodeValue()).ShouldBe(1);
            studentEd.Language.Count(l => l.Language1 == LanguageDescriptor.English_eng.GetStructuredCodeValue() && l.LanguageUse[0] == LanguageUseDescriptor.CorrespondenceLanguage.GetStructuredCodeValue()).ShouldBe(1);
        }

        [Test]
        public void ShouldSetPrimaryLanguage()
        {
             var studentEd = new StudentEducationOrganizationAssociation();

            studentEd.SetPrimaryLanguage(LanguageDescriptor.English_eng);
            studentEd.Language.Length.ShouldBe(1);
            studentEd.Language[0].Language1.ShouldBe(LanguageDescriptor.English_eng.GetStructuredCodeValue());
            studentEd.Language[0].LanguageUse.Length.ShouldBe(1);
            studentEd.Language[0].LanguageUse.Contains(StudentEducationOrganizationHelper.PrimaryLanguageUseType.GetStructuredCodeValue()).ShouldBeTrue();
        }

        [Test]
        public void ShouldGetPrimaryLanguage()
        {
            var context = BuildContext();
            var studentEd = context.GetStudentEducationOrganization();

            studentEd.SetPrimaryLanguage(LanguageDescriptor.Spanish_spa);
            studentEd.AddLanguage(LanguageDescriptor.English_eng, LanguageUseDescriptor.OtherLanguageProficiency);
            studentEd.Language.Length.ShouldBe(2);
            context.GetStudentEducationOrganization().GetPrimaryLanguage().ShouldBe(LanguageDescriptor.Spanish_spa);
        }

        [Test]
        public void ShouldThrowIfPrimaryLanguageAlreadySet()
        {
             var studentEd = new StudentEducationOrganizationAssociation();

            studentEd.SetPrimaryLanguage(LanguageDescriptor.Spanish_spa);
            Assert.Throws<InvalidOperationException>(() => studentEd.SetPrimaryLanguage(LanguageDescriptor.English_eng));
        }

        [Test]
        public void ShouldThrowIfPrimaryLanguageAddedTwice()
        {
            var studentEd = new StudentEducationOrganizationAssociation();
            studentEd.SetPrimaryLanguage(LanguageDescriptor.Spanish_spa);
            Assert.Throws<InvalidOperationException>(() => studentEd.AddLanguage(LanguageDescriptor.English_eng, StudentEducationOrganizationHelper.PrimaryLanguageUseType));
        }
    }
}
