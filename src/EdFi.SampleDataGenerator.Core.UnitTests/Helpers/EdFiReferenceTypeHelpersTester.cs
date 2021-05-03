using EdFi.SampleDataGenerator.Core.Entities;
using EdFi.SampleDataGenerator.Core.Helpers;
using NUnit.Framework;
using Shouldly;

namespace EdFi.SampleDataGenerator.Core.UnitTests.Helpers
{
    [TestFixture]
    public class EdFiReferenceTypeHelpersTester
    {
        [Test]
        public void ReferenceCheckShouldFailIfReferenceIsNull()
        {
            GraduationPlanReferenceType refType = null;
            refType.References("Test").ShouldBeFalse();
        }

        [Test]
        public void ReferenceCheckShouldFailIfRefIsNull()
        {
            GraduationPlanReferenceType refType = new GraduationPlanReferenceType {};
            refType.References("Test").ShouldBeFalse();
        }

        [Test]
        public void ReferenceCheckShouldFailIfRefDoesNotMatchId()
        {
            GraduationPlanReferenceType refType = new GraduationPlanReferenceType {@ref = "Foo"};
            refType.References("Test").ShouldBeFalse();
        }

        [Test]
        public void ReferenceCheckShouldSucceedIfRefMatchesId()
        {
            GraduationPlanReferenceType refType = new GraduationPlanReferenceType {@ref = "Test"};
            refType.References("Test").ShouldBeTrue();
        }

        [Test]
        public void ReferenceCheckShouldBeCaseInsensitive()
        {
            GraduationPlanReferenceType refType = new GraduationPlanReferenceType {@ref = "test"};
            refType.References("Test").ShouldBeTrue();
        }

        [Test]
        public void ReferencesGraduationPlanShouldBeFalseIfReferenceIsNull()
        {
            GraduationPlanReferenceType graduationPlanReference = null;
            var graduationPlan = new GraduationPlan
            {
                id = "Test",
                EducationOrganizationReference = new EducationOrganizationReferenceType {},
                GraduationPlanType = GraduationPlanTypeDescriptor.Recommended.GetStructuredCodeValue(),
                GraduationSchoolYear = SchoolYearType.Item19901991
            };

            graduationPlanReference.ReferencesGraduationPlan(graduationPlan).ShouldBeFalse();
        }

        [Test]
        public void ReferencesGraduationPlanShouldBeFalseIfGraduationPlanIsNull()
        {
            var graduationPlanReference = new GraduationPlanReferenceType { @ref = null, GraduationPlanIdentity = null };
            GraduationPlan graduationPlan = null;

            graduationPlanReference.ReferencesGraduationPlan(graduationPlan).ShouldBeFalse();
        }

        [Test]
        public void ReferencesGraduationPlanShouldBeTrueIfReferenceIsValid()
        {
            var graduationPlanReference = new GraduationPlanReferenceType { @ref = "Foo", GraduationPlanIdentity = null };
            var graduationPlan = new GraduationPlan
            {
                id = "Foo",
                EducationOrganizationReference = new EducationOrganizationReferenceType { },
                GraduationPlanType = GraduationPlanTypeDescriptor.Recommended.GetStructuredCodeValue(),
                GraduationSchoolYear = SchoolYearType.Item19901991
            };

            graduationPlanReference.ReferencesGraduationPlan(graduationPlan).ShouldBeTrue();
        }

        [Test]
        public void ReferencesGraduationPlanShouldBeFalseIfReferenceIsInvalid()
        {
            var graduationPlanReference = new GraduationPlanReferenceType { @ref = "Foo", GraduationPlanIdentity = null };
            var graduationPlan = new GraduationPlan
            {
                id = "Bar",
                EducationOrganizationReference = new EducationOrganizationReferenceType { },
                GraduationPlanType = GraduationPlanTypeDescriptor.Recommended.GetStructuredCodeValue(),
                GraduationSchoolYear = SchoolYearType.Item19901991
            };

            graduationPlanReference.ReferencesGraduationPlan(graduationPlan).ShouldBeFalse();
        }

