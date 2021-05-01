using System;
using System.Collections.Generic;
using System.Linq;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common.Dependencies;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common.Entity;
using EdFi.SampleDataGenerator.Core.DataGeneration.InterchangeEntities;
using EdFi.SampleDataGenerator.Core.Entities;
using EdFi.SampleDataGenerator.Core.Helpers;

namespace EdFi.SampleDataGenerator.Core.DataGeneration.Generators.StudentGradebook
{
    public class GradebookEntryEntityGenerator : StudentGradebookEntityGlobalGenerator
    {
        const int WeeksBetweenHomework = 1;
        const int WeeksBetweenQuizzes = 2;
        const int WeeksBetweenUnitTests = 6;

        private IEnumerable<CalendarDate> _currentSchoolInstructionalDays;

        public override IEntity GeneratesEntity => StudentGradebookEntity.GradebookEntry;
        public override IEntity[] DependsOnEntities => EntityDependencies.Create(EducationOrganizationEntity.School, MasterScheduleEntity.Section);

        public GradebookEntryEntityGenerator(IRandomNumberGenerator randomNumberGenerator) : base(randomNumberGenerator)
        {
        }

        protected override void GenerateCore(GlobalDataGeneratorContext context)
        {
            var gradebookEntries = GenerateGradebookEntries(context);
            context.GlobalData.GradebookEntries = gradebookEntries;
        }

        private List<GradebookEntry> GenerateGradebookEntries(GlobalDataGeneratorContext context)
        {
            var gradebookEntries = new List<GradebookEntry>();

            foreach (var school in context.GlobalData.EducationOrganizationData.Schools)
            {
                var schoolReference = school.GetSchoolReference();
                _currentSchoolInstructionalDays = Configuration.GetSchoolInstructionalDays(schoolReference).ToList();

                var sessions = Configuration.EducationOrgCalendarData.Sessions;

                foreach (var session in sessions)
                {
                    var gradingPeriods = Configuration.EducationOrgCalendarData.GradingPeriods
                        .ForSchool(school.SchoolId)
                        .StartedOnOrAfter(session.BeginDate)
                        .CompletedOnOrBefore(session.EndDate)
                        .ToList();

                    var courses = context.GlobalData.MasterScheduleData.CourseOfferings
                        .Where(c => c.SessionReference.ReferencesSession(session));

                    var sections = context.GlobalData.MasterScheduleData.Sections
                        .Where(x => x.LocationSchoolReference.ReferencesSameSchoolAs(schoolReference) && courses.Any(c => x.CourseOfferingReference.ReferencesCourseOffering(c))).ToList();

                    foreach (var section in sections)
                    {
                        gradebookEntries.AddRange(GenerateGradebookEntriesForSectionAssociation(section.GetSectionReference(), gradingPeriods));
                    }
                }
            }

            return gradebookEntries;
        }

        private IEnumerable<GradebookEntry> GenerateGradebookEntriesForSectionAssociation(SectionReferenceType sectionReference, IEnumerable<GradingPeriod> gradingPeriods)
        {
            var gradebookEntries = new List<GradebookEntry>();

            var schoolCalendarWeek = 1;

            foreach (var gradingPeriod in gradingPeriods)
            {
                var weeksInGradingPeriod = ((int)(gradingPeriod.EndDate - gradingPeriod.BeginDate).TotalDays/7) + 1;

                for (var gradingPeriodWeek = 0; gradingPeriodWeek < weeksInGradingPeriod; ++gradingPeriodWeek)
                {
                    if (schoolCalendarWeek % WeeksBetweenHomework == 0)
                    {
                        var dateAssigned = GetGradebookDateAssigned(gradingPeriod, gradingPeriodWeek);
                        gradebookEntries.Add(CreateGradebookEntry(sectionReference, gradingPeriod, GradebookEntryTypeDescriptor.Homework, dateAssigned));
                    }

                    if (schoolCalendarWeek % WeeksBetweenUnitTests == 0)
                    {
                        var dateAssigned = GetGradebookDateAssigned(gradingPeriod, gradingPeriodWeek, dayOffset: 4); //set tests for late week with a 4 day offset
                        gradebookEntries.Add(CreateGradebookEntry(sectionReference, gradingPeriod, GradebookEntryTypeDescriptor.UnitTest, dateAssigned));
                    }

                    //only take a quiz on weeks where we don't have a test scheduled
                    else if (schoolCalendarWeek % WeeksBetweenQuizzes == 0)
                    {
                        var dateAssigned = GetGradebookDateAssigned(gradingPeriod, gradingPeriodWeek, dayOffset: 3); //set quizzes for mid-week with a 3 day offset
                        gradebookEntries.Add(CreateGradebookEntry(sectionReference, gradingPeriod, GradebookEntryTypeDescriptor.Quiz, dateAssigned));
                    }

                    ++schoolCalendarWeek;
                }
            }

            return gradebookEntries;
        }

        private GradebookEntry CreateGradebookEntry(SectionReferenceType sectionReference, GradingPeriod gradingPeriod, GradebookEntryTypeDescriptor gradebookEntryType, DateTime dateAssigned)
        {
            var courseCode = $"{sectionReference.SectionIdentity.CourseOfferingReference.CourseOfferingIdentity.LocalCourseCode}";
            var gradingPeriodName = gradingPeriod.GradingPeriod1.ParseToCodeValue();
            var gradebookEntryTypeName = gradebookEntryType.CodeValue;
            var gradebookEntryTitle = $"{courseCode} - {gradingPeriodName} - {gradebookEntryTypeName} - {dateAssigned:yyyyMMdd}";

            return new GradebookEntry
            {
                SectionReference = sectionReference,
                GradingPeriodReference = gradingPeriod.GetGradingPeriodReference(),
                GradebookEntryType = gradebookEntryType.GetStructuredCodeValue(),
                DateAssigned = dateAssigned,
                GradebookEntryTitle = gradebookEntryTitle
            };
        }

        private DateTime GetGradebookDateAssigned(GradingPeriod gradingPeriod, int weekNumber, int dayOffset = 0)
        {
            var dateAssigned = gradingPeriod.BeginDate.AddDays(weekNumber * 7 + dayOffset);
            if (dateAssigned > gradingPeriod.EndDate)
                dateAssigned = gradingPeriod.EndDate;

            var nextDate = _currentSchoolInstructionalDays.FirstOrDefault(d => d.Date >= dateAssigned) ??
                           _currentSchoolInstructionalDays.Last(d => d.Date < dateAssigned);

            return nextDate.Date;
        }
    }
}
