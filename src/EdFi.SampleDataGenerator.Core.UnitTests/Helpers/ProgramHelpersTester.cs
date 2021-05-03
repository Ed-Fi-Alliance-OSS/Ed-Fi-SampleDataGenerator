using EdFi.SampleDataGenerator.Core.Entities;
using EdFi.SampleDataGenerator.Core.Helpers;
using NUnit.Framework;
using Shouldly;

namespace EdFi.SampleDataGenerator.Core.UnitTests.Helpers
{
    [TestFixture]
    public class ProgramHelpersTester
    {
        [Test]
        public void ReferencesSameProgramShouldBeTrueIfIdentityMatches()
        {
            var programReference = new ProgramReferenceType
            {
                @ref = null,
                ProgramIdentity = GetProgramIdentityType("Foo", ProgramTypeDescriptor.RegularEducation, "Regular"),
                ProgramLookup = null
            };

            var otherProgramReference = new ProgramReferenceType
            {
                @ref = null,
                ProgramIdentity = GetProgramIdentityType("Foo", ProgramTypeDescriptor.RegularEducation, "Regular"),
                ProgramLookup = null
            };

            programReference.ReferencesSameProgramAs(otherProgramReference).ShouldBeTrue();
        }

        [Test]
        public void ReferencesSameProgramShouldBeFalseIfIdentityDoNotMatch()
        {
            var programReference = new ProgramReferenceType
            {
                @ref = null,
                ProgramIdentity = GetProgramIdentityType("Foo", ProgramTypeDescriptor.RegularEducation, "Regular"),
                ProgramLookup = null
            };

            var otherProgramReference = new ProgramReferenceType
            {
                @ref = null,
                ProgramIdentity = GetProgramIdentityType("Bar", ProgramTypeDescriptor.RegularEducation, "Regular"),
                ProgramLookup = null
            };
            programReference.ReferencesSameProgramAs(otherProgramReference).ShouldBeFalse();

            otherProgramReference.ProgramIdentity = GetProgramIdentityType("Foo", ProgramTypeDescriptor.EnglishAsASecondLanguageESL, "Regular");
            programReference.ReferencesSameProgramAs(otherProgramReference).ShouldBeFalse();

            otherProgramReference.ProgramIdentity = GetProgramIdentityType("Foo", ProgramTypeDescriptor.RegularEducation, "ESL");
            programReference.ReferencesSameProgramAs(otherProgramReference).ShouldBeFalse();
        }

        [Test]
        public void ReferencesSameProgramShouldBeTrueIfLookupMatches()
        {
            var programReference = new ProgramReferenceType
            {
                @ref = null,
                ProgramIdentity = null,
                ProgramLookup = GetProgramLookupType("Foo", ProgramTypeDescriptor.RegularEducation, "Regular")
            };

            var otherProgramReference = new ProgramReferenceType
            {
                @ref = null,
                ProgramIdentity = null,
                ProgramLookup = GetProgramLookupType("Foo", ProgramTypeDescriptor.RegularEducation, "Regular")
            };

            programReference.ReferencesSameProgramAs(otherProgramReference).ShouldBeTrue();
        }

        [Test]
        public void ReferencesSameProgramShouldBeFalseIfLookupDoNotMatch()
        {
            var programReference = new ProgramReferenceType
            {
                @ref = null,
                ProgramIdentity = null,
                ProgramLookup = GetProgramLookupType("Foo", ProgramTypeDescriptor.RegularEducation, "Regular")
            };

            var otherProgramReference = new ProgramReferenceType
            {
                @ref = null,
                ProgramIdentity = null,
                ProgramLookup = GetProgramLookupType("Bar", ProgramTypeDescriptor.RegularEducation, "Regular")
            };
            programReference.ReferencesSameProgramAs(otherProgramReference).ShouldBeFalse();

            otherProgramReference.ProgramLookup = GetProgramLookupType("Foo", ProgramTypeDescriptor.EnglishAsASecondLanguageESL, "Regular");
            programReference.ReferencesSameProgramAs(otherProgramReference).ShouldBeFalse();

            otherProgramReference.ProgramLookup = GetProgramLookupType("Foo", ProgramTypeDescriptor.RegularEducation, "ESL");
            programReference.ReferencesSameProgramAs(otherProgramReference).ShouldBeFalse();
        }

        [Test]
        public void ReferencesSameProgramShouldBeTrueIfReferencesMatch()
        {
            var programReference = new ProgramReferenceType
            {
                @ref = "Test",
                ProgramIdentity = null,
                ProgramLookup = null
            };
            var otherProgramReference = new ProgramReferenceType
            {
                @ref = "Test",
                ProgramIdentity = null,
                ProgramLookup = null
            };

            programReference.ReferencesSameProgramAs(otherProgramReference).ShouldBeTrue();
        }

        [Test]
        public void ReferencesSameProgramShouldBeFalseIfReferencesDoNotMatch()
        {
            var programReference = new ProgramReferenceType
            {
                @ref = "Foo",
                ProgramIdentity = null,
                ProgramLookup = null
            };
            var otherProgramReference = new ProgramReferenceType
            {
                @ref = "Bar",
                ProgramIdentity = null,
                ProgramLookup = null
            };

            programReference.ReferencesSameProgramAs(otherProgramReference).ShouldBeFalse();
        }

        [Test]
        public void ReferencesSameProgramShouldBeFalseIfNull()
        {
            ProgramReferenceType programReference = null;
            ProgramReferenceType otherProgramReference = null;

            programReference.ReferencesSameProgramAs(otherProgramReference).ShouldBeFalse();
        }

        private ProgramIdentityType GetProgramIdentityType(string edOrgId, ProgramTypeDescriptor programType, string programName)
        {
            return new ProgramIdentityType
            {
                EducationOrganizationReference = new EducationOrganizationReferenceType { @ref = edOrgId },
                ProgramType = programType.GetStructuredCodeValue(),
                ProgramName = programName
            };
        }
        private ProgramLookupType GetProgramLookupType(string edOrgId, ProgramTypeDescriptor programType, string programName)
        {
            return new ProgramLookupType
            {
                EducationOrganizationReference = new EducationOrganizationReferenceType { @ref = edOrgId },
                ProgramType = programType.GetStructuredCodeValue(),
                ProgramName = programName
            };
        }
    }
}
