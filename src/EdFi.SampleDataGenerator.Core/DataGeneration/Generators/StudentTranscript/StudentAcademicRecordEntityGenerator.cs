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
    public class StudentAcademicRecordEntityGenerator : StudentTranscriptEntityGenerator
    {
        public StudentAcademicRecordEntityGenerator(IRandomNumberGenerator randomNumberGenerator) : base(randomNumberGenerator)
        {
        }

        public override IEntity GeneratesEntity => StudentTranscriptEntity.StudentAcademicRecord;
        public override IEntity[] DependsOnEntities => EntityDependencies.Create(StudentEntity.Student, StudentEnrollmentEntity.GraduationPlan, StudentTranscriptEntity.CourseTranscript);

        public override void GenerateAdditiveData(StudentDataGeneratorContext context, IDataPeriod dataPeriod)
        {
            var transcriptSessionsCompletedInDataPeriod = context.GeneratedStudentData.StudentTranscriptData.StudentTranscriptSessions
                .CompletedInDateRange(dataPeriod.Intersect(context.EnrollmentDateRange));
            var studentAcademicRecords = GenerateStudentAcademicRecords(context, transcriptSessionsCompletedInDataPeriod);
            context.GeneratedStudentData.StudentTranscriptData.StudentAcademicRecords.AddRange(studentAcademicRecords);
        }

        protected override void GenerateCore(StudentDataGeneratorContext context)
        {
            var previousYearsStudentTranscriptSessions = context.GeneratedStudentData.StudentTranscriptData.StudentTranscriptSessions.ForPreviousSchoolYears();
            var studentAcademicRecords = GenerateStudentAcademicRecords(context, previousYearsStudentTranscriptSessions);
            context.GeneratedStudentData.StudentTranscriptData.StudentAcademicRecords.AddRange(studentAcademicRecords);
        }

        private IEnumerable<StudentAcademicRecord> GenerateStudentAcademicRecords(StudentDataGeneratorContext context, IEnumerable<StudentTranscriptSession> sessionsToGenerate)
        {
            var student = context.GeneratedStudentData.StudentData.Student;

            var totalCreditsEarned = context.GeneratedStudentData.StudentTranscriptData.StudentAcademicRecords.GetTotalCreditsEarned();

            foreach (var session in sessionsToGenerate)
            {
                var coursesForSession = context.GeneratedStudentData.StudentTranscriptData.CourseTranscripts
                    .Where(ct => ct.StudentAcademicRecordReference.References(session.StudentAcademicRecordId))
                    .ToList();

                var sessionCreditsEarned = coursesForSession.Sum(c => c.EarnedCredits.Credits1);
                var sessionCreditsAttempted = coursesForSession.Sum(c => c.AttemptedCredits.Credits1);
                totalCreditsEarned += sessionCreditsEarned;

                var studentAcademicRecord = new StudentAcademicRecord
                {
                    id = session.StudentAcademicRecordId,
                    CumulativeEarnedCredits = new Credits { Credits1 = totalCreditsEarned },
                    SessionEarnedCredits = new Credits { Credits1 = sessionCreditsEarned },
                    SessionAttemptedCredits = new Credits { Credits1 = sessionCreditsAttempted },
                    StudentReference = student.GetStudentReference(),
                    EducationOrganizationReference = Configuration.SchoolProfile.GetEducationOrganizationReference(),
                    SchoolYear = session.SchoolYear,
                    Term = session.Term
                };

               yield return studentAcademicRecord;
            }
        }
    }
}
