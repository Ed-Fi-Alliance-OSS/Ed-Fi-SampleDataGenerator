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

namespace EdFi.SampleDataGenerator.Core.DataGeneration.Generators.StudentAttendance
{
    public class StudentSectionAttendanceEventEntityGenerator : StudentAttendanceEntityGenerator
    {
        public override IEntity GeneratesEntity => StudentAttendanceEntity.StudentSectionAttendanceEvent;
        public override IEntity[] DependsOnEntities => EntityDependencies.Create(StudentEntity.Student, StudentEnrollmentEntity.StudentSectionAssociation);

        private Dictionary<string, List<CalendarDate>> _schoolCalendarDatesBySessionName;
        private List<Section> _sectionsForSchool = new List<Section>();

        public const double DefaultAverageAttendanceRate = 0.96;

        public const double DefaultAverageTardyRate = 0.01;

        private const double MaxPercentageOfAbsencesThatAreUnexecused = 0.8;
        private static readonly AttendanceParameters StandardAttendanceParameters = new AttendanceParameters
        {
            AverageAbsenceRate = 1 - DefaultAverageAttendanceRate,
            AverageTardyRate = DefaultAverageTardyRate,
        };

        private static readonly AttendanceTemplate StandardTemplate = new AttendanceTemplate
        {
            AttendanceTemplateType = AttendanceTemplateType.Standard,
            TemplateParameters = StandardAttendanceParameters,
            StandardParameters = StandardAttendanceParameters
        };

        private static readonly AttendanceTemplate MissWeekdayTemplate = new AttendanceTemplate
        {
            AttendanceTemplateType = AttendanceTemplateType.MissSpecificDay,
            MaxPerformanceIndex = StudentPerformanceProfileDistribution.BottomQuartile,
            Probability = 0.005,
            TemplateParameters = new AttendanceParameters
            {
                AverageAbsenceRate = 0.95,
                AverageTardyRate = null
            },
            StandardParameters = new AttendanceParameters
            {
                AverageAbsenceRate = 0.01,
                AverageTardyRate = StandardAttendanceParameters.AverageTardyRate
            },
        };

        private static readonly AttendanceTemplate FrequentlyTardyTemplate = new AttendanceTemplate
        {
            AttendanceTemplateType = AttendanceTemplateType.FrequentlyTardy,
            MaxPerformanceIndex = StudentPerformanceProfileDistribution.FiftiethPercentile,
            Probability = 0.05,
            TemplateParameters = new AttendanceParameters
            {
                AverageAbsenceRate = null,
                AverageTardyRate = 1.0
            },
            StandardParameters = new AttendanceParameters
            {
                AverageAbsenceRate = StandardAttendanceParameters.AverageAbsenceRate,
                AverageTardyRate = null
            },
        };

        private static readonly AttendanceTemplate AbsentAtBeginningOfDayTemplate = new AttendanceTemplate
        {
            AttendanceTemplateType = AttendanceTemplateType.AbsentAtBeginningOfDay,
            MaxPerformanceIndex = StudentPerformanceProfileDistribution.BottomQuartile,
            Probability = 0.01,
            TemplateParameters = new AttendanceParameters
            {
                AverageAbsenceRate = 1.0,
                AverageTardyRate = null,
            },
            StandardParameters = StandardAttendanceParameters,
        };

        private static readonly AttendanceTemplate AbsentAtEndOfDayTemplate = new AttendanceTemplate
        {
            AttendanceTemplateType = AttendanceTemplateType.AbsentAtEndOfDay,
            MaxPerformanceIndex = StudentPerformanceProfileDistribution.BottomQuartile,
            Probability = 0.01,
            TemplateParameters = new AttendanceParameters
            {
                AverageAbsenceRate = 1.0,
                AverageTardyRate = null
            },
            StandardParameters = StandardAttendanceParameters
        };

        private static readonly AttendanceTemplate TardyForFirstPeriodTemplate = new AttendanceTemplate
        {
            AttendanceTemplateType = AttendanceTemplateType.TardyForFirstPeriod,
            MaxPerformanceIndex = StudentPerformanceProfileDistribution.FiftiethPercentile,
            Probability = 0.05,
            TemplateParameters = new AttendanceParameters
            {
                AverageAbsenceRate = null,
                AverageTardyRate = 1.0,
            },
            StandardParameters = StandardAttendanceParameters
        };