        [Test]
        public void ReferencesGraduationPlanShouldBeTrueIfIdentityEdOrgRefMatches()
        {
            var graduationPlanReference = new GraduationPlanReferenceType
            {
                @ref = null,
                GraduationPlanIdentity = new GraduationPlanIdentityType
                {
                    EducationOrganizationReference = new EducationOrganizationReferenceType { @ref = "Foo" },
                    GraduationPlanType = GraduationPlanTypeDescriptor.Recommended.GetStructuredCodeValue(),
                    GraduationSchoolYear = SchoolYearType.Item19901991
                }
            };
            var graduationPlan = new GraduationPlan
            {
                id = "Foo",
                EducationOrganizationReference = new EducationOrganizationReferenceType { @ref = "Foo" },
                GraduationPlanType = GraduationPlanTypeDescriptor.Recommended.GetStructuredCodeValue(),
                GraduationSchoolYear = SchoolYearType.Item19901991
            };

            graduationPlanReference.ReferencesGraduationPlan(graduationPlan).ShouldBeTrue();
        }

        [Test]
        public void ReferencesGraduationPlanShouldBeTrueIfIdentityEdOrgRefIdentityMatches()
        {
            var graduationPlanReference = new GraduationPlanReferenceType
            {
                @ref = null,
                GraduationPlanIdentity = new GraduationPlanIdentityType
                {
                    EducationOrganizationReference = new EducationOrganizationReferenceType { EducationOrganizationIdentity = new EducationOrganizationIdentityType { EducationOrganizationId = 1 } },
                    GraduationPlanType = GraduationPlanTypeDescriptor.Recommended.GetStructuredCodeValue(),
                    GraduationSchoolYear = SchoolYearType.Item19901991
                }
            };
            var graduationPlan = new GraduationPlan
            {
                id = "Foo",
                EducationOrganizationReference = new EducationOrganizationReferenceType { EducationOrganizationIdentity = new EducationOrganizationIdentityType { EducationOrganizationId = 1 } },
                GraduationPlanType = GraduationPlanTypeDescriptor.Recommended.GetStructuredCodeValue(),
                GraduationSchoolYear = SchoolYearType.Item19901991
            };

            graduationPlanReference.ReferencesGraduationPlan(graduationPlan).ShouldBeTrue();
        }

        [Test]
        public void ReferencesGraduationPlanShouldBeTrueIfIdentityEdOrgRefLookupMatches()
        {
            var graduationPlanReference = new GraduationPlanReferenceType
            {
                @ref = null,
                GraduationPlanIdentity = new GraduationPlanIdentityType
                {
                    EducationOrganizationReference = new EducationOrganizationReferenceType { EducationOrganizationLookup = new EducationOrganizationLookupType { EducationOrganizationId = 1} },
                    GraduationPlanType = GraduationPlanTypeDescriptor.Recommended.GetStructuredCodeValue(),
                    GraduationSchoolYear = SchoolYearType.Item19901991
                }
            };
            var graduationPlan = new GraduationPlan
            {
                id = "Foo",
                EducationOrganizationReference = new EducationOrganizationReferenceType { EducationOrganizationLookup = new EducationOrganizationLookupType { EducationOrganizationId = 1 } },
                GraduationPlanType = GraduationPlanTypeDescriptor.Recommended.GetStructuredCodeValue(),
                GraduationSchoolYear = SchoolYearType.Item19901991
            };

            graduationPlanReference.ReferencesGraduationPlan(graduationPlan).ShouldBeTrue();
        }

