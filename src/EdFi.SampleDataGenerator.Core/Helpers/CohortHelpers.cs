using System.Collections.Generic;
using System.Linq;
using EdFi.SampleDataGenerator.Core.Entities;

namespace EdFi.SampleDataGenerator.Core.Helpers
{
    public static class CohortHelpers
    {
        public static CohortReferenceType GetCohortReference(this Cohort cohort)
        {
            return new CohortReferenceType
            {
                CohortIdentity = new CohortIdentityType
                {
                    CohortIdentifier = cohort.CohortIdentifier,
                    EducationOrganizationReference = cohort.EducationOrganizationReference
                }
            };
        }

        public static ProgramTypeDescriptor[] GetProgramTypesByAcademicSubject(this AcademicSubjectDescriptor academicSubject)
        {
            if (academicSubject == AcademicSubjectDescriptor.English)
            {
                return new[]
                {
                    ProgramTypeDescriptor.EnglishAsASecondLanguageESL,
                    ProgramTypeDescriptor.Bilingual,
                    ProgramTypeDescriptor.BilingualSummer,
                    ProgramTypeDescriptor.IndianEducation,
                    ProgramTypeDescriptor.ForeignExchange
                };
            }
            if (academicSubject == AcademicSubjectDescriptor.Mathematics)
            {
                return new[]
                {
                    ProgramTypeDescriptor.RegularEducation,
                    ProgramTypeDescriptor.RemedialEducation,
                    ProgramTypeDescriptor.AdultContinuingEducation,
                    ProgramTypeDescriptor.TechnicalPreparatory,
                    ProgramTypeDescriptor.TeacherProfessionalDevelopmentMentoring
                };
            }
            return new ProgramTypeDescriptor[] { };
        }

        public static IEnumerable<Cohort> GetCohortsByEducationOrganizationReference(this EducationOrganizationReferenceType educationOrganizationReference, IEnumerable<Cohort> cohorts)
        {
            return
                cohorts.Where(
                    x =>
                        x.EducationOrganizationReference.ReferencesSameEducationOrganizationAs(
                            educationOrganizationReference));
        }
    }
}
