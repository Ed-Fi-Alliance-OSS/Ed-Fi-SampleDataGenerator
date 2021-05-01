using System.Collections.Generic;
using System.Linq;
using EdFi.SampleDataGenerator.Core.Entities;

namespace EdFi.SampleDataGenerator.Core.Helpers
{
    public static class StudentAcademicRecordHelpers
    {
        public static string GenerateStudentAcademicRecordId(Student student, SchoolYearType schoolYear, string term)
        {
            var sessionId = SessionHelpers.GenerateSessionId(schoolYear, term);
            return $"SAR_{sessionId}_{student.StudentUniqueId}";
        }

        public static StudentAcademicRecordReferenceType GetReference(string studentAcademicRecordId)
        {
            return new StudentAcademicRecordReferenceType { @ref = studentAcademicRecordId };
        }

        public static decimal GetTotalCreditsEarned(this List<StudentAcademicRecord> records)
        {
            return records.Select(x => x.SessionEarnedCredits).Sum(x => x.Credits1);
        }
    }
}