        [Test]
        public void ReferencesGraduationPlanShouldBeFalseIfEdOrgRefDoesNotMatch()
        {
            var graduationPlanReference = new GraduationPlanReferenceType
            {
                @ref = null,
                GraduationPlanIdentity = new GraduationPlanIdentityType
                {
                    EducationOrganizationReference = new EducationOrganizationReferenceType { @ref = "Foo" },
                    GraduationPlanType = GraduationPlanTypeDescriptor.Recommended.GetStructuredCodeValue(),
                    GraduationSchoolYear = SchoolYearType.Item19901991
                }
            };
            var graduationPlan = new GraduationPlan
            {
                id = "Test",
                EducationOrganizationReference = new EducationOrganizationReferenceType { @ref = "Bar" },
                GraduationPlanType = GraduationPlanTypeDescriptor.Recommended.GetStructuredCodeValue(),
                GraduationSchoolYear = SchoolYearType.Item19901991
            };

            graduationPlanReference.ReferencesGraduationPlan(graduationPlan).ShouldBeFalse();
        }

        [Test]
        public void ReferencesGraduationPlanShouldBeFalseIfEdOrgIdentityDoesNotMatch()
        {
            var graduationPlanReference = new GraduationPlanReferenceType
            {
                @ref = null,
                GraduationPlanIdentity = new GraduationPlanIdentityType
                {
                    EducationOrganizationReference = new EducationOrganizationReferenceType { EducationOrganizationIdentity = new EducationOrganizationIdentityType {EducationOrganizationId = 1} },
                    GraduationPlanType = GraduationPlanTypeDescriptor.Recommended.GetStructuredCodeValue(),
                    GraduationSchoolYear = SchoolYearType.Item19901991
                }
            };
            var graduationPlan = new GraduationPlan
            {
                id = "Test",
                EducationOrganizationReference = new EducationOrganizationReferenceType { EducationOrganizationIdentity = new EducationOrganizationIdentityType { EducationOrganizationId = 2 } },
                GraduationPlanType = GraduationPlanTypeDescriptor.Recommended.GetStructuredCodeValue(),
                GraduationSchoolYear = SchoolYearType.Item19901991
            };

            graduationPlanReference.ReferencesGraduationPlan(graduationPlan).ShouldBeFalse();
        }

        [Test]
        public void ReferencesGraduationPlanShouldBeFalseIfIdentityGraduationPlanTypeDoesNotMatch()
        {
            var graduationPlanReference = new GraduationPlanReferenceType
            {
                @ref = null,
                GraduationPlanIdentity = new GraduationPlanIdentityType
                {
                    EducationOrganizationReference = new EducationOrganizationReferenceType { @ref = "Foo" },
                    GraduationPlanType = GraduationPlanTypeDescriptor.Minimum.GetStructuredCodeValue(),
                    GraduationSchoolYear = SchoolYearType.Item19901991
                }
            };
            var graduationPlan = new GraduationPlan
            {
                id = "Test",
                EducationOrganizationReference = new EducationOrganizationReferenceType { @ref = "Foo" },
                GraduationPlanType = GraduationPlanTypeDescriptor.Recommended.GetStructuredCodeValue(),
                GraduationSchoolYear = SchoolYearType.Item19901991
            };

            graduationPlanReference.ReferencesGraduationPlan(graduationPlan).ShouldBeFalse();
        }

        [Test]
        public void ReferencesGraduationPlanShouldBeFalseIfIdentityGraduationSchoolYearDoesNotMatch()
        {
            var graduationPlanReference = new GraduationPlanReferenceType
            {
                @ref = null,
                GraduationPlanIdentity = new GraduationPlanIdentityType
                {
                    EducationOrganizationReference = new EducationOrganizationReferenceType { @ref = "Foo" },
                    GraduationPlanType = GraduationPlanTypeDescriptor.Recommended.GetStructuredCodeValue(),
                    GraduationSchoolYear = SchoolYearType.Item19911992
                }
            };
            var graduationPlan = new GraduationPlan
            {
                id = "Test",
                EducationOrganizationReference = new EducationOrganizationReferenceType { @ref = "Foo" },
                GraduationPlanType = GraduationPlanTypeDescriptor.Recommended.GetStructuredCodeValue(),
                GraduationSchoolYear = SchoolYearType.Item19901991
            };

            graduationPlanReference.ReferencesGraduationPlan(graduationPlan).ShouldBeFalse();
        }

