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

namespace EdFi.SampleDataGenerator.Core.DataGeneration.Generators.StudentEnrollment
{
    public sealed class StudentSectionAssociationEntityGenerator : StudentEnrollmentEntityGenerator
    {
        public StudentSectionAssociationEntityGenerator(IRandomNumberGenerator randomNumberGenerator) : base(randomNumberGenerator)
        {
        }

        public override IEntity GeneratesEntity => StudentEnrollmentEntity.StudentSectionAssociation;
        public override IEntity[] DependsOnEntities => EntityDependencies.Create(StudentEntity.Student, StudentTranscriptEntity.StudentTranscriptSession);

        public override void GenerateAdditiveData(StudentDataGeneratorContext context, IDataPeriod dataPeriod)
        {
            var sectionAssignments = GetSectionAssignmentsForStudent(context, dataPeriod);
            context.GeneratedStudentData.StudentEnrollmentData.StudentSectionAssociations.AddRange(sectionAssignments);

            EndStudentSectionAssignmentsForUnenrollments(context, dataPeriod);
        }

        private void EndStudentSectionAssignmentsForUnenrollments(StudentDataGeneratorContext context, IDataPeriod dataPeriod)
        {
            var dataPeriodDateRange = dataPeriod.AsDateRange();
            if (context.EnrollmentDateRange.EndsIn(dataPeriodDateRange))
            {
                foreach (var studentSectionAssociation in context.GeneratedStudentData.StudentEnrollmentData.StudentSectionAssociations)
                {
                    if (studentSectionAssociation.EndDate > context.EnrollmentDateRange.EndDate)
                    {
                        studentSectionAssociation.EndDate = context.EnrollmentDateRange.EndDate;
                    }
                }
            }
        }


        private IEnumerable<StudentSectionAssociation> GetSectionAssignmentsForStudent(StudentDataGeneratorContext context, IDataPeriod dataPeriod)
        {
            var classPeriods = Configuration.GlobalData.EducationOrganizationData.ClassPeriods
                .Where(cp => cp.SchoolReference.ReferencesSchool(Configuration.SchoolProfile))
                .ToList();

            var dataPeriodDateRange = dataPeriod.AsDateRange();

            var transcriptSessionsToGenerateThisDataPeriod = context.GeneratedStudentData.StudentTranscriptData
                .StudentTranscriptSessions
                .Where(session =>
                {
                    if (session.Session == null)
                        return false;

                    var sessionDateRange = session.Session.AsDateRange();
                    var enrollmentDateRange = context.EnrollmentDateRange;

                    var sectionAssociationBeginDate = sessionDateRange.Intersect(enrollmentDateRange)?.StartDate;

                    return sectionAssociationBeginDate != null && dataPeriodDateRange.Contains(sectionAssociationBeginDate.Value);
                });

            foreach (var transcriptSession in transcriptSessionsToGenerateThisDataPeriod)
            {
                var courseOfferings = Configuration.GlobalData.MasterScheduleData.CourseOfferings
                    .Where(co => co.SchoolReference.ReferencesSchool(Configuration.SchoolProfile) && co.SessionReference.ReferencesSession(transcriptSession.Session))
                    .ToList();

                var availableSections = Configuration.GlobalData.MasterScheduleData.Sections
                    .Where(s => classPeriods.Any(cp => s.ClassPeriodReference.First().ReferencesClassPeriod(cp)))
                    .ToList();

                var numberOfSectionsToAssign = Math.Min(classPeriods.Count, transcriptSession.StudentCourses.Count);

                var sectionAssociationBeginDate = context.EnrollmentDateRange.StartDate > transcriptSession.Session.BeginDate
                    ? context.EnrollmentDateRange.StartDate
                    : transcriptSession.Session.BeginDate;

                for (var i = 0; i < numberOfSectionsToAssign; ++i)
                {
                    var course = transcriptSession.StudentCourses[i].Course;
                    var courseOffering = courseOfferings.First(co => co.CourseReference.ReferencesCourse(course));

                    var classPeriod = classPeriods[i];
                    var section = availableSections.First(s =>
                                s.ClassPeriodReference.First().ReferencesClassPeriod(classPeriod) &&
                                s.CourseOfferingReference.ReferencesCourseOffering(courseOffering));

                    var sectionAssocation = new StudentSectionAssociation
                    {
                        SectionReference = section.GetSectionReference(),
                        StudentReference = context.Student.GetStudentReference(),
                        HomeroomIndicator = classPeriod.IsHomeRoom(),
                        HomeroomIndicatorSpecified = true,
                        BeginDate = sectionAssociationBeginDate,
                        EndDate = transcriptSession.Session.EndDate,
                        EndDateSpecified = true
                    };

                    yield return sectionAssocation;
                }
            }
        }
    }
}
