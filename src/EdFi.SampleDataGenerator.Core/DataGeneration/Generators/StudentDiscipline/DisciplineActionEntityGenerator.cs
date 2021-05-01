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
    public class DisciplineActionEntityGenerator : StudentDisciplineEntityGenerator
    {
        private static int _disciplineActionEntityId = 1;

        public override IEntity GeneratesEntity => StudentDisciplineEntity.DisciplineAction;
        public override IEntity[] DependsOnEntities => EntityDependencies.Create(StudentEntity.Student, StudentDisciplineEntity.DisciplineIncident, StudentDisciplineEntity.StudentDisciplineIncidentAssociation);

        private readonly DisciplineActionOption[] _seriousIncidentDisciplineActionOptions =
        {
            new DisciplineActionOption { DisciplineType = DisciplineDescriptor.Expulsion, MinLength = 0, MaxLength = 1, Probability = 1}
        };

        private readonly DisciplineActionOption[] _normalIncidentDisciplineActionOptions =
        {
            new DisciplineActionOption { DisciplineType = DisciplineDescriptor.InSchoolSuspension, MinLength = 1, MaxLength = 5, Probability = 0.10},
            new DisciplineActionOption { DisciplineType = DisciplineDescriptor.OutOfSchoolSuspension, MinLength = 5, MaxLength = 10, Probability = 0.05},
            new DisciplineActionOption { DisciplineType = DisciplineDescriptor.RemovalFromClassroom, MinLength = 1, MaxLength = 1, Probability = 0.85},
        };

        public DisciplineActionEntityGenerator(IRandomNumberGenerator randomNumberGenerator) : base(randomNumberGenerator)
        {
        }

        public override void GenerateAdditiveData(StudentDataGeneratorContext context, IDataPeriod dataPeriod)
        {
            var disciplineActions = new List<DisciplineAction>();

            var dataPeriodEnrollmentDateRange = dataPeriod.Intersect(context.EnrollmentDateRange);
            var disciplineIncidentsThisDataPeriod =
                context.GeneratedStudentData.StudentDisciplineData.DisciplineIncidents.Where(i => dataPeriodEnrollmentDateRange.Contains(i.IncidentDate));

            foreach (var disciplineIncident in disciplineIncidentsThisDataPeriod)
            {
                var disciplineOption = disciplineIncident.ReportedToLawEnforcement
                    ? _seriousIncidentDisciplineActionOptions.GetRandomItem(RandomNumberGenerator)
                    : _normalIncidentDisciplineActionOptions.GetRandomItemWithDistribution(i => i.Probability, RandomNumberGenerator);

                var disciplineLength = RandomNumberGenerator.Generate(disciplineOption.MinLength, disciplineOption.MaxLength);

                var disciplineDate = disciplineIncident.IncidentDate;

                var disciplineAction = new DisciplineAction
                {
                    id = $"DSCActionID_{_disciplineActionEntityId}_{disciplineIncident.id}",
                    DisciplineActionIdentifier = $"{_disciplineActionEntityId}",
                    Discipline = new[] { disciplineOption.DisciplineType.GetStructuredCodeValue() },
                    DisciplineDate = disciplineDate,
                    DisciplineActionLength = disciplineLength,
                    DisciplineActionLengthSpecified = true,
                    ActualDisciplineActionLength = disciplineLength,
                    ActualDisciplineActionLengthSpecified = true,
                    DisciplineActionLengthDifferenceReason = DisciplineActionLengthDifferenceReasonDescriptor.NoDifference.GetStructuredCodeValue(),
                    StudentReference = context.Student.GetStudentReference(),
                    ResponsibilitySchoolReference = Configuration.SchoolProfile.GetSchoolReference(),
                    StudentDisciplineIncidentAssociationReference = GetStudentDisciplineIncidentAssociationReference(context, disciplineIncident)
                };

                ++_disciplineActionEntityId;

                disciplineActions.Add(disciplineAction);
            }
            context.GeneratedStudentData.StudentDisciplineData.DisciplineActions.AddRange(disciplineActions);
        }

        private StudentDisciplineIncidentAssociationReferenceType[] GetStudentDisciplineIncidentAssociationReference(StudentDataGeneratorContext context, DisciplineIncident disciplineIncident)
        {
            return new[]
            {
                new StudentDisciplineIncidentAssociationReferenceType
                {
                    StudentDisciplineIncidentAssociationIdentity = new StudentDisciplineIncidentAssociationIdentityType
                    {
                        DisciplineIncidentReference = disciplineIncident.GetDisciplineIncidentReference(),
                        StudentReference = context.Student.GetStudentReference()
                    }
                }
            };
        }

        private class DisciplineActionOption
        {
            public DisciplineDescriptor DisciplineType { get; set; }
            public double Probability { get; set; }
            public int MinLength { get; set; }
            public int MaxLength { get; set; }
        }
    }
}