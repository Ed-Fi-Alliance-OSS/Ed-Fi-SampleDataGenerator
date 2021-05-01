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

namespace EdFi.SampleDataGenerator.Core.DataGeneration.Generators.StudentEnrollment
{
    public sealed class StudentSchoolAssociationEntityGenerator : StudentEnrollmentEntityGenerator
    {
        public StudentSchoolAssociationEntityGenerator(IRandomNumberGenerator randomNumberGenerator) : base(randomNumberGenerator)
        {
        }

        public override IEntity GeneratesEntity => StudentEnrollmentEntity.StudentSchoolAssociation;
        public override IEntity[] DependsOnEntities => EntityDependencies.Create(StudentEntity.Student);

        protected override void GenerateFromSeedData(StudentDataGeneratorContext context)
        {
            //since the Seed record school Id will also match the id in the
            //context.SchoolProfile, we'll just call GenerateCore to populate this value
            GenerateCore(context);
        }

        private List<GraduationPlan> _graduationPlans;
        protected override void OnConfigure()
        {
            var graduationYear = Configuration.GradeProfile.GetGraduationYear(Configuration.SchoolProfile, Configuration.GlobalConfig.TimeConfig.SchoolCalendarConfig.SchoolYear());

            _graduationPlans = Configuration.GlobalData.GraduationPlans
                .GetGraduationPlans(Configuration.SchoolProfile, graduationYear)
                .OrderBy(gp => gp.TotalRequiredCredits?.Credits1)
                .ToList();
        }

        public override void GenerateAdditiveData(StudentDataGeneratorContext context, IDataPeriod dataPeriod)
        {
            var dataPeriodDateRange = dataPeriod.AsDateRange();
            if (context.EnrollmentDateRange.StartsIn(dataPeriodDateRange))
            {
                var studentSchoolAssociation = GetEnrollmentRecord(context);
                context.GeneratedStudentData.StudentEnrollmentData.StudentSchoolAssociation = studentSchoolAssociation;
            }

            if (context.EnrollmentDateRange.EndsIn(dataPeriodDateRange))
            {
                context.GeneratedStudentData.StudentEnrollmentData.StudentSchoolAssociation.ExitWithdrawDate = context.EnrollmentDateRange.EndDate;
                context.GeneratedStudentData.StudentEnrollmentData.StudentSchoolAssociation.ExitWithdrawDateSpecified = true;
            }
        }

        private StudentSchoolAssociation GetEnrollmentRecord(StudentDataGeneratorContext context)
        {
            var student = context.Student;
            var gradeLevel = Configuration.GradeProfile.GetGradeLevel();
            var schoolCalendarConfig = Configuration.GlobalConfig.TimeConfig.SchoolCalendarConfig;
            var graduationYear = Configuration.GradeProfile.GetGraduationYear(Configuration.SchoolProfile,
                schoolCalendarConfig.SchoolYear());
            var schoolReference = Configuration.SchoolProfile.GetSchoolReference();

            var graduationPlanType = GetStudentGraduationPlan(context);

            var studentSchoolAssociation = new StudentSchoolAssociation
            {
                StudentReference = student.GetStudentReference(),
                SchoolReference = schoolReference,
                EntryDate = context.EnrollmentDateRange.StartDate,
                EntryGradeLevel = gradeLevel.GetStructuredCodeValue(),
                GraduationPlanReference = new GraduationPlanReferenceType
                {
                    GraduationPlanIdentity = new GraduationPlanIdentityType
                    {
                        EducationOrganizationReference = Configuration.SchoolProfile.GetEducationOrganizationReference(),
                        GraduationPlanType = graduationPlanType,
                        GraduationSchoolYear = graduationYear
                    }
                }
            };

            return studentSchoolAssociation;
        }

        private string GetStudentGraduationPlan(StudentDataGeneratorContext context)
        {
            var totalPlans = _graduationPlans.Count;
            var graduationPlanIndex = Math.Min((int) (context.StudentPerformanceProfile.PerformanceIndex * totalPlans), totalPlans - 1);

            return _graduationPlans[graduationPlanIndex].GraduationPlanType;
        }
    }
}
