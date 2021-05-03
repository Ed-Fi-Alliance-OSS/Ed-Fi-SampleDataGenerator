using System;
using System.Collections.Generic;
using System.Linq;
using EdFi.SampleDataGenerator.Core.Config;
using EdFi.SampleDataGenerator.Core.Entities;

namespace EdFi.SampleDataGenerator.Core.Helpers
{
    public static class EdFiReferenceTypeHelpers
    {
        public static AssessmentReferenceType GetAssessmentReference(this Assessment assessment)
        {
            return new AssessmentReferenceType
            {
                AssessmentIdentity = new AssessmentIdentityType
                {
                    AssessmentIdentifier = assessment.AssessmentIdentifier,
                    Namespace = assessment.Namespace
                }
            };
        }

        public static EducationOrganizationReferenceType GetEducationOrganizationReference(int edOrgId)
        {
            return new EducationOrganizationReferenceType
            {
                EducationOrganizationIdentity = new EducationOrganizationIdentityType { EducationOrganizationId = edOrgId },
                EducationOrganizationLookup = new EducationOrganizationLookupType { EducationOrganizationId = edOrgId }
            };
        }

        public static EducationOrganizationReferenceType GetEducationOrganizationReference(this ISchoolProfile schoolProfile)
        {
            return GetEducationOrganizationReference(schoolProfile.SchoolId);
        }

        public static EducationOrganizationReferenceType GetEducationOrganizationReference(this LocalEducationAgency loaclLocalEducationAgency)
        {
            return GetEducationOrganizationReference(loaclLocalEducationAgency.LocalEducationAgencyId);
        }

        public static LocalEducationAgencyReferenceType GetLocalEducationOrganizationAgencyReference(this LocalEducationAgency localEducationAgency)
        {
            return new LocalEducationAgencyReferenceType
            {
                LocalEducationAgencyIdentity = new LocalEducationAgencyIdentityType { LocalEducationAgencyId = localEducationAgency.LocalEducationAgencyId },
                LocalEducationAgencyLookup = new LocalEducationAgencyLookupType { LocalEducationAgencyId = localEducationAgency.LocalEducationAgencyId }
            };
        }

        public static LocalEducationAgency GetLocalEducationAgency(this ISchoolProfile schoolProfile, IEnumerable<School> schools, IEnumerable<LocalEducationAgency> localEducationAgencies)
        {
            var localEducationAgencyReference = schools.First(x => x.GetSchoolReference().ReferencesSchool(schoolProfile)).LocalEducationAgencyReference;

            var localEducationAgencyId =
                localEducationAgencyReference.LocalEducationAgencyIdentity?.LocalEducationAgencyId ??
                localEducationAgencyReference.LocalEducationAgencyLookup?.LocalEducationAgencyId;

            return localEducationAgencies.First(x => x.LocalEducationAgencyId == localEducationAgencyId || localEducationAgencyReference.References(x.id));
        }

        public static int GetNumericClassPeriod(this ClassPeriodReferenceType classPeriodReference)
        {
            return ClassPeriodHelpers.GetNumericClassPeriod(classPeriodReference.ClassPeriodIdentity.ClassPeriodName);
        }

        public static ParentReferenceType GetParentReference(this Parent parent)
        {
            return new ParentReferenceType
            {
                ParentIdentity = new ParentIdentityType { ParentUniqueId = parent.ParentUniqueId },
                ParentLookup = new ParentLookupType { ParentUniqueId = parent.ParentUniqueId }
            };
        }

        public static StaffReferenceType GetStaffReference(this Staff staff)
        {
            return new StaffReferenceType
            {
                StaffIdentity = new StaffIdentityType
                {
                    StaffUniqueId = staff.StaffUniqueId
                }
            };
        }

        public static StudentReferenceType GetStudentReference(this Student student)
        {
            return new StudentReferenceType
            {
                StudentIdentity = new StudentIdentityType { StudentUniqueId = student.StudentUniqueId },
                StudentLookup = new StudentLookupType
                {
                    StudentUniqueId = student.StudentUniqueId,
                    BirthData = student.BirthData,
                    Name = student.Name
                }
            };
        }

        public static SchoolReferenceType GetSchoolReference(this ISchoolProfile schoolProfile)
        {
            return new SchoolReferenceType
            {
                SchoolIdentity = new SchoolIdentityType {SchoolId = schoolProfile.SchoolId},
                SchoolLookup = new SchoolLookupType { SchoolId = schoolProfile.SchoolId }
            };
        }