        [Test]
        public void ReferencesSameEntityShouldBeFalseIfRefIsNull()
        {
            GraduationPlanReferenceType refType = new GraduationPlanReferenceType { @ref = "test" };
            GraduationPlanReferenceType otherRefType = null;

            refType.ReferencesSameEntityAs(otherRefType).ShouldBeFalse();
        }

        [Test]
        public void ReferencesSameEntityShouldBeFalseIfRefsDoNotMatch()
        {
            GraduationPlanReferenceType refType = new GraduationPlanReferenceType { @ref = "ref1" };
            GraduationPlanReferenceType otherRefType = new GraduationPlanReferenceType { @ref = "ref2" };

            refType.ReferencesSameEntityAs(otherRefType).ShouldBeFalse();
        }

        [Test]
        public void ReferencesSameEntityShoudlBeTrueIfRefsMatch()
        {
            GraduationPlanReferenceType refType = new GraduationPlanReferenceType { @ref = "ref1" };
            GraduationPlanReferenceType otherRefType = new GraduationPlanReferenceType { @ref = "ref1" };

            refType.ReferencesSameEntityAs(otherRefType).ShouldBeTrue();
        }

        [Test]
        public void ReferencesSameEducationOrganizationShouldBeTrueIfIdentityMatches()
        {
            var edOrgReference = new EducationOrganizationReferenceType
            {
                @ref = null,
                EducationOrganizationIdentity = new EducationOrganizationIdentityType {EducationOrganizationId = 1},
                EducationOrganizationLookup = null
            };

            var otherEdOrgReference = new EducationOrganizationReferenceType
            {
                @ref = null,
                EducationOrganizationIdentity = new EducationOrganizationIdentityType {EducationOrganizationId = 1},
                EducationOrganizationLookup = null
            };

            edOrgReference.ReferencesSameEducationOrganizationAs(otherEdOrgReference).ShouldBeTrue();
        }

        [Test]
        public void ReferencesSameEducationOrganizationShouldBeTrueIfLookupMatches()
        {
            var edOrgReference = new EducationOrganizationReferenceType
            {
                @ref = null,
                EducationOrganizationIdentity = null,
                EducationOrganizationLookup = new EducationOrganizationLookupType { EducationOrganizationId = 1}
            };

            var otherEdOrgReference = new EducationOrganizationReferenceType
            {
                @ref = null,
                EducationOrganizationIdentity = null,
                EducationOrganizationLookup = new EducationOrganizationLookupType { EducationOrganizationId = 1}
            };

            edOrgReference.ReferencesSameEducationOrganizationAs(otherEdOrgReference).ShouldBeTrue();
        }

        [Test]
        public void ReferencesSameEducationOrganizationShouldBeTrueIfReferencesMatch()
        {
            var edOrgReference = new EducationOrganizationReferenceType
            {
                @ref = "Test",
                EducationOrganizationIdentity = null,
                EducationOrganizationLookup = null
            };

            var otherEdOrgReference = new EducationOrganizationReferenceType
            {
                @ref = "Test",
                EducationOrganizationIdentity = null,
                EducationOrganizationLookup = null
            };

            edOrgReference.ReferencesSameEducationOrganizationAs(otherEdOrgReference).ShouldBeTrue();
        }

        [Test]
        public void ReferencesSameEducationOrganizationShouldBeFalseIfReferencesDoNotMatch()
        {
            var edOrgReference = new EducationOrganizationReferenceType
            {
                @ref = "Foo",
                EducationOrganizationIdentity = null,
                EducationOrganizationLookup = null
            };

            var otherEdOrgReference = new EducationOrganizationReferenceType
            {
                @ref = "Bar",
                EducationOrganizationIdentity = null,
                EducationOrganizationLookup = null
            };

            edOrgReference.ReferencesSameEducationOrganizationAs(otherEdOrgReference).ShouldBeFalse();
        }

