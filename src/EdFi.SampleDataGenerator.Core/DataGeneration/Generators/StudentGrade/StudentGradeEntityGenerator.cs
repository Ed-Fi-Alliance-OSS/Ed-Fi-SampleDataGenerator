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

namespace EdFi.SampleDataGenerator.Core.DataGeneration.Generators.StudentGrade
{
    public class StudentGradeEntityGenerator : StudentGradeBaseEntityGenerator
    {
        public override IEntity GeneratesEntity => StudentGradeEntity.Grade;
        public override IEntity[] DependsOnEntities => EntityDependencies.Create(StudentEnrollmentEntity.StudentSectionAssociation, StudentTranscriptEntity.StudentTranscriptSession);

        private Dictionary<DateTime, List<StudentCourse>> _studentCoursesByGradingPeriodBeginDate;

        private List<Section> _sectionsForSchool;
        private List<CourseOffering> _courseOfferingsForSchool;

        public StudentGradeEntityGenerator(IRandomNumberGenerator randomNumberGenerator) : base(randomNumberGenerator)
        {
        }

        protected override void OnConfigure()
        {
            _sectionsForSchool = Configuration.GlobalData.MasterScheduleData.Sections.ForSchool(Configuration.SchoolProfile).ToList();
            _courseOfferingsForSchool = Configuration.GlobalData.MasterScheduleData.CourseOfferings
                .Where(co => _sectionsForSchool.Any(section => section.CourseOfferingReference.ReferencesCourseOffering(co)))
                .ToList();
        }

        protected override void GenerateCore(StudentDataGeneratorContext context)
        {
            //tie back to StudentTranscriptSession to get grade for each session
            //then generate grades for each grading period using that as an average
            var transcriptSessions =
                context.GeneratedStudentData.StudentTranscriptData.StudentTranscriptSessions.ForCurrentSchoolYear();

            var gradingPeriods = Configuration.EducationOrgCalendarData.GradingPeriods
                .ForSchool(Configuration.SchoolProfile.SchoolId)
                .ToList();

            _studentCoursesByGradingPeriodBeginDate = gradingPeriods.ToDictionary(gp => gp.BeginDate,
                gp => new List<StudentCourse>());

            foreach (var transcriptSession in transcriptSessions)
            {
                var studentCoursesForSession = transcriptSession.StudentCourses;
                var gradingPeriodsForSession = gradingPeriods.CompletedDuring(transcriptSession.Session.AsDateRange()).ToList();

                foreach (var studentCourse in studentCoursesForSession)
                {
                    var grades = GradeGenerationHelpers.GenerateGradesFromGradePointAverage(studentCourse.CourseGradeAverage, gradingPeriodsForSession.Count, RandomNumberGenerator, GradeRange.FullGradingScale).ToList();

                    for (var i = 0; i < gradingPeriodsForSession.Count; ++i)
                    {
                        var gradingPeriod = gradingPeriodsForSession[i];
                        _studentCoursesByGradingPeriodBeginDate[gradingPeriod.BeginDate]
                            .Add(
                                new StudentCourse
                                {
                                    Course = studentCourse.Course,
                                    CourseGradeAverage = grades[i]
                                }
                            );
                    }
                }
            }
        }

        public override void GenerateAdditiveData(StudentDataGeneratorContext context, IDataPeriod dataPeriod)
        {
            var grades = GenerateGradesForDataPeriod(context, dataPeriod);
            context.GeneratedStudentData.StudentGradeData.Grades.AddRange(grades);
        }

        private IEnumerable<Grade> GenerateGradesForDataPeriod(StudentDataGeneratorContext context, IDataPeriod dataPeriod)
        {
            var studentSectionAssociations = context.GeneratedStudentData.StudentEnrollmentData.StudentSectionAssociations;

            var gradingPeriods =
                 Configuration.EducationOrgCalendarData.GradingPeriods
                    .ForSchool(Configuration.SchoolProfile.SchoolId)
                    .CompletedDuring(dataPeriod.Intersect(context.EnrollmentDateRange))
                    .ToList();

            return studentSectionAssociations.SelectMany(studentSectionAssociation =>
                    GenerateGradesForSectionAssociation(studentSectionAssociation, gradingPeriods));
        }

        private IEnumerable<Grade> GenerateGradesForSectionAssociation(StudentSectionAssociation studentSectionAssociation, IEnumerable<GradingPeriod> gradingPeriods)
        {
            var gradingPeriodsForSection = gradingPeriods.Where(
                x =>
                    x.BeginDate >= studentSectionAssociation.BeginDate &&
                    x.BeginDate <= studentSectionAssociation.EndDate);

            return gradingPeriodsForSection.Select(gradingPeriod => CreateGrade(studentSectionAssociation, gradingPeriod, GradeTypeDescriptor.GradingPeriod));
        }

        private Grade CreateGrade(StudentSectionAssociation studentSectionAssociation, GradingPeriod gradingPeriod, GradeTypeDescriptor gradeType)
        {
            var studentCourses = _studentCoursesByGradingPeriodBeginDate[gradingPeriod.BeginDate];

            var section = _sectionsForSchool.First(s => studentSectionAssociation.SectionReference.ReferencesSection(s));
            var courseOffering = _courseOfferingsForSchool.First(co => section.CourseOfferingReference.ReferencesCourseOffering(co));

            var course = studentCourses.First(sc => courseOffering.CourseReference.ReferencesCourse(sc.Course));

            var letterGrade = LetterGrade.GetLetterGradeFromNumericGrade(course.CourseGradeAverage);

            return new Grade
            {
                LetterGradeEarned = letterGrade.DisplayName,
                NumericGradeEarned = course.CourseGradeAverage,
                NumericGradeEarnedSpecified = true,
                DiagnosticStatement = string.Empty,
                GradeType = gradeType.GetStructuredCodeValue(),
                PerformanceBaseConversion = letterGrade.ToPerformanceBaseConversionDescriptor().GetStructuredCodeValue(),
                StudentSectionAssociationReference = studentSectionAssociation.GetStudentSectionAssociationReference(),
                GradingPeriodReference = gradingPeriod.GetGradingPeriodReference()
            };
        }
    }
}