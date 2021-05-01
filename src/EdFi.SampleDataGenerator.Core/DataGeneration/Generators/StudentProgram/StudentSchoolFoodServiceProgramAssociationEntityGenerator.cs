using EdFi.SampleDataGenerator.Core.DataGeneration.Common;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common.Dependencies;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common.Entity;
using EdFi.SampleDataGenerator.Core.DataGeneration.InterchangeEntities;
using EdFi.SampleDataGenerator.Core.Entities;
using EdFi.SampleDataGenerator.Core.Helpers;
using EdFi.SampleDataGenerator.Core.Config;
using EdFi.SampleDataGenerator.Core.DataGeneration.Generators.Student;

namespace EdFi.SampleDataGenerator.Core.DataGeneration.Generators.StudentProgram
{
    public class StudentSchoolFoodServiceProgramAssociationEntityGenerator : StudentProgramEntityGenerator
    {
        public override IEntity GeneratesEntity => StudentProgramEntity.StudentSchoolFoodServiceProgramAssociation;
        public override IEntity[] DependsOnEntities => EntityDependencies.Create(StudentProgramEntity.StudentProgramAssociation);

        public StudentSchoolFoodServiceProgramAssociationEntityGenerator(IRandomNumberGenerator randomNumberGenerator) : base(randomNumberGenerator)
        {
        }

        public override void GenerateAdditiveData(StudentDataGeneratorContext context, IDataPeriod dataPeriod)
        {
            if (!context.StudentCharacteristics.IsFoodServiceEligible) return;
            
            if (StudentProgramHelpers.StudentBecameEnrolledDuringDataPeriod(context, dataPeriod))
            {
                var student = context.Student;

                var studentProgramAssociation =  new StudentSchoolFoodServiceProgramAssociation()
                {
                    StudentReference = student.GetStudentReference(),
                    BeginDate = context.EnrollmentDateRange.StartDate,
                    EndDateSpecified = false,
                    EducationOrganizationReference = Configuration.GetEducationOrganizationReference(),
                    SchoolFoodServiceProgramService = new[]
                    {
                        new SchoolFoodServiceProgramService
                        {
                            SchoolFoodServiceProgramService1 = context.StudentCharacteristics.FoodServiceElected?.GetStructuredCodeValue(),
                            ServiceBeginDate = context.EnrollmentDateRange.StartDate,
                            ServiceBeginDateSpecified = true ,
                            ServiceEndDateSpecified = false
                        } 
                    },
                    ProgramReference = Configuration.GetProgram(ProgramTypeDescriptor.Other).GetProgramReference()
                };

                context.GeneratedStudentData.StudentProgramData.StudentSchoolFoodServiceProgramAssociations.Add(studentProgramAssociation);
            }

            if (StudentProgramHelpers.StudentBecameUnenrolledDuringDataPeriod(context, dataPeriod) &&
                StudentProgramHelpers.StudentBecameUnenrolledBeforeEndOfSchoolCalendar(context, Configuration))
            {
                foreach (var studentProgramAssociation in context.GeneratedStudentData.StudentProgramData.StudentSchoolFoodServiceProgramAssociations)
                {
                    studentProgramAssociation.EndDate = context.EnrollmentDateRange.EndDate;
                    studentProgramAssociation.EndDateSpecified = true;
                    foreach (var foodProgram in studentProgramAssociation.SchoolFoodServiceProgramService )
                    {
                        foodProgram.ServiceEndDate = context.EnrollmentDateRange.EndDate;
                        foodProgram.ServiceEndDateSpecified = true;
                    }
                }
            }
        }
    }
}
