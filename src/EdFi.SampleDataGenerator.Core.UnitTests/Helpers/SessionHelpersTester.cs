using EdFi.SampleDataGenerator.Core.Entities;
using EdFi.SampleDataGenerator.Core.Helpers;
using NUnit.Framework;
using Shouldly;

namespace EdFi.SampleDataGenerator.Core.UnitTests.Helpers
{
    [TestFixture]
    public class SessionHelpersTester
    {
        [Test]
        public void ReferencesSameSessionShouldBeTrueIfIdentityMatches()
        {
            var sessionReference = new SessionReferenceType
            {
                @ref = null,
                SessionIdentity = GetSessionIdentityType(1, SchoolYearType.Item20042005, TermDescriptor.Semester),
                SessionLookup = null
            };

            var otherSessionReference = new SessionReferenceType
            {
                @ref = null,
                SessionIdentity = GetSessionIdentityType(1, SchoolYearType.Item20042005, TermDescriptor.Semester),
                SessionLookup = null
            };

            sessionReference.ReferencesSameSessionAs(otherSessionReference).ShouldBeTrue();
        }

        [Test]
        public void ReferencesSameSessionShouldBeFalseIfIdentityDoNotMatch()
        {
            var sessionReference = new SessionReferenceType
            {
                @ref = null,
                SessionIdentity = GetSessionIdentityType(1, SchoolYearType.Item20042005, TermDescriptor.Semester),
                SessionLookup = null
            };

            var otherSessionReference = new SessionReferenceType
            {
                @ref = null,
                SessionIdentity = GetSessionIdentityType(2, SchoolYearType.Item20162017, TermDescriptor.Quarter),
                SessionLookup = null
            };
            sessionReference.ReferencesSameSessionAs(otherSessionReference).ShouldBeFalse();

            otherSessionReference.SessionIdentity = GetSessionIdentityType(1, SchoolYearType.Item19931994, TermDescriptor.Semester);
            sessionReference.ReferencesSameSessionAs(otherSessionReference).ShouldBeFalse();

            otherSessionReference.SessionIdentity = GetSessionIdentityType(1, SchoolYearType.Item20042005, TermDescriptor.Quarter);
            sessionReference.ReferencesSameSessionAs(otherSessionReference).ShouldBeFalse();
        }

        [Test]
        public void ReferencesSameSessionShouldBeTrueIfLookupMatches()
        {
            var sessionReference = new SessionReferenceType
            {
                @ref = null,
                SessionIdentity = null,
                SessionLookup = GetSessionLookupType(1, SchoolYearType.Item20042005, TermDescriptor.Semester)
            };

            var otherSessionReference = new SessionReferenceType
            {
                @ref = null,
                SessionIdentity = null,
                SessionLookup = GetSessionLookupType(1, SchoolYearType.Item20042005, TermDescriptor.Semester)
            };

            sessionReference.ReferencesSameSessionAs(otherSessionReference).ShouldBeTrue();
        }

        [Test]
        public void ReferencesSameSessionShouldBeFalseIfLookupsDoNotMatch()
        {
            var sessionReference = new SessionReferenceType
            {
                @ref = null,
                SessionIdentity = null,
                SessionLookup = GetSessionLookupType(1, SchoolYearType.Item20042005, TermDescriptor.Semester)
            };

            var otherSessionReference = new SessionReferenceType
            {
                @ref = null,
                SessionIdentity = null,
                SessionLookup = GetSessionLookupType(2, SchoolYearType.Item20042005, TermDescriptor.Semester)
            };
            sessionReference.ReferencesSameSessionAs(otherSessionReference).ShouldBeFalse();

            otherSessionReference.SessionLookup = GetSessionLookupType(1, SchoolYearType.Item19901991, TermDescriptor.Semester);
            sessionReference.ReferencesSameSessionAs(otherSessionReference).ShouldBeFalse();

            otherSessionReference.SessionLookup = GetSessionLookupType(1, SchoolYearType.Item20042005, TermDescriptor.Quarter);
            sessionReference.ReferencesSameSessionAs(otherSessionReference).ShouldBeFalse();

            otherSessionReference.SessionLookup = GetSessionLookupType(1, SchoolYearType.Item20042005, TermDescriptor.Semester, "someothersessionname");
            sessionReference.ReferencesSameSessionAs(otherSessionReference).ShouldBeFalse();
        }

        [Test]
        public void ReferencesSameSessionShouldBeTrueIfReferencesMatch()
        {
            var sessionReference = new SessionReferenceType
            {
                @ref = "Test",
                SessionIdentity = null,
                SessionLookup = null
            };
            var otherSessionReference = new SessionReferenceType
            {
                @ref = "Test",
                SessionIdentity = null,
                SessionLookup = null
            };

            sessionReference.ReferencesSameSessionAs(otherSessionReference).ShouldBeTrue();
        }

        [Test]
        public void ReferencesSameSessionShouldBeFalseIfReferencesDoNotMatch()
        {
            var sessionReference = new SessionReferenceType
            {
                @ref = "Foo",
                SessionIdentity = null,
                SessionLookup = null
            };
            var otherSessionReference = new SessionReferenceType
            {
                @ref = "Bar",
                SessionIdentity = null,
                SessionLookup = null
            };

            sessionReference.ReferencesSameSessionAs(otherSessionReference).ShouldBeFalse();
        }

        [Test]
        public void ReferencesSameSessionShouldBeFalseIfNull()
        {
            SessionReferenceType sessionReference = null;
            SessionReferenceType otherSessionReference = null;

            sessionReference.ReferencesSameSessionAs(otherSessionReference).ShouldBeFalse();
        }

        private SessionIdentityType GetSessionIdentityType(int schoolId, SchoolYearType schoolYear, TermDescriptor termType)
        {
            return new SessionIdentityType
            {
                SchoolReference = new SchoolReferenceType { SchoolIdentity = new SchoolIdentityType { SchoolId = schoolId } },
                SchoolYear = schoolYear,
                SessionName = termType.CodeValue
            };
        }
        private SessionLookupType GetSessionLookupType(int schoolId, SchoolYearType schoolYear, TermDescriptor termType, string sessionName = null)
        {
            return new SessionLookupType
            {
                SchoolReference = new SchoolReferenceType { SchoolIdentity = new SchoolIdentityType { SchoolId = schoolId } },
                SchoolYear = schoolYear,
                Term = termType.CodeValue,
                SessionName = sessionName ?? termType.CodeValue
            };
        }
    }
}
