using System;
using System.Collections.Generic;
using System.Linq;
using EdFi.SampleDataGenerator.Core.DataGeneration.Generators;
using EdFi.SampleDataGenerator.Core.DataGeneration.Generators.Student;
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
            studentEd.AddLanguage(LanguageMapType.Spanish, LanguageUseType.Homelanguage);

            studentEd.SpeaksLanguage(LanguageMapType.Spanish).ShouldBeTrue();
            studentEd.SpeaksLanguageWithUseType(LanguageMapType.Spanish, LanguageUseType.Homelanguage).ShouldBe(true);
        }

        [Test]
        public void ShouldNotDetectLanguageIfAnotherLanguageNotSet()
        {
             var studentEd = new StudentEducationOrganizationAssociation();
            studentEd.AddLanguage(LanguageMapType.English, LanguageUseType.Homelanguage);

            studentEd.SpeaksLanguage(LanguageMapType.Spanish).ShouldBeFalse();
        }

        [Test]
        public void ShouldNotDetectLanguageIfNotSet()
        {
             var studentEd = new StudentEducationOrganizationAssociation();
            studentEd.SpeaksLanguage(LanguageMapType.Spanish).ShouldBeFalse();
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
                        Language1 = LanguageMapType.English.Value,
                        LanguageUse = new[]
                        {
                            LanguageUseType.Correspondencelanguage.Value,
                        }
                    }
                }
            };

            studentEd.AddLanguage(LanguageMapType.English, LanguageUseType.Homelanguage);
            studentEd.Language.Length.ShouldBe(1);
            studentEd.Language[0].Language1.ShouldBe(LanguageMapType.English.Value);
            studentEd.Language[0].LanguageUse.Length.ShouldBe(2);
            studentEd.Language[0].LanguageUse.Contains(LanguageUseType.Correspondencelanguage.Value).ShouldBeTrue();
            studentEd.Language[0].LanguageUse.Contains(LanguageUseType.Homelanguage.Value).ShouldBeTrue();
        }

        [Test]
        public void ShouldAddLanguage()
        {
             var studentEd = new StudentEducationOrganizationAssociation();

            studentEd.AddLanguage(LanguageMapType.English, LanguageUseType.Homelanguage);
            studentEd.Language.Length.ShouldBe(1);
            studentEd.Language[0].Language1.ShouldBe(LanguageMapType.English.Value);
            studentEd.Language[0].LanguageUse.Length.ShouldBe(1);
            studentEd.Language[0].LanguageUse.Contains(LanguageUseType.Homelanguage.Value).ShouldBeTrue();
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
                        Language1 = LanguageMapType.English.Value,
                        LanguageUse = new[]
                        {
                            LanguageUseType.Correspondencelanguage.Value,
                        }
                    }
                }
            };
            studentEd.AddLanguage(LanguageMapType.Spanish, LanguageUseType.Homelanguage);
            studentEd.Language.Length.ShouldBe(2);
            studentEd.Language.Count(l => l.Language1 == LanguageMapType.Spanish.Value && l.LanguageUse[0] == LanguageUseType.Homelanguage.Value).ShouldBe(1);
            studentEd.Language.Count(l => l.Language1 == LanguageMapType.English.Value && l.LanguageUse[0] == LanguageUseType.Correspondencelanguage.Value).ShouldBe(1);
        }

        [Test]
        public void ShouldSetPrimaryLanguage()
        {
             var studentEd = new StudentEducationOrganizationAssociation();

            studentEd.SetPrimaryLanguage(LanguageMapType.English);
            studentEd.Language.Length.ShouldBe(1);
            studentEd.Language[0].Language1.ShouldBe(LanguageMapType.English.Value);
            studentEd.Language[0].LanguageUse.Length.ShouldBe(1);
            studentEd.Language[0].LanguageUse.Contains(StudentEducationOrganizationHelper.PrimaryLanguageUseType).ShouldBeTrue();
        }

        [Test]
        public void ShouldGetPrimaryLanguage()
        {
            var context = BuildContext();
            var studentEd = context.GetStudentEducationOrganization();

            studentEd.SetPrimaryLanguage(LanguageMapType.Spanish);
            studentEd.AddLanguage(LanguageMapType.English, LanguageUseType.Otherlanguageproficiency);
            studentEd.Language.Length.ShouldBe(2);
            context.GetStudentEducationOrganization().GetPrimaryLanguage().ShouldBe(LanguageMapType.Spanish);
        }

        [Test]
        public void ShouldThrowIfPrimaryLanguageAlreadySet()
        {
             var studentEd = new StudentEducationOrganizationAssociation();

            studentEd.SetPrimaryLanguage(LanguageMapType.Spanish);
            Assert.Throws<InvalidOperationException>(() => studentEd.SetPrimaryLanguage(LanguageMapType.English));
        }

        [Test]
        public void ShouldThrowIfPrimaryLanguageAddedTwice()
        {
            var studentEd = new StudentEducationOrganizationAssociation();
            studentEd.SetPrimaryLanguage(LanguageMapType.Spanish);
            Assert.Throws<InvalidOperationException>(() => studentEd.AddLanguage(LanguageMapType.English, StudentEducationOrganizationHelper.PrimaryLanguageUseType));
        }
    }
}