        [Test]
        public void ReferencesSameEducationOrganizationShouldBeFalseIfNull()
        {
            EducationOrganizationReferenceType edOrgReference = null;
            EducationOrganizationReferenceType otherEdOrgReference = null;

            edOrgReference.ReferencesSameEducationOrganizationAs(otherEdOrgReference).ShouldBeFalse();
        }

        [Test]
        public void ReferencesSameSchoolShouldBeTrueIfIdentityMatches()
        {
            var schoolReference = new SchoolReferenceType
            {
                @ref = null,
                SchoolIdentity = new SchoolIdentityType
                {
                    SchoolId = 1
                },
                SchoolLookup = null
            };

            var otherSchoolReference = new SchoolReferenceType
            {
                @ref = null,
                SchoolIdentity = new SchoolIdentityType
                {
                    SchoolId = 1
                },
                SchoolLookup = null
            };

            schoolReference.ReferencesSameSchoolAs(otherSchoolReference).ShouldBeTrue();
        }

        [Test]
        public void ReferencesSameSchoolShouldBeFalseIfIdentityDoNotMatch()
        {
            var schoolReference = new SchoolReferenceType
            {
                @ref = null,
                SchoolIdentity = new SchoolIdentityType
                {
                    SchoolId = 1
                },
                SchoolLookup = null
            };

            var otherSchoolReference = new SchoolReferenceType
            {
                @ref = null,
                SchoolIdentity = new SchoolIdentityType
                {
                    SchoolId = 2
                },
                SchoolLookup = null
            };

            schoolReference.ReferencesSameSchoolAs(otherSchoolReference).ShouldBeFalse();
        }

        [Test]
        public void ReferencesSameSchoolShouldBeTrueIfLookupMatches()
        {
            var schoolReference = new SchoolReferenceType
            {
                @ref = null,
                SchoolIdentity = null,
                SchoolLookup = new SchoolLookupType
                {
                    SchoolId = 1
                }
            };

            var otherSchoolReference = new SchoolReferenceType
            {
                @ref = null,
                SchoolIdentity = null,
                SchoolLookup = new SchoolLookupType
                {
                    SchoolId = 1
                }
            };

            schoolReference.ReferencesSameSchoolAs(otherSchoolReference).ShouldBeTrue();
        }

        [Test]
        public void ReferencesSameSchoolShouldBeFalseIfLookupDoNotMatch()
        {
            var schoolReference = new SchoolReferenceType
            {
                @ref = null,
                SchoolIdentity = null,
                SchoolLookup = new SchoolLookupType
                {
                    SchoolId = 1
                }
            };

            var otherSchoolReference = new SchoolReferenceType
            {
                @ref = null,
                SchoolIdentity = null,
                SchoolLookup = new SchoolLookupType
                {
                    SchoolId = 2
                }
            };

            schoolReference.ReferencesSameSchoolAs(otherSchoolReference).ShouldBeFalse();
        }

        [Test]
        public void ReferencesSameSchoolShouldBeTrueIfReferencesMatch()
        {
            var schoolReference = new SchoolReferenceType
            {
                @ref = "Test",
                SchoolIdentity = null,
                SchoolLookup = null
            };
            var otherSchoolReference = new SchoolReferenceType
            {
                @ref = "Test",
                SchoolIdentity = null,
                SchoolLookup = null
            };

            schoolReference.ReferencesSameSchoolAs(otherSchoolReference).ShouldBeTrue();
        }

        [Test]
        public void ReferencesSameSchoolShouldBeFalseIfReferencesDoNotMatch()
        {
            var schoolReference = new SchoolReferenceType
            {
                @ref = "Foo",
                SchoolIdentity = null,
                SchoolLookup = null
            };
            var otherSchoolReference = new SchoolReferenceType
            {
                @ref = "Bar",
                SchoolIdentity = null,
                SchoolLookup = null
            };

            schoolReference.ReferencesSameSchoolAs(otherSchoolReference).ShouldBeFalse();
        }

        [Test]
        public void ReferencesSameSchoolShouldBeFalseIfNull()
        {
            SchoolReferenceType schoolReference = null;
            SchoolReferenceType otherSchoolReference = null;

            schoolReference.ReferencesSameSchoolAs(otherSchoolReference).ShouldBeFalse();
        }

