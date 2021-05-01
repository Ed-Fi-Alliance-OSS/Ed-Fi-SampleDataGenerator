using System.Collections.Generic;
using System.Linq;
using EdFi.SampleDataGenerator.Core.Config;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common.Entity;
using EdFi.SampleDataGenerator.Core.DataGeneration.InterchangeEntities;
using EdFi.SampleDataGenerator.Core.Entities;
using EdFi.SampleDataGenerator.Core.Helpers;
using EdFi.SampleDataGenerator.Core.Serialization.Output;

namespace EdFi.SampleDataGenerator.Core.DataGeneration.Generators.StudentTranscript
{
    public class CourseTranscriptEntityGenerator : StudentTranscriptEntityGenerator
    {
        public CourseTranscriptEntityGenerator(IRandomNumberGenerator randomNumberGenerator) : base(randomNumberGenerator)
        {
        }

        public override IEntity GeneratesEntity => StudentTranscriptEntity.CourseTranscript;
        public override IEntity[] DependsOnEntities => new IEntity[] { StudentTranscriptEntity.StudentTranscriptSession };

        public override void GenerateAdditiveData(StudentDataGeneratorContext context, IDataPeriod dataPeriod)
        {
            var transcriptSessionsCompletedInDataPeriod = context.GeneratedStudentData.StudentTranscriptData.StudentTranscriptSessions
                .CompletedInDateRange(dataPeriod.Intersect(context.EnrollmentDateRange));
            var courseTranscripts = GenerateCourseTranscripts(transcriptSessionsCompletedInDataPeriod);

            context.GeneratedStudentData.StudentTranscriptData.CourseTranscripts.AddRange(courseTranscripts);
        }

        protected override void GenerateCore(StudentDataGeneratorContext context)
        {
            var previousYearsStudentTranscriptSessions = context.GeneratedStudentData.StudentTranscriptData.StudentTranscriptSessions.ForPreviousSchoolYears();
            var courseTranscripts = GenerateCourseTranscripts(previousYearsStudentTranscriptSessions);

            context.GeneratedStudentData.StudentTranscriptData.CourseTranscripts.AddRange(courseTranscripts);
        }

        private IEnumerable<CourseTranscript> GenerateCourseTranscripts(IEnumerable<StudentTranscriptSession> studentTranscriptSessions)
        {
            var sessionsPerYear = Configuration.EducationOrgCalendarData.Sessions.ForSchool(Configuration.SchoolProfile).Count();

            foreach (var session in studentTranscriptSessions)
            {
                var currentGradeLevel = session.GradeLevel;

                foreach (var studentCourse in session.StudentCourses)
                {
                    var numericGrade = studentCourse.CourseGradeAverage;
                    var studentLetterGradeAverage = LetterGrade.GetLetterGradeFromNumericGrade(numericGrade);

                    var courseAttemptResult = studentLetterGradeAverage.Equals(LetterGrade.F)
                        ? CourseAttemptResultDescriptor.Fail
                        : CourseAttemptResultDescriptor.Pass;

                    var courseCredit = 1/(decimal) sessionsPerYear;

                    var courseTranscript = new CourseTranscript
                    {
                        CourseAttemptResult = courseAttemptResult.GetStructuredCodeValue(),
                        AttemptedCredits = new Credits {Credits1 = courseCredit},
                        EarnedCredits = new Credits {Credits1 = courseCredit},
                        WhenTakenGradeLevel = currentGradeLevel.GetStructuredCodeValue(),
                        MethodCreditEarned = MethodCreditEarnedDescriptor.ClassroomCredit.GetStructuredCodeValue(),
                        FinalLetterGradeEarned = studentLetterGradeAverage.Grade,
                        FinalNumericGradeEarned = numericGrade,
                        FinalNumericGradeEarnedSpecified = true,
                        CourseReference = new CourseReferenceType
                        {
                            CourseIdentity = new CourseIdentityType
                            {
                                CourseCode = studentCourse.Course.CourseCode,
                                EducationOrganizationReference = Configuration.SchoolProfile.GetEducationOrganizationReference()
                            },
                            CourseLookup = new CourseLookupType
                            {
                                CourseCode = studentCourse.Course.CourseCode,
                                EducationOrganizationReference = Configuration.SchoolProfile.GetEducationOrganizationReference()
                            }
                        },
                        StudentAcademicRecordReference = StudentAcademicRecordHelpers.GetReference(session.StudentAcademicRecordId)
                    };

                    yield return courseTranscript;
                }
            }
        }
    }
}
