using System;
using System.Collections.Generic;
using System.Linq;
using EdFi.SampleDataGenerator.Core.Config;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common.Dependencies;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common.Entity;
using EdFi.SampleDataGenerator.Core.DataGeneration.InterchangeEntities;
using EdFi.SampleDataGenerator.Core.Entities;
using EdFi.SampleDataGenerator.Core.Helpers;
using EdFi.SampleDataGenerator.Core.Serialization.Output;

namespace EdFi.SampleDataGenerator.Core.DataGeneration.Generators.StudentTranscript
{
    public class StudentTranscriptSessionGenerator : StudentTranscriptEntityGenerator
    {
        public StudentTranscriptSessionGenerator(IRandomNumberGenerator randomNumberGenerator) : base(randomNumberGenerator)
        {
        }

        public override IEntity GeneratesEntity => StudentTranscriptEntity.StudentTranscriptSession;
        public override IEntity[] DependsOnEntities => EntityDependencies.Create(StudentEnrollmentEntity.StudentSectionAssociation);

        protected override void GenerateCore(StudentDataGeneratorContext context)
        {
            var result = GetSessionsThatNeedAcademicRecordsGenerated(context);
            context.GeneratedStudentData.StudentTranscriptData.StudentTranscriptSessions.AddRange(result);
        }

        private IEnumerable<StudentTranscriptSession> GetSessionsThatNeedAcademicRecordsGenerated(StudentDataGeneratorContext context)
        {
            const double acceleratedMathProbability = .1;

            bool takesMathAheadOfGradeLevel =
                context.StudentPerformanceProfile.IsStudentHighPerforming(Configuration) &&
                RandomNumberGenerator.GetRandomBool(acceleratedMathProbability);

            var previousSchoolYearSessions = GenerateSessionsForPreviousSchoolYears(context, takesMathAheadOfGradeLevel);
            var currentSchoolYearSessions = GenerateSessionsForCurrentSchoolYear(context, takesMathAheadOfGradeLevel);

            return previousSchoolYearSessions.SafeConcat(currentSchoolYearSessions);
        }

        private IEnumerable<StudentTranscriptSession> GenerateSessionsForPreviousSchoolYears(StudentDataGeneratorContext context, bool takesMathAheadOfGradeLevel)
        {
            var student = context.Student;

            var schoolProfile = Configuration.SchoolProfile;
            var currentSchoolYear = Configuration.GetCurrentSchoolYear();
            var graduationYear = Configuration.GradeProfile.GetGraduationYear(schoolProfile, currentSchoolYear);

            var gradeLevelsPriorToCurrentGrade = Configuration.SchoolProfile.GradeProfiles
                .Where(gp => gp.GetGraduationYear(schoolProfile, currentSchoolYear) > graduationYear)
                .ToList();

            var sessions = Configuration.EducationOrgCalendarData.Sessions.ForSchool(Configuration.SchoolProfile)
                    .OrderBy(s => s.EndDate)
                    .ToList();

            for (var i = 0; i < gradeLevelsPriorToCurrentGrade.Count; ++i)
            {
                var schoolYearOffset = -(gradeLevelsPriorToCurrentGrade.Count - i);
                var schoolYear = currentSchoolYear.AddYears(schoolYearOffset);
                var gradeLevel = gradeLevelsPriorToCurrentGrade[i].GetGradeLevel();

                var studentCoursesForSchoolYear = GetStudentCoursesForGradeLevel(context, gradeLevel, takesMathAheadOfGradeLevel, currentYear: false);
                var studentCoursesBySession = GetStudentCoursesBySessionName(studentCoursesForSchoolYear, sessions);

                foreach (var session in sessions)
                {
                    yield return new StudentTranscriptSession
                    {
                        Session = null, //we don't actually have a valid Session object for years prior to the current school year
                        SchoolYear = schoolYear,
                        Term = session.Term,
                        StudentAcademicRecordId = StudentAcademicRecordHelpers.GenerateStudentAcademicRecordId(student, schoolYear, session.Term),
                        GradeLevel = gradeLevel,
                        StudentCourses = studentCoursesBySession[session.SessionName],
                        CurrentSchoolYear = false,
                    };
                }
            }
        }

        private IEnumerable<StudentTranscriptSession> GenerateSessionsForCurrentSchoolYear(StudentDataGeneratorContext context, bool takesMathAheadOfGradeLevel)
        {
            var student = context.Student;
            var currentSchoolYear = Configuration.GetCurrentSchoolYear();

            var gradeLevel = Configuration.GradeProfile.GetGradeLevel();

            var sessions = Configuration.EducationOrgCalendarData.Sessions.ForSchool(Configuration.SchoolProfile)
                    .OrderBy(s => s.EndDate)
                    .ToList();

            var studentCoursesForSchoolYear = GetStudentCoursesForGradeLevel(context, gradeLevel, takesMathAheadOfGradeLevel, currentYear: true);
            var studentCoursesBySession = GetStudentCoursesBySessionName(studentCoursesForSchoolYear, sessions);

            return sessions.Select(session => 
                new StudentTranscriptSession
                {
                    Session = session,
                    SchoolYear = currentSchoolYear,
                    Term = session.Term,
                    StudentAcademicRecordId = StudentAcademicRecordHelpers.GenerateStudentAcademicRecordId(student, currentSchoolYear, session.Term),
                    GradeLevel = gradeLevel,
                    StudentCourses = studentCoursesBySession[session.SessionName],
                    CurrentSchoolYear = true,
                });
        }