        [Test]
        public void ReferencesSameSectionShouldBeTrueIfIdentityMatches()
        {
            var sectionReference = new SectionReferenceType
            {
                @ref = null,
                SectionIdentity = new SectionIdentityType
                {
                    SectionIdentifier = "Code_One"
                },
                SectionLookup = null
            };

            var otherSectionReference = new SectionReferenceType
            {
                @ref = null,
                SectionIdentity = new SectionIdentityType
                {
                    SectionIdentifier = "Code_One"
                },
                SectionLookup = null
            };

            sectionReference.ReferencesSameSectionAs(otherSectionReference).ShouldBeTrue();
        }

        [Test]
        public void ReferencesSameSectionShouldBeFalseIfIdentityDoNotMatch()
        {
            var sectionReference = new SectionReferenceType
            {
                @ref = null,
                SectionIdentity = new SectionIdentityType
                {
                    SectionIdentifier = "Code_One"
                },
                SectionLookup = null
            };

            var otherSectionReference = new SectionReferenceType
            {
                @ref = null,
                SectionIdentity = new SectionIdentityType
                {
                    SectionIdentifier = "Code_Two"
                },
                SectionLookup = null
            };

            sectionReference.ReferencesSameSectionAs(otherSectionReference).ShouldBeFalse();
        }

        [Test]
        public void ReferencesSameSectionShouldBeTrueIfLookupMatches()
        {
            var sectionReference = new SectionReferenceType
            {
                @ref = null,
                SectionIdentity = null,
                SectionLookup = new SectionLookupType
                {
                    SectionIdentifier = "Code_One"
                }
            };

            var otherSectionReference = new SectionReferenceType
            {
                @ref = null,
                SectionIdentity = null,
                SectionLookup = new SectionLookupType
                {
                    SectionIdentifier = "Code_One"
                }
            };

            sectionReference.ReferencesSameSectionAs(otherSectionReference).ShouldBeTrue();
        }

        [Test]
        public void ReferencesSameSectionShouldBeFalseIfLookupDoNotMatch()
        {
            var sectionReference = new SectionReferenceType
            {
                @ref = null,
                SectionIdentity = null,
                SectionLookup = new SectionLookupType
                {
                    SectionIdentifier = "Code_One"
                }
            };

            var otherSectionReference = new SectionReferenceType
            {
                @ref = null,
                SectionIdentity = null,
                SectionLookup = new SectionLookupType
                {
                    SectionIdentifier = "Code_Two"
                }
            };

            sectionReference.ReferencesSameSectionAs(otherSectionReference).ShouldBeFalse();
        }

        [Test]
        public void ReferencesSameSectionShouldBeTrueIfReferencesMatch()
        {
            var sectionReference = new SectionReferenceType
            {
                @ref = "Test",
                SectionIdentity = null,
                SectionLookup = null
            };
            var otherSectionReference = new SectionReferenceType
            {
                @ref = "Test",
                SectionIdentity = null,
                SectionLookup = null
            };

            sectionReference.ReferencesSameSectionAs(otherSectionReference).ShouldBeTrue();
        }

        [Test]
        public void ReferencesSameSectionShouldBeFalseIfReferencesDoNotMatch()
        {
            var sectionReference = new SectionReferenceType
            {
                @ref = "Foo",
                SectionIdentity = null,
                SectionLookup = null
            };
            var otherSectionReference = new SectionReferenceType
            {
                @ref = "Bar",
                SectionIdentity = null,
                SectionLookup = null
            };

            sectionReference.ReferencesSameSectionAs(otherSectionReference).ShouldBeFalse();
        }

        [Test]
        public void ReferencesSameSectionShouldBeFalseIfNull()
        {
            SectionReferenceType sectionReference = null;
            SectionReferenceType otherSectionReference = null;

            sectionReference.ReferencesSameSectionAs(otherSectionReference).ShouldBeFalse();
        }
    }
}
