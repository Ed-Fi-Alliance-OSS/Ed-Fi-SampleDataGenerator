using System.Collections.Generic;
using System.Linq;
using EdFi.SampleDataGenerator.Core.Config;
using EdFi.SampleDataGenerator.Core.Entities;

namespace EdFi.SampleDataGenerator.Core.Helpers
{
    public static class AssessmentsHelpers
    {
        public const string ACTContentStandardTitle = "ACT";
        public const string SATContentStandardTitle = "SAT";
        public const string PSATContentStandardTitle = "PSAT";
        public const string STATEContentStandardTitle = "STATE";

        public static IEnumerable<Assessment> GetAssessmentsForGradeLevel(this IEnumerable<Assessment> assessments,
            IGradeProfile gradeProfile)
        {
            var targetGradeLevel = gradeProfile.GetGradeLevel().GetNumericGradeLevel();

            return
                from assessment in assessments
                let maxGradeLevel = assessment.AssessedGradeLevel.Select(x => x.GetNumericGradeLevel()).Max()
                let minGradeLevel = assessment.AssessedGradeLevel.Select(x => x.GetNumericGradeLevel()).Min()
                where targetGradeLevel >= minGradeLevel && targetGradeLevel <= maxGradeLevel
                select assessment;
        }

        public static bool IsSATVariant(this Assessment assessment)
        {
            return assessment.ContentStandard.Title == SATContentStandardTitle ||
                   assessment.ContentStandard.Title == PSATContentStandardTitle;
        }

        public static bool StudentHasTakenSATVariant(this IEnumerable<Assessment> assessmentsAlreadyTaken)
        {
            return assessmentsAlreadyTaken.Any(a =>
                a.ContentStandard.Title == SATContentStandardTitle ||
                a.ContentStandard.Title == PSATContentStandardTitle
            );
        }
        public static bool IsStateAssessment(this Assessment assessment)
        {
            return assessment.ContentStandard.Title == STATEContentStandardTitle;
        }

        public static bool IsCollegeEntranceExam(this Assessment assessment)
        {
            return assessment.ContentStandard.Title == SATContentStandardTitle ||
                   assessment.ContentStandard.Title == PSATContentStandardTitle ||
                   assessment.ContentStandard.Title == ACTContentStandardTitle;
        }

        public static bool StudentHasTakenAssessment(this IEnumerable<Assessment> assessmentsAlreadyTaken, Assessment assessment)
        {
            return assessmentsAlreadyTaken.Any(a =>
                a.AssessmentTitle.Equals(assessment.AssessmentTitle)
            );
        }
    }
}
