using System;
using System.Collections.Generic;
using System.Linq;
using EdFi.SampleDataGenerator.Core.Date;
using EdFi.SampleDataGenerator.Core.Entities;
using EdFi.SampleDataGenerator.Core.Helpers;

namespace EdFi.SampleDataGenerator.Core.Serialization.Output
{
    public class StudentTranscriptSession
    {
        public Session Session { get; set; }
        public SchoolYearType SchoolYear { get; set; }
        public string Term { get; set; }
        public string StudentAcademicRecordId { get; set; }
        public GradeLevelDescriptor GradeLevel { get; set; }
        public List<StudentCourse> StudentCourses { get; set; } = new List<StudentCourse>();
        public bool CurrentSchoolYear { get; set; }
    }

    public class StudentCourse
    {
        public Course Course { get; set; }
        public int CourseGradeAverage { get; set; }
    }

    public static class StudentTranscriptSessionHelpers
    {
        public static IEnumerable<StudentTranscriptSession> CompletedInDateRange(this IEnumerable<StudentTranscriptSession> studentTranscriptSessions, DateRange dateRange)
        {
            return studentTranscriptSessions.Where(sts => sts.Session != null && dateRange.Contains(sts.Session.EndDate));
        }

        public static IEnumerable<StudentTranscriptSession> ForCurrentSchoolYear(this IEnumerable<StudentTranscriptSession> studentTranscriptSessions)
        {
            return studentTranscriptSessions.Where(sts => sts.CurrentSchoolYear);
        }

        public static IEnumerable<StudentTranscriptSession> ForPreviousSchoolYears(this IEnumerable<StudentTranscriptSession> studentTranscriptSessions)
        {
            return studentTranscriptSessions.Where(sts => !sts.CurrentSchoolYear);
        }
    }
}