using System.Collections.Generic;
using System.Linq;
using EdFi.SampleDataGenerator.Core.Config;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common.Dependencies;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common.Entity;
using EdFi.SampleDataGenerator.Core.DataGeneration.InterchangeEntities;
using EdFi.SampleDataGenerator.Core.Entities;
using EdFi.SampleDataGenerator.Core.Helpers;

namespace EdFi.SampleDataGenerator.Core.DataGeneration.Generators.StudentDiscipline
{
    public class DisciplineIncidentEntityGenerator : StudentDisciplineEntityGenerator
    {
        private readonly IncidentLocationDescriptor[] _incidentLocations = DescriptorHelpers.GetAll<IncidentLocationDescriptor>();

        private static int _disciplineEventId = 1;

        public override IEntity GeneratesEntity => StudentDisciplineEntity.DisciplineIncident;
        public override IEntity[] DependsOnEntities => EntityDependencies.Create(StudentEntity.Student, StaffAssociationEntity.Staff);

        private List<CalendarDate> _schoolCalendarDates = new List<CalendarDate>();
        private double _populationAverageDisciplineEventProbabilityPerStudent;
        private double _populationAverageSeriousDisciplineEventProbability;

        public DisciplineIncidentEntityGenerator(IRandomNumberGenerator randomNumberGenerator) : base(randomNumberGenerator)
        {
        }

        protected override void OnConfigure()
        {
            _schoolCalendarDates = Configuration.GetSchoolInstructionalDays().ToList();

            var schoolDaysInYear = _schoolCalendarDates.Count;

            //the Population in question here is the entire School as the DisciplineProfile
            //is attached in the model at the School level
            var totalStudentsInPopulation = Configuration.SchoolProfile.InitialStudentCount;
            var populationLevelDisciplineEventCount = Configuration.SchoolProfile.DisciplineProfile.TotalExpectedDisciplineEvents;
            var populationLevelSeriousDisciplineEventCount = Configuration.SchoolProfile.DisciplineProfile.TotalExpectedSeriousDisciplineEvents;

            _populationAverageDisciplineEventProbabilityPerStudent = populationLevelDisciplineEventCount / (double)(totalStudentsInPopulation * schoolDaysInYear);
            _populationAverageSeriousDisciplineEventProbability = populationLevelSeriousDisciplineEventCount / (double)populationLevelDisciplineEventCount;
        }

        public override void GenerateAdditiveData(StudentDataGeneratorContext context, IDataPeriod dataPeriod)
        {
            var disciplineEvents = new List<DisciplineIncident>();

            var schoolCalendarDatesInPeriod = _schoolCalendarDates.WithinDateRange(dataPeriod.Intersect(context.EnrollmentDateRange)).ToList();

            //if the student is essentially perfect then we won't generate any discipline
            //incidents for them
            if (!context.StudentPerformanceProfile.IsPerfectStudent)
            {
                var schoolDaysInPeriod = schoolCalendarDatesInPeriod.Count;
                var schoolDaysInYear = _schoolCalendarDates.Count;

                //Using population averages we'll adjust event probabilities to the individual student's performance profile.
                //The idea here is the number of discipline events (and the chance that any given event is a Serious offense)
                //is inversely proportional to the student's performance profile.
                //
                //The average across the distribution should match the population average, however, to ensure we generate
                //the expected number of events across the entire population
                var studentDisciplineEventProbability = (1 - context.StudentPerformanceProfile.PerformanceIndex) * _populationAverageDisciplineEventProbabilityPerStudent * 2;
                var studentDisciplineEventRateForGenerationPeriod = studentDisciplineEventProbability * schoolDaysInPeriod / schoolDaysInYear;
                var studentSeriousDisciplineEventProbability = (1 - context.StudentPerformanceProfile.PerformanceIndex) * _populationAverageSeriousDisciplineEventProbability * 2;

                var eventDateOffset = 0;
                var schoolId = Configuration.SchoolProfile.SchoolId;

                do
                {
                    //Assume discipline events are a Poisson process with a rate tailored
                    //per-student, according to the student's individual performance profile
                    var eventOccurrence = RandomNumberGenerator.GeneratePoissonDelayInt(studentDisciplineEventRateForGenerationPeriod);
                    if (MathHelpers.AdditionWillOverflowInteger(eventDateOffset, eventOccurrence))
                        break;

                    eventDateOffset += eventOccurrence;

                    if (eventDateOffset < schoolDaysInPeriod)
                    {
                        var eventHour = RandomNumberGenerator.Generate(8, 15);
                        var eventMinute = RandomNumberGenerator.Generate(0, 60);
                        var secondsSinceMidnight = (eventHour * 3600) + (eventMinute * 60);

                        var eventCalendarDate = schoolCalendarDatesInPeriod[eventDateOffset];
                        var eventDate = eventCalendarDate.Date.AddSeconds(secondsSinceMidnight);

                        var staffReporter = Configuration.GlobalData.StaffAssociationData.Staff.GetRandomItem(RandomNumberGenerator);

                        var isSeriousOffense = RandomNumberGenerator.GetRandomBool(studentSeriousDisciplineEventProbability);
                        var behaviorType = isSeriousOffense
                            ? DisciplineHelpers.SeriousBehaviors.ToStructuredCodeValueFormatArray().GetRandomItem(RandomNumberGenerator)
                            : DisciplineHelpers.NonSeriousBehaviors.ToStructuredCodeValueFormatArray().GetRandomItem(RandomNumberGenerator);

                        var disciplineEvent = new DisciplineIncident
                        {
                            id = $"DISC_{_disciplineEventId}_{schoolId}",
                            IncidentIdentifier = $"{_disciplineEventId}",
                            IncidentDate = eventDate,
                            IncidentTime = eventDate,
                            IncidentTimeSpecified = true,
                            IncidentLocation = _incidentLocations.GetRandomItem(RandomNumberGenerator).GetStructuredCodeValue(),
                            ReporterDescription = ReporterDescriptionDescriptor.Staff.GetStructuredCodeValue(),
                            ReporterName = staffReporter.Name.AsString(),
                            Behavior = new[] { new Behavior { Behavior1 = behaviorType } },
                            ReportedToLawEnforcement = isSeriousOffense,
                            ReportedToLawEnforcementSpecified = true,
                            SchoolReference = Configuration.SchoolProfile.GetSchoolReference(),
                            StaffReference = staffReporter.GetStaffReference()
                        };

                        ++_disciplineEventId;
                        disciplineEvents.Add(disciplineEvent);
                    }
                } while (eventDateOffset < schoolDaysInPeriod);
            }

            context.GeneratedStudentData.StudentDisciplineData.DisciplineIncidents.AddRange(disciplineEvents);
        }
    }
}