        public static SchoolReferenceType GetSchoolReference(this School school)
        {
            return new SchoolReferenceType
            {
                SchoolIdentity = new SchoolIdentityType { SchoolId = school.SchoolId },
                SchoolLookup = new SchoolLookupType { SchoolId = school.SchoolId }
            };
        }

        public static SectionReferenceType GetSectionReference(this Section section)
        {
            return new SectionReferenceType
            {
                SectionIdentity = new SectionIdentityType
                {
                    SectionIdentifier = section.SectionIdentifier,
                    CourseOfferingReference = section.CourseOfferingReference
                }
            };
        }

        public static StudentDisciplineIncidentAssociationReferenceType GetStudentDisciplineIncidentAssociationReference(this DisciplineIncident disciplineIncident)
        {
            return new StudentDisciplineIncidentAssociationReferenceType
            {
                StudentDisciplineIncidentAssociationIdentity = new StudentDisciplineIncidentAssociationIdentityType
                {
                    DisciplineIncidentReference = new DisciplineIncidentReferenceType
                    {
                        DisciplineIncidentIdentity = new DisciplineIncidentIdentityType
                        {
                            IncidentIdentifier = disciplineIncident.IncidentIdentifier,
                            SchoolReference = disciplineIncident.SchoolReference
                        }
                    }
                }
            };
        }

        public static DisciplineIncidentReferenceType GetDisciplineIncidentReference(this DisciplineIncident disciplineIncident)
        {
            return new DisciplineIncidentReferenceType
            {
                DisciplineIncidentIdentity = new DisciplineIncidentIdentityType
                {
                    IncidentIdentifier = disciplineIncident.IncidentIdentifier,
                    SchoolReference = disciplineIncident.SchoolReference
                }
            };
        }

        public static ProgramReferenceType GetProgramReference(this Program program)
        {
            return new ProgramReferenceType
            {
                ProgramIdentity = new ProgramIdentityType
                {
                    ProgramName = program.ProgramName,
                    ProgramType = program.ProgramType,
                    EducationOrganizationReference = program.EducationOrganizationReference
                },
                ProgramLookup = new ProgramLookupType
                {
                    ProgramId = program.ProgramId,
                    ProgramName = program.ProgramName,
                    ProgramType = program.ProgramType,
                    EducationOrganizationReference = program.EducationOrganizationReference
                }
            };
        }

        public static bool References(this ReferenceType reference, string id)
        {
            return reference?.@ref != null && reference.@ref.Equals(id, StringComparison.OrdinalIgnoreCase);
        }

        public static bool ReferencesClassPeriod(this ClassPeriodReferenceType classPeriodReference, ClassPeriod classPeriod)
        {
            return classPeriodReference != null &&
                   classPeriod != null &&
                   classPeriodReference.ClassPeriodIdentity != null &&
                   classPeriodReference.ClassPeriodIdentity.ClassPeriodName == classPeriod.ClassPeriodName &&
                   classPeriodReference.ClassPeriodIdentity.SchoolReference.ReferencesSameSchoolAs(classPeriod.SchoolReference);
        }

        public static bool ReferencesCourse(this CourseReferenceType courseReference, Course course)
        {
            return courseReference != null &&
                   course != null &&
                   courseReference.CourseIdentity != null &&
                   courseReference.CourseIdentity.CourseCode == course.CourseCode &&
                   courseReference.CourseIdentity.EducationOrganizationReference.ReferencesSameEducationOrganizationAs(course.EducationOrganizationReference);
        }

        public static bool ReferencesCourseOffering(this CourseOfferingReferenceType courseOfferingReference, CourseOffering courseOffering)
        {
            return courseOfferingReference != null &&
                   courseOffering != null &&
                   courseOfferingReference.CourseOfferingIdentity != null &&
                   courseOfferingReference.CourseOfferingIdentity.LocalCourseCode == courseOffering.LocalCourseCode &&
                   courseOfferingReference.CourseOfferingIdentity.SchoolReference.ReferencesSameSchoolAs(courseOffering.SchoolReference) &&
                   courseOfferingReference.CourseOfferingIdentity.SessionReference.ReferencesSameSessionAs(courseOffering.SessionReference);
        }