        private static readonly AttendanceTemplate[] NonStandardAttendanceTemplates =
        {
            MissWeekdayTemplate,
            FrequentlyTardyTemplate,
            AbsentAtBeginningOfDayTemplate,
            AbsentAtEndOfDayTemplate,
            TardyForFirstPeriodTemplate
        };

        private static readonly DayOfWeek[] Weekdays = EnumHelpers.GetAll<DayOfWeek>().Where(d => d.IsWeekday()).ToArray();

        public StudentSectionAttendanceEventEntityGenerator(IRandomNumberGenerator randomNumberGenerator) : base(randomNumberGenerator)
        {
        }

        protected override void OnConfigure()
        {
            var averageAttendanceRate = 
                Configuration.SchoolProfile.AttendanceProfile?.AverageAttendanceRate ??
                DefaultAverageAttendanceRate;

            var averageTardyRate =
                Configuration.SchoolProfile.AttendanceProfile?.AverageTardyRate ??
                DefaultAverageTardyRate;

            StandardAttendanceParameters.AverageAbsenceRate = 1.0 - averageAttendanceRate;
            StandardAttendanceParameters.AverageTardyRate = averageTardyRate;

            var schoolCalendarDates = Configuration.GetSchoolInstructionalDays().ToList();
            _schoolCalendarDatesBySessionName = Configuration.GetAvailableSessions()
                .ToDictionary(
                    session => session.SessionName,
                    session => schoolCalendarDates.WithinDateRange(session.AsDateRange()).ToList()
                );

            _sectionsForSchool = Configuration.MasterScheduleData.Sections
                .ForSchool(Configuration.SchoolProfile)
                .ToList();
        }

        public override void GenerateAdditiveData(StudentDataGeneratorContext context, IDataPeriod dataPeriod)
        {
            var generatedAttendanceEvents = new List<StudentSectionAttendanceEvent>();
            var studentPerformanceIndex = context.StudentPerformanceProfile.PerformanceIndex;
            var template = context.GeneratedStudentData.StudentAttendanceData.StudentAttendanceTemplate;

            foreach (var session in Configuration.GetAvailableSessions(dataPeriod))
            {
                if (!context.EnrollmentDateRange.Overlaps(session.AsDateRange()))
                    continue;

                var calendarDates = _schoolCalendarDatesBySessionName[session.SessionName].WithinDateRange(dataPeriod.Intersect(context.EnrollmentDateRange)).ToList();
                var sectionsForTerm = _sectionsForSchool.ForSession(session);

                var studentSections = context.GeneratedStudentData.StudentEnrollmentData.StudentSectionAssociations
                    .Where(ssa => sectionsForTerm.Any(section => ssa.SectionReference.ReferencesSection(section)))
                    .ToList();

                var internalContext = new StudentSectionAttendanceGeneratorContext
                {
                    AttendanceTemplate = template,
                    AvailableCalendarDates = calendarDates,
                    StudentSections = studentSections
                };

                if (template.AttendanceTemplateType == AttendanceTemplateType.FrequentlyTardy)
                {
                    var events = GenerateFrequentlyTardyEvents(internalContext);
                    generatedAttendanceEvents.AddRange(events);
                }

                if (template.AttendanceTemplateType == AttendanceTemplateType.MissSpecificDay)
                {
                    var events = GenerateMissSpecificDayEvents(internalContext);
                    generatedAttendanceEvents.AddRange(events);
                }

                if (template.AttendanceTemplateType == AttendanceTemplateType.AbsentAtBeginningOfDay)
                {
                    var events = GenerateAbsentAtBeginningOfDayEvents(internalContext);
                    generatedAttendanceEvents.AddRange(events);
                }

                if (template.AttendanceTemplateType == AttendanceTemplateType.AbsentAtEndOfDay)
                {
                    var events = GenerateAbsentAtEndOfDayEvents(internalContext);
                    generatedAttendanceEvents.AddRange(events);
                }

                if (template.AttendanceTemplateType == AttendanceTemplateType.TardyForFirstPeriod)
                {
                    var events = GenerateTardyForFirstPeriodEvents(internalContext);
                    generatedAttendanceEvents.AddRange(events);
                }

                var standardAttendanceEvents = GenerateStandardEvents(internalContext, studentPerformanceIndex, generatedAttendanceEvents);
                generatedAttendanceEvents.AddRange(standardAttendanceEvents);
            }

            context.GeneratedStudentData.StudentAttendanceData.StudentSectionAttendanceEvents.AddRange(generatedAttendanceEvents.ToArray());
        }

