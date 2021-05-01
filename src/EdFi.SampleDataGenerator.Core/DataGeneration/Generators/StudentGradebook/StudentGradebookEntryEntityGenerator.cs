using System.Collections.Generic;
using System.Linq;
using EdFi.SampleDataGenerator.Core.Config;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common.Dependencies;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common.Entity;
using EdFi.SampleDataGenerator.Core.DataGeneration.InterchangeEntities;
using EdFi.SampleDataGenerator.Core.Entities;
using EdFi.SampleDataGenerator.Core.Helpers;

namespace EdFi.SampleDataGenerator.Core.DataGeneration.Generators.StudentGradebook
{
    public class StudentGradebookEntryEntityGenerator : StudentGradebookEntityGenerator
    {
        public override IEntity GeneratesEntity => StudentGradebookEntity.StudentGradebookEntry;
        public override IEntity[] DependsOnEntities => EntityDependencies.Create(StudentEnrollmentEntity.StudentSectionAssociation, StudentGradeEntity.Grade);

        private List<GradebookEntry> _gradebookEntriesForSchool = new List<GradebookEntry>();

        public StudentGradebookEntryEntityGenerator(IRandomNumberGenerator randomNumberGenerator) : base(randomNumberGenerator)
        {
        }

        protected override void OnConfigure()
        {
            var schoolReference = Configuration.SchoolProfile.GetSchoolReference();
            var sessions = Configuration.EducationOrgCalendarData.Sessions
                            .StartedBefore(Configuration.GlobalConfig.TimeConfig.DataClockConfig.EndDate)
                            .ToList();

            var courses = Configuration.GlobalData.MasterScheduleData.CourseOfferings
                            .Where(c => sessions.Any(s => c.SessionReference.ReferencesSession(s)))
                            .ToList();

            var sections = Configuration.GlobalData.MasterScheduleData.Sections
                            .Where(x => x.LocationSchoolReference.ReferencesSameSchoolAs(schoolReference) && courses.Any(c => x.CourseOfferingReference.ReferencesCourseOffering(c)))
                            .ToList();

            _gradebookEntriesForSchool = Configuration.GlobalData.GradebookEntries
                                            .Where(gbe => sections.Any(s => gbe.SectionReference.ReferencesSection(s)))
                                            .ToList();
        }

        public override void GenerateAdditiveData(StudentDataGeneratorContext context, IDataPeriod dataPeriod)
        {
            var studentGradebookEntries = GenerateStudentGradebookEntriesForDataPeriod(context, dataPeriod);
            context.GeneratedStudentData.StudentGradebookData.StudentGradebookEntries.AddRange(studentGradebookEntries);
        }

        private IEnumerable<StudentGradebookEntry> GenerateStudentGradebookEntriesForDataPeriod(StudentDataGeneratorContext context, IDataPeriod dataPeriod)
        {
            var gradingPeriods =
                 Configuration.EducationOrgCalendarData.GradingPeriods
                    .ForSchool(Configuration.SchoolProfile.SchoolId)
                    .CompletedDuring(dataPeriod.AsDateRange())
                    .ToList();

            var studentSectionAssociations = context.GeneratedStudentData.StudentEnrollmentData.StudentSectionAssociations;

            foreach (var gradingPeriod in gradingPeriods)
            {
                var studentGrades = context.GeneratedStudentData.StudentGradeData.Grades
                    .Where(g => g.GradingPeriodReference.ReferencesGradingPeriod(gradingPeriod));

                var gradebookEntriesForGradingPeriod = _gradebookEntriesForSchool
                    .Where
                    (
                        gbe =>
                            gbe.GradingPeriodReference.ReferencesGradingPeriod(gradingPeriod) &&
                            studentSectionAssociations.Any(ssa => ssa.SectionReference.ReferencesSameSectionAs(gbe.SectionReference))
                    )
                    .ToList();

                foreach (var studentGrade in studentGrades)
                {
                    var studentSectionAssociation = studentSectionAssociations.First(ssa =>
                                studentGrade.StudentSectionAssociationReference.ReferencesStudentSectionAssociation(ssa));

                    var gradebookEntriesForSection = gradebookEntriesForGradingPeriod
                        .Where(gbe => gbe.SectionReference.ReferencesSameSectionAs(studentSectionAssociation.SectionReference) && context.EnrollmentDateRange.Contains(gbe.DateAssigned))
                        .ToList();

                    var averageGradeForGradingPeriod = (int) studentGrade.NumericGradeEarned;
                    var grades = GradeGenerationHelpers.GenerateGradesFromGradePointAverage(averageGradeForGradingPeriod, gradebookEntriesForSection.Count, RandomNumberGenerator, GradeRange.FullGradingScale).ToList();

                    for (var i = 0; i < gradebookEntriesForSection.Count; ++i)
                    {
                        var gradebookEntry = gradebookEntriesForSection[i];
                        var grade = grades[i];

                        yield return CreateStudentGradebookEntry(gradebookEntry, studentSectionAssociation, grade);
                    }
                }
            }
        }

        private static StudentGradebookEntry CreateStudentGradebookEntry(GradebookEntry gradebookEntry, StudentSectionAssociation studentSectionAssociation, int numericGrade)
        {
            return new StudentGradebookEntry
            {
                GradebookEntryReference = gradebookEntry.GetGradebookEntryReference(),
                StudentSectionAssociationReference = studentSectionAssociation.GetStudentSectionAssociationReference(),
                LetterGradeEarned = LetterGrade.GetLetterGradeFromNumericGrade(numericGrade).Grade,
                NumericGradeEarned = numericGrade,
                NumericGradeEarnedSpecified = true,
                DateFulfilled = gradebookEntry.DateAssigned,
                DateFulfilledSpecified = true
            };
        }
    }
}