        public static bool ReferencesGradingPeriod(this GradingPeriodReferenceType gradingPeriodReference, GradingPeriod gradingPeriod)
        {
            return gradingPeriodReference != null &&
                   gradingPeriod != null &&
                   gradingPeriodReference.GradingPeriodIdentity != null &&
                   gradingPeriodReference.GradingPeriodIdentity.SchoolYear == gradingPeriod.SchoolYear &&
                   gradingPeriodReference.GradingPeriodIdentity.PeriodSequence == gradingPeriod.PeriodSequence &&
                   gradingPeriodReference.GradingPeriodIdentity.SchoolReference.ReferencesSameSchoolAs(gradingPeriod.SchoolReference) &&
                   gradingPeriodReference.GradingPeriodIdentity.GradingPeriod == gradingPeriod.GradingPeriod1;
        }

        public static bool ReferencesSession(this CourseOfferingReferenceType courseOfferingReference, Session session)
        {
            return courseOfferingReference != null &&
                   session != null &&
                   courseOfferingReference.CourseOfferingIdentity != null &&
                   courseOfferingReference.CourseOfferingIdentity.SessionReference.ReferencesSession(session);
        }

        public static bool ReferencesStudentSectionAssociation(
            this StudentSectionAssociationReferenceType studentSectionAssociationReferenceType,
            StudentSectionAssociation studentSectionAssociation)
        {
            return studentSectionAssociationReferenceType != null &&
                studentSectionAssociation != null &&
                studentSectionAssociationReferenceType.StudentSectionAssociationIdentity != null &&
                studentSectionAssociationReferenceType.StudentSectionAssociationIdentity.SectionReference.ReferencesSameSectionAs(studentSectionAssociation.SectionReference) &&
                studentSectionAssociationReferenceType.StudentSectionAssociationIdentity.BeginDate == studentSectionAssociation.BeginDate &&
                studentSectionAssociationReferenceType.StudentSectionAssociationIdentity.StudentReference.ReferencesSameStudentAs(studentSectionAssociation.StudentReference);
        }

        public static bool ReferencesGraduationPlan(this GraduationPlanReferenceType graduationPlanReference, GraduationPlan graduationPlan)
        {
            return 
                graduationPlanReference != null &&
                graduationPlan != null &&
                (
                    graduationPlanReference.References(graduationPlan.id) ||
                    (
                        graduationPlanReference.GraduationPlanIdentity?.EducationOrganizationReference != null && 
                        graduationPlanReference.GraduationPlanIdentity.EducationOrganizationReference.ReferencesSameEducationOrganizationAs(graduationPlan.EducationOrganizationReference) && 
                        graduationPlanReference.GraduationPlanIdentity.GraduationPlanType == graduationPlan.GraduationPlanType && 
                        graduationPlanReference.GraduationPlanIdentity.GraduationSchoolYear == graduationPlan.GraduationSchoolYear
                    )
                );
        }

        public static bool ReferencesSameSchoolAs(this EducationOrganizationReferenceType educationOrganizationReference, SchoolReferenceType schoolReferenceType)
        {
            var edOrgId = educationOrganizationReference?.EducationOrganizationIdentity?.EducationOrganizationId ??
                          educationOrganizationReference?.EducationOrganizationLookup?.EducationOrganizationId;

            return edOrgId.HasValue && schoolReferenceType.ReferencesSchool(edOrgId.Value);
        }

        public static bool ReferencesSchool(this EducationOrganizationReferenceType educationOrganizationReference, int schoolId)
        {
            var edOrgId = educationOrganizationReference?.EducationOrganizationIdentity?.EducationOrganizationId ??
                          educationOrganizationReference?.EducationOrganizationLookup?.EducationOrganizationId;

            return edOrgId.HasValue && edOrgId == schoolId;
        }

        public static bool ReferencesSchool(this SchoolReferenceType schoolReferenceType, ISchoolProfile schoolProfile)
        {
            return schoolReferenceType.ReferencesSchool(schoolProfile.SchoolId) ||
                   schoolReferenceType.References(schoolProfile.GetSchoolEntityId());
        }

        public static bool ReferencesSchool(this SchoolReferenceType schoolReferenceType, int schoolId)
        {
            return schoolReferenceType?.SchoolIdentity?.SchoolId == schoolId ||
                   schoolReferenceType?.SchoolLookup?.SchoolId == schoolId;
        }

