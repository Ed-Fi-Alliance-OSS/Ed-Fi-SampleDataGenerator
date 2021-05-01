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

namespace EdFi.SampleDataGenerator.Core.DataGeneration.Generators.StudentDiscipline
{
    public class StudentDisciplineIncidentAssociationEntityGenerator : StudentDisciplineEntityGenerator
    {
        private const double MaxPerpetratorChance = 0.50;

        public override IEntity GeneratesEntity => StudentDisciplineEntity.StudentDisciplineIncidentAssociation;
        public override IEntity[] DependsOnEntities => EntityDependencies.Create(StudentEntity.Student, StudentDisciplineEntity.DisciplineIncident);

        public StudentDisciplineIncidentAssociationEntityGenerator(IRandomNumberGenerator randomNumberGenerator) : base(randomNumberGenerator)
        {
        }

        public override void GenerateAdditiveData(StudentDataGeneratorContext context, IDataPeriod dataPeriod)
        {
            var disciplineIncidentAssociations = new List<StudentDisciplineIncidentAssociation>();
            var student = context.Student;

            var dataPeriodEnrollmentDateRage = dataPeriod.Intersect(context.EnrollmentDateRange);
            var disciplineIncidentsThisDataPeriod =
                context.GeneratedStudentData.StudentDisciplineData.DisciplineIncidents.Where(i => dataPeriodEnrollmentDateRage.Contains(i.IncidentDate));

            foreach (var disciplineIncident in disciplineIncidentsThisDataPeriod)
            {
                var studentParticipationCodeType = StudentParticipationCodeDescriptor.Perpetrator;

                if (disciplineIncident.ReportedToLawEnforcement)
                {
                    //the idea here is the chance of a student being a perpetrator is inversely proprtional to their
                    //Student Performance Profile
                    var perpetratorChance = (1 - context.StudentPerformanceProfile.PerformanceIndex) * MaxPerpetratorChance;
                    var isPerpetrator = RandomNumberGenerator.GetRandomBool(perpetratorChance);

                    studentParticipationCodeType = isPerpetrator
                        ? DisciplineHelpers.PerpetratorCodeDescriptors.GetRandomItem(RandomNumberGenerator)
                        : DisciplineHelpers.NonPerpetratorCodeDescriptors.GetRandomItem(RandomNumberGenerator);
                }

                var disciplineIncidentAssociation = new StudentDisciplineIncidentAssociation
                {
                    StudentReference = student.GetStudentReference(),
                    StudentParticipationCode = studentParticipationCodeType.GetStructuredCodeValue(),
                    DisciplineIncidentReference = disciplineIncident.GetDisciplineIncidentReference()
                };

                disciplineIncidentAssociations.Add(disciplineIncidentAssociation);
            }

            context.GeneratedStudentData.StudentDisciplineData.StudentDisciplineIncidentAssociations.AddRange(disciplineIncidentAssociations);
        }
    }
}