        protected override void GenerateCore(StudentDataGeneratorContext context)
        {
            //choose the student's attendance template up front so it's applied consistently
            //through every data period
            var studentPerformanceIndex = context.StudentPerformanceProfile.PerformanceIndex;
            var template = NonStandardAttendanceTemplates
                .Where(t => (t.MinPerformanceIndex == null || studentPerformanceIndex >= t.MinPerformanceIndex) &&
                            (t.MaxPerformanceIndex == null || studentPerformanceIndex <= t.MaxPerformanceIndex))
                .GetRandomItemWithDistribution(t => t.Probability, RandomNumberGenerator)
                ?? StandardTemplate;

            context.GeneratedStudentData.StudentAttendanceData.StudentAttendanceTemplate = template;
        }

        private class StudentSectionAttendanceGeneratorContext
        {
            public AttendanceTemplate AttendanceTemplate { get; set; }
            public List<StudentSectionAssociation> StudentSections { get; set; }
            public List<CalendarDate> AvailableCalendarDates { get; set; }
        }

        private IEnumerable<StudentSectionAttendanceEvent> GenerateStandardEvents(StudentSectionAttendanceGeneratorContext context, double studentPerformanceIndex, List<StudentSectionAttendanceEvent> existingEvents)
        {
            //apply the standard template events
            var absenceRate = context.AttendanceTemplate.StandardParameters.AverageAbsenceRate;
            var tardyRate = context.AttendanceTemplate.StandardParameters.AverageTardyRate;

            //but only to dates that we haven't already used in the above templates (if applicable)
            var datesThatAlreadyContainEvents = new HashSet<DateTime>(existingEvents.Select(x => x.AttendanceEvent.EventDate).Distinct());
            var remainingCalendarDates =
                context.AvailableCalendarDates.Where(cd => !datesThatAlreadyContainEvents.Contains(cd.Date)).ToList();

            var absentDates = GenerateEventDates(remainingCalendarDates, absenceRate);
            var tardyDates = GenerateEventDates(remainingCalendarDates.Except(absentDates).ToList(), tardyRate);

            var unexecusedRate = GetStudentUnexecusedRate(studentPerformanceIndex);

            Func<AttendanceEventCategoryDescriptor> attendanceEventTypeFunc = () =>
                RandomNumberGenerator.GetRandomBool(unexecusedRate)
                    ? AttendanceEventCategoryDescriptor.UnexcusedAbsence
                    : AttendanceEventCategoryDescriptor.ExcusedAbsence;


            var absenceEvents = absentDates.SelectMany(ed => GenerateSectionAttendanceEvents(ed, attendanceEventTypeFunc(), context.StudentSections));
            var tardyEvents = tardyDates.Select(ed => GenerateSectionAttendanceEvent(ed, AttendanceEventCategoryDescriptor.Tardy, context.StudentSections.GetRandomItem(RandomNumberGenerator)));

            return absenceEvents.SafeConcat(tardyEvents);
        }

        private IEnumerable<StudentSectionAttendanceEvent> GenerateAbsentAtBeginningOfDayEvents(StudentSectionAttendanceGeneratorContext context)
        {
            const double tardyForNextPeriodRate = 0.25;

            var absenceRate = context.AttendanceTemplate.TemplateParameters.AverageAbsenceRate;
            var eventDates = GenerateEventDates(context.AvailableCalendarDates, absenceRate);

            var firstPeriod = GetStudentSectionForClassPeriod(context, 1);
            var secondPeriod = GetStudentSectionForClassPeriod(context, 2);

            var absenceEvents = eventDates.Select(ed => GenerateSectionAttendanceEvent(ed, AttendanceEventCategoryDescriptor.UnexcusedAbsence, firstPeriod));
            var tardyEvents = eventDates.Where(ed => RandomNumberGenerator.GetRandomBool(tardyForNextPeriodRate))
                .Select(ed => GenerateSectionAttendanceEvent(ed, AttendanceEventCategoryDescriptor.Tardy, secondPeriod));

            return absenceEvents.SafeConcat(tardyEvents);
        }

        private IEnumerable<StudentSectionAttendanceEvent> GenerateTardyForFirstPeriodEvents(StudentSectionAttendanceGeneratorContext context)
        {
            var tardyRate = context.AttendanceTemplate.TemplateParameters.AverageTardyRate;
            var eventDates = GenerateEventDates(context.AvailableCalendarDates, tardyRate);

            var firstPeriod = GetStudentSectionForClassPeriod(context, 1);
            var events = eventDates.Select(ed => GenerateSectionAttendanceEvent(ed, AttendanceEventCategoryDescriptor.Tardy, firstPeriod));
            return events;
        }