        private List<Course> GetCoursesForGradeLevel(StudentPerformanceProfile studentPerformanceProfile, GradeLevelDescriptor gradeLevel, bool takesMathAheadOfGradeLevel)
        {
            var schoolId = Configuration.SchoolProfile.SchoolId;
            var classPeriods = Configuration.GlobalData.EducationOrganizationData.ClassPeriods
                .Where(cp => cp.SchoolReference.ReferencesSchool(Configuration.SchoolProfile))
                .ToList();

            var availableCoursesBySubject = Configuration.GlobalData.EducationOrganizationData.Courses
                .Where(c => c.EducationOrganizationReference.ReferencesSchool(schoolId))
                .Where(c => CourseIsApplicableForGradeLevel(c, gradeLevel, takesMathAheadOfGradeLevel))
                .Where(c => StudentConsidersTakingCourse(studentPerformanceProfile, c))
                .GroupBy(c => c.AcademicSubject);

            var randomlySelectedCourses = availableCoursesBySubject.GetNRandomItems(RandomNumberGenerator, classPeriods.Count);
            return randomlySelectedCourses;
        }

        private static bool CourseIsApplicableForGradeLevel(Course course, GradeLevelDescriptor gradeLevel, bool takesMathAheadOfGradeLevel)
        {
            const int minimumGradeLevelForAcceleratedMath = 7;

            var gradeLevelIsAppropriateForAcceleratedMath = gradeLevel.GetNumericGradeLevel() >= minimumGradeLevelForAcceleratedMath;

            if (takesMathAheadOfGradeLevel && course.AcademicSubject.Equals(AcademicSubjectDescriptor.Mathematics.GetStructuredCodeValue(), StringComparison.OrdinalIgnoreCase) && gradeLevelIsAppropriateForAcceleratedMath)
            {
                GradeLevelDescriptor nextGradeLevel;
                if (gradeLevel.TryGetNextK12GradeLevel(out nextGradeLevel))
                {
                    return CourseNormallyAppliesTo(course, nextGradeLevel);
                }

                //The student has exhausted Mathematics offerings K-12.
                return false;
            }

            return CourseNormallyAppliesTo(course, gradeLevel);
        }

        private static bool CourseNormallyAppliesTo(Course course, GradeLevelDescriptor gradeLevel)
        {
            return course.OfferedGradeLevel.Contains(gradeLevel.GetStructuredCodeValue());
        }

        private bool StudentConsidersTakingCourse(StudentPerformanceProfile studentPerformanceProfile, Course course)
        {
            if (course.RestrictedToHighPerformingStudents())
            {
                bool courseIsForGiftedAndTalentedProgram =
                    course.CourseLevelCharacteristic.Contains(CourseLevelCharacteristicDescriptor.GiftedAndTalented.GetStructuredCodeValue());

                if (courseIsForGiftedAndTalentedProgram)
                {
                    return studentPerformanceProfile.IsEligibleForGiftedAndTalentedProgram(Configuration) &&
                           Configuration.GetProgram(ProgramTypeDescriptor.GiftedAndTalented) != null;
                }

                return studentPerformanceProfile.IsStudentHighPerforming(Configuration);
            }

            return true; //All students consider taking this course.
        }

        private List<StudentCourse> GetStudentCoursesForGradeLevel(StudentDataGeneratorContext context, GradeLevelDescriptor gradeLevel, bool takesMathAheadOfGradeLevel, bool currentYear)
        {
            var gradePointAverage = Configuration.GlobalConfig.StudentGradeRanges.GetGradePointAverageByPerformanceIndex(context.StudentPerformanceProfile);
            var gradeRange = GradeRange.FullGradingScale;

            if (!currentYear)
            {
                //currently we avoid generating failing grades for previous years due to the complications
                //it will create on assigning students' courses for subsequent years
                if (gradePointAverage < LetterGrade.D.MinNumericGrade)
                {
                    gradePointAverage = RandomNumberGenerator.Generate(LetterGrade.D.MinNumericGrade, LetterGrade.D.MaxNumericGrade + 1);
                    gradeRange = GradeRange.NoFailures;
                }
            }

            var courses = GetCoursesForGradeLevel(context.StudentPerformanceProfile, gradeLevel, takesMathAheadOfGradeLevel);
            var grades = GradeGenerationHelpers.GenerateGradesFromGradePointAverage(gradePointAverage, courses.Count, RandomNumberGenerator, gradeRange).ToList();

            var result = courses.Select((t, i) => 
                new StudentCourse
                {
                    Course = t,
                    CourseGradeAverage = grades[i]
                })
                .ToList();

            return result;
        }

        private Dictionary<string, List<StudentCourse>> GetStudentCoursesBySessionName(List<StudentCourse> studentCoursesForYear, List<Session> sessions)
        {
            var result = sessions.ToDictionary(s => s.SessionName, s => new List<StudentCourse>());

            foreach (var studentCourse in studentCoursesForYear)
            {
                //note that even though we don't allow failing grades for the year,
                //we do allow failing grades per-session as long as the full year
                //average is above failing
                var gradesForCourse = GradeGenerationHelpers.GenerateGradesFromGradePointAverage(studentCourse.CourseGradeAverage, sessions.Count, RandomNumberGenerator, GradeRange.FullGradingScale).ToList();

                for (var i = 0; i < sessions.Count; ++i)
                {
                    var session = sessions[i];
                    result[session.SessionName].Add(new StudentCourse { Course = studentCourse.Course, CourseGradeAverage = gradesForCourse[i]});
                }
            }

            return result;
        }
    }
}
