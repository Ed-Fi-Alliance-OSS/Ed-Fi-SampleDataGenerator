using System.Collections.Generic;
using System.Linq;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common.Dependencies;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common.Entity;
using EdFi.SampleDataGenerator.Core.DataGeneration.InterchangeEntities;
using EdFi.SampleDataGenerator.Core.Entities;
using EdFi.SampleDataGenerator.Core.Helpers;

namespace EdFi.SampleDataGenerator.Core.DataGeneration.Generators.StudentCohort
{
    public class StudentCohortAssociationEntityGenerator : StudentCohortEntityGenerator
    {
        private const double AttendanceInterventionAbsenceRateThreshold = 0.1;
        private int _numberOfInstructionalDays;

        public override IEntity GeneratesEntity => StudentCohortEntity.StudentCohortAssociation;
        public override IEntity[] DependsOnEntities => EntityDependencies.Create(StudentEnrollmentEntity.StudentSectionAssociation, StudentGradeEntity.Grade, StudentDisciplineEntity.StudentDisciplineIncidentAssociation, StudentAttendanceEntity.StudentSectionAttendanceEvent);

        public StudentCohortAssociationEntityGenerator(IRandomNumberGenerator randomNumberGenerator) : base(randomNumberGenerator)
        {
        }

        protected override void OnConfigure()
        {
            _numberOfInstructionalDays = Configuration.GetSchoolInstructionalDays().Count();
        }

        protected override void GenerateCore(StudentDataGeneratorContext context)
        {
            var studentCohortAssociations = CreateStudentCohortAssociations(context).Where(a => a != null);
            context.GeneratedStudentData.StudentCohortData.StudentCohortAssociations = studentCohortAssociations.ToArray();
        }

        private IEnumerable<StudentCohortAssociation> CreateStudentCohortAssociations(StudentDataGeneratorContext context)
        {
            var cohortsBySchool = Configuration.SchoolProfile.GetEducationOrganizationReference().GetCohortsByEducationOrganizationReference(Configuration.GlobalData.CohortData.Cohorts).ToList();

            yield return AcademicInterventionStudentCohortAssociation(context, cohortsBySchool);
            yield return DisciplineInterventionStudentCohortAssociation(context, cohortsBySchool);
            yield return AttendanceInterventionStudentCohortAssociation(context, cohortsBySchool);
        }

        private StudentCohortAssociation AcademicInterventionStudentCohortAssociation(StudentDataGeneratorContext context, IEnumerable<Cohort> cohorts)
        {
            var studentGrades = context.GeneratedStudentData.StudentGradeData.Grades;
            var academicInterventionCohort = cohorts.FirstOrDefault(x => x.CohortType == CohortTypeDescriptor.AcademicIntervention.GetStructuredCodeValue());

            var performanceForAcademicIntervention = new []
            {
                PerformanceBaseConversionDescriptor.BelowBasic.GetStructuredCodeValue(),
                PerformanceBaseConversionDescriptor.WellBelowBasic.GetStructuredCodeValue(),
                PerformanceBaseConversionDescriptor.Fail.GetStructuredCodeValue(),
            };

            var letterGradesForAcademicIntervention = new []
            {
                LetterGrade.D.Grade,
                LetterGrade.F.Grade
            };

            var maximumNumericGradeForAcademicIntervention = LetterGrade.D.MaxNumericGrade;

            if (academicInterventionCohort != null &&
                studentGrades.Any(
                    x =>
                        performanceForAcademicIntervention.Contains(x.PerformanceBaseConversion) ||
                        letterGradesForAcademicIntervention.Contains(x.LetterGradeEarned) ||
                        (x.NumericGradeEarnedSpecified && x.NumericGradeEarned <= maximumNumericGradeForAcademicIntervention)))
            {
                return CreateStudentCohortAssociation(academicInterventionCohort, context.Student);
            }

            return null;
        }

        private StudentCohortAssociation DisciplineInterventionStudentCohortAssociation(StudentDataGeneratorContext context, IEnumerable<Cohort> cohorts)
        {
            var studentDisciplineIncidentAssociations = context.GeneratedStudentData.StudentDisciplineData.StudentDisciplineIncidentAssociations;
            var disciplineInterventionCohort = cohorts.FirstOrDefault(x => x.CohortType == CohortTypeDescriptor.DisciplineIntervention.GetStructuredCodeValue());

            if (disciplineInterventionCohort != null &&
                studentDisciplineIncidentAssociations.Any(
                    x =>
                        DisciplineHelpers.PerpetratorCodeDescriptors.ToStructuredCodeValueFormatArray().Contains(x.StudentParticipationCode) &&
                         x.Behavior != null && x.Behavior.Any(behavior => DisciplineHelpers.SeriousBehaviors.ToStructuredCodeValueFormatArray().Contains(behavior.Behavior1))))
            {
                return CreateStudentCohortAssociation(disciplineInterventionCohort, context.Student);
            }

            return null;
        }

        private StudentCohortAssociation AttendanceInterventionStudentCohortAssociation(StudentDataGeneratorContext context, IEnumerable<Cohort> cohorts)
        {
            var studentSectionAttendanceEvents = context.GeneratedStudentData.StudentAttendanceData.StudentSectionAttendanceEvents;
            var studentSectionAssociations = context.GeneratedStudentData.StudentEnrollmentData.StudentSectionAssociations;
            var attendanceInterventionCohort = cohorts.FirstOrDefault(x => x.CohortType == CohortTypeDescriptor.AttendanceIntervention.GetStructuredCodeValue());
            
            var numberOfAbsenceEvents =
                studentSectionAttendanceEvents.Count(
                    x => x.AttendanceEvent.AttendanceEventCategory.Is(AttendanceEventCategoryDescriptor.ExcusedAbsence) ||
                    x.AttendanceEvent.AttendanceEventCategory.Is(AttendanceEventCategoryDescriptor.UnexcusedAbsence));
            
            return (numberOfAbsenceEvents / (studentSectionAssociations.Count * _numberOfInstructionalDays * 1.0)) >= AttendanceInterventionAbsenceRateThreshold
                ? CreateStudentCohortAssociation(attendanceInterventionCohort, context.Student)
                : null;
        }

        private StudentCohortAssociation CreateStudentCohortAssociation(Cohort cohort, Entities.Student student)
        {
            return new StudentCohortAssociation
            {
                CohortReference = cohort.GetCohortReference(),
                StudentReference = student.GetStudentReference(),
                BeginDate = Configuration.GlobalConfig.TimeConfig.SchoolCalendarConfig.StartDate
            };
        }
    }
}