        private IEnumerable<StudentSectionAttendanceEvent> GenerateAbsentAtEndOfDayEvents(StudentSectionAttendanceGeneratorContext context)
        {
            var absenceRate = context.AttendanceTemplate.TemplateParameters.AverageAbsenceRate;
            var eventDates = GenerateEventDates(context.AvailableCalendarDates, absenceRate);

            var lastPeriod = GetStudentSectionForClassPeriod(context, Configuration.SchoolProfile.CourseLoad);

            var events = eventDates.Select(ed => GenerateSectionAttendanceEvent(ed, AttendanceEventCategoryDescriptor.UnexcusedAbsence, lastPeriod));
            return events;
        }

        private IEnumerable<StudentSectionAttendanceEvent> GenerateMissSpecificDayEvents(StudentSectionAttendanceGeneratorContext context)
        {
            var absenceRate = context.AttendanceTemplate.TemplateParameters.AverageAbsenceRate;
            var weekDay = Weekdays.GetRandomItem(RandomNumberGenerator);
            var calendarDates = context.AvailableCalendarDates.Where(d => d.Date.Date.DayOfWeek == weekDay).ToList();
            var eventDates = GenerateEventDates(calendarDates, absenceRate);

            var events = eventDates.SelectMany(ed => GenerateSectionAttendanceEvents(ed, AttendanceEventCategoryDescriptor.UnexcusedAbsence, context.StudentSections));
            return events;
        }

        private IEnumerable<StudentSectionAttendanceEvent> GenerateFrequentlyTardyEvents(StudentSectionAttendanceGeneratorContext context)
        {
            var tardyRate = context.AttendanceTemplate.TemplateParameters.AverageTardyRate;
            var eventDates = GenerateEventDates(context.AvailableCalendarDates, tardyRate);

            var events =
                eventDates.Select(
                    ed =>
                        GenerateSectionAttendanceEvent(ed, AttendanceEventCategoryDescriptor.Tardy,
                            context.StudentSections.GetRandomItem(RandomNumberGenerator)));
            return events;
        }

        private static double GetStudentUnexecusedRate(double studentPerformanceIndex)
        {
            //unexcused rate should vary based on student profile - it's much more likely that our
            //worst students have unexcused absences, yet all students are likely to have *some* excused absences
            return (1 - studentPerformanceIndex) * MaxPercentageOfAbsencesThatAreUnexecused;
        }

        private StudentSectionAttendanceEvent GenerateSectionAttendanceEvent(CalendarDate calendarDate, AttendanceEventCategoryDescriptor eventType, StudentSectionAssociation studentSectionAssociation)
        {
            return new StudentSectionAttendanceEvent
            {
                AttendanceEvent = new AttendanceEvent
                {
                    AttendanceEventCategory = eventType.GetStructuredCodeValue(),
                    EventDate = calendarDate.Date
                },
                SectionReference = studentSectionAssociation.SectionReference,
                StudentReference = studentSectionAssociation.StudentReference
            };
        }

        private IEnumerable<StudentSectionAttendanceEvent> GenerateSectionAttendanceEvents(CalendarDate calendarDate, AttendanceEventCategoryDescriptor eventType, IEnumerable<StudentSectionAssociation> studentSectionAssociations)
        {
            return studentSectionAssociations.Select(sa => GenerateSectionAttendanceEvent(calendarDate, eventType, sa));
        }

        private List<CalendarDate> GenerateEventDates(List<CalendarDate> sourceList, double? lambda)
        {
            var result = new List<CalendarDate>();
            var eventDateOffset = 0;

            if (lambda == null || lambda < 0.00001)
                return result;

            do
            {
                var eventOccurrence = RandomNumberGenerator.GeneratePoissonDelayInt(lambda.Value);
                if (MathHelpers.AdditionWillOverflowInteger(eventDateOffset, eventOccurrence))
                    break;

                eventDateOffset += eventOccurrence;

                if (eventDateOffset < sourceList.Count)
                {
                    result.Add(sourceList[eventDateOffset]);
                }

            } while (eventDateOffset < sourceList.Count);

            return result;
        }

        private StudentSectionAssociation GetStudentSectionForClassPeriod(StudentSectionAttendanceGeneratorContext context, int classPeriod)
        {
            return context.StudentSections.GetStudentSectionForClassPeriod(Configuration.MasterScheduleData.Sections.ForSchool(Configuration.SchoolProfile), classPeriod);
        }
    }
}