        public static bool ReferencesSameSchoolAs(this SchoolReferenceType reference1, SchoolReferenceType reference2)
        {
            return reference1 != null &&
                   reference2 != null &&
                   (
                       reference1.ReferencesSameEntityAs(reference2) ||
                       (
                           (reference1.SchoolIdentity != null && reference2.SchoolIdentity != null && reference1.SchoolIdentity.SchoolId == reference2.SchoolIdentity.SchoolId) ||
                           (reference1.SchoolLookup != null && reference2.SchoolLookup != null && reference1.SchoolLookup.SchoolId == reference2.SchoolLookup.SchoolId)
                       )
                   );
        }

        public static bool ReferencesSameEducationOrganizationAs(this EducationOrganizationReferenceType reference1, EducationOrganizationReferenceType reference2)
        {
            return reference1 != null &&
                reference2 != null &&
                (
                    reference1.ReferencesSameEntityAs(reference2) ||
                    (
                        (reference1.EducationOrganizationIdentity != null && reference2.EducationOrganizationIdentity != null && reference1.EducationOrganizationIdentity.EducationOrganizationId == reference2.EducationOrganizationIdentity.EducationOrganizationId) ||
                        (reference1.EducationOrganizationLookup != null && reference2.EducationOrganizationLookup != null && reference1.EducationOrganizationLookup.EducationOrganizationId == reference2.EducationOrganizationLookup.EducationOrganizationId)
                    )
                );
        }

        public static bool ReferencesSameEntityAs(this ReferenceType reference1, ReferenceType reference2)
        {
            return reference1?.@ref != null &&
                   reference2?.@ref != null &&
                   reference1.@ref.Equals(reference2.@ref, StringComparison.OrdinalIgnoreCase);
        }
        
        public static bool ReferencesSameSectionAs(this SectionReferenceType reference1, SectionReferenceType reference2)
        {
            return reference1 != null &&
                   reference2 != null &&
                   (
                       reference1.ReferencesSameEntityAs(reference2) ||
                       (
                           (reference1.SectionIdentity != null && reference2.SectionIdentity != null && reference1.SectionIdentity.SectionIdentifier == reference2.SectionIdentity.SectionIdentifier) ||
                           (reference1.SectionLookup != null && reference2.SectionLookup != null && reference1.SectionLookup.SectionIdentifier == reference2.SectionLookup.SectionIdentifier)
                       )
                   );
        }

        public static bool ReferencesSameStudentAs(this StudentReferenceType reference1, StudentReferenceType reference2)
        {
            return reference1 != null &&
                reference2 != null &&
                (
                    reference1.ReferencesSameEntityAs(reference2) ||
                    (
                        (reference1.StudentIdentity != null && reference2.StudentIdentity != null && reference1.StudentIdentity.StudentUniqueId == reference2.StudentIdentity.StudentUniqueId) ||
                        (reference1.StudentLookup != null && reference2.StudentLookup != null && reference1.StudentLookup.StudentUniqueId == reference2.StudentLookup.StudentUniqueId)
                    )
                );
        }

        public static bool ReferencesSameProgramAs(this ProgramReferenceType reference1, ProgramReferenceType reference2)
        {
            return reference1 != null &&
                   reference2 != null &&
                   (
                       reference1.ReferencesSameEntityAs(reference2) ||
                       (
                           (reference1.ProgramIdentity != null && reference2.ProgramIdentity != null &&
                           reference1.ProgramIdentity.EducationOrganizationReference.ReferencesSameEducationOrganizationAs(reference2.ProgramIdentity.EducationOrganizationReference) &&
                           reference1.ProgramIdentity.ProgramType == reference2.ProgramIdentity.ProgramType &&
                           reference1.ProgramIdentity.ProgramName == reference2.ProgramIdentity.ProgramName) ||
                           (reference1.ProgramLookup != null && reference2.ProgramLookup != null &&
                           reference1.ProgramLookup.ProgramId == reference2.ProgramLookup.ProgramId &&
                           reference1.ProgramLookup.EducationOrganizationReference.ReferencesSameEducationOrganizationAs(reference2.ProgramLookup.EducationOrganizationReference) &&
                           reference1.ProgramLookup.ProgramType == reference2.ProgramLookup.ProgramType &&
                           reference1.ProgramLookup.ProgramName == reference2.ProgramLookup.ProgramName)
                       )
                   );
        }

        public static bool ReferencesSection(this SectionReferenceType sectionReference, Section section)
        {
            return sectionReference != null &&
                   section != null &&
                   (
                       sectionReference.SectionIdentity?.SectionIdentifier == section.SectionIdentifier ||
                       sectionReference.SectionLookup?.SectionIdentifier == section.SectionIdentifier
                    );
        }
    }
}
