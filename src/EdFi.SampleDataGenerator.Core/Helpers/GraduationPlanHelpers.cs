using System;
using System.Collections.Generic;
using System.Linq;
using EdFi.SampleDataGenerator.Core.Config;
using EdFi.SampleDataGenerator.Core.Entities;

namespace EdFi.SampleDataGenerator.Core.Helpers
{
    public static class GraduationPlanHelpers
    {
        public static IEnumerable<IGraduationPlanTemplate> GetGraduationPlanTemplates(this IEnumerable<IGraduationPlanTemplateReference> templateReferences, IEnumerable<IGraduationPlanTemplate> templates)
        {
            return templates.Where(t => templateReferences.Any(tr => tr.Name.Equals(t.Name, StringComparison.OrdinalIgnoreCase)));
        }

        public static GraduationPlan GetGraduationPlan(this IGraduationPlanTemplate template, ISchoolProfile school, SchoolYearType planYear)
        {
            return new GraduationPlan
            {
                id = template.GetGraduationPlanId(school, planYear),
                GraduationPlanType = template.GetGraduationPlanTypeDescriptor().GetStructuredCodeValue(),
                EducationOrganizationReference = school.GetEducationOrganizationReference(),
                GraduationSchoolYear = planYear,
                TotalRequiredCredits = new Credits
                {
                    Credits1 = template.TotalCreditsRequired
                }
            };
        }

        public static GraduationPlan GetGraduationPlan(this IEnumerable<GraduationPlan> graduationPlans, GraduationPlanReferenceType graduationPlanReference)
        {
            return graduationPlans.SingleOrDefault(gp => graduationPlanReference.ReferencesGraduationPlan(gp));
        }

        public static IEnumerable<GraduationPlan> GetGraduationPlans(this IEnumerable<GraduationPlan> graduationPlans, ISchoolProfile school, SchoolYearType planYear)
        {
            return graduationPlans.Where(gp => gp.GraduationSchoolYear == planYear && gp.EducationOrganizationReference?.EducationOrganizationIdentity?.EducationOrganizationId == school.SchoolId);
        }

        public static string GetGraduationPlanId(this IEnumerable<GraduationPlan> graduationPlans, ISchoolProfile school, SchoolYearType planYear, GraduationPlanTypeDescriptor graduationPlanType)
        {
            return graduationPlans.GetGraduationPlans(school, planYear).SingleOrDefault(gp => gp.GraduationPlanType.Equals(graduationPlanType.CodeValue, StringComparison.OrdinalIgnoreCase))?.id;
        }

        public static string GetGraduationPlanId(this IGraduationPlanTemplate template, ISchoolProfile school, SchoolYearType planYear)
        {
            return $"GPLN_{template.GetGraduationPlanTypeDescriptor().CodeValue}_{school.SchoolName.LettersOnly()}_{planYear.ToCodeValue()}";
        }
    }
}
