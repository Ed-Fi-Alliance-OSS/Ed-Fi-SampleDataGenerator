using EdFi.SampleDataGenerator.Core.DataGeneration.Common;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common.Dependencies;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common.Entity;
using EdFi.SampleDataGenerator.Core.DataGeneration.Generators.Student;
using EdFi.SampleDataGenerator.Core.DataGeneration.InterchangeEntities;
using EdFi.SampleDataGenerator.Core.Entities;
using EdFi.SampleDataGenerator.Core.Helpers;
using System.Collections.Generic;
using EdFi.SampleDataGenerator.Core.Config;

namespace EdFi.SampleDataGenerator.Core.DataGeneration.Generators.StudentProgram
{
    public class StudentProgramAssociationEntityGenerator : StudentProgramEntityGenerator
    {
        public override IEntity GeneratesEntity => StudentProgramEntity.StudentProgramAssociation;
        public override IEntity[] DependsOnEntities => EntityDependencies.Create(StudentEntity.Student);

        public StudentProgramAssociationEntityGenerator(IRandomNumberGenerator randomNumberGenerator) : base(randomNumberGenerator)
        {
        }

        public override void GenerateAdditiveData(StudentDataGeneratorContext context, IDataPeriod dataPeriod)
        {
            if (StudentProgramHelpers.StudentBecameEnrolledDuringDataPeriod(context, dataPeriod))
            {
                var studentProgramAssociation = new List<StudentProgramAssociation>();

                if (context.GetStudentEducationOrganization().GetPrimaryLanguage().GetStructuredCodeValue() != LanguageDescriptor.English_eng.GetStructuredCodeValue())
                {
                    var program = Configuration.GetProgram(ProgramTypeDescriptor.EnglishAsASecondLanguageESL);

                    studentProgramAssociation.Add(CreateStudentProgramAssociationEntity(context, program));
                }

                if (context.StudentPerformanceProfile.IsEligibleForGiftedAndTalentedProgram(Configuration))
                {
                    var program = Configuration.GetProgram(ProgramTypeDescriptor.GiftedAndTalented);

                    if (program != null)
                    {
                        studentProgramAssociation.Add(CreateStudentProgramAssociationEntity(context, program));
                    }
                }

                context.GeneratedStudentData.StudentProgramData.StudentProgramAssociations.AddRange(studentProgramAssociation);
            }

            if (StudentProgramHelpers.StudentBecameUnenrolledDuringDataPeriod(context, dataPeriod) &&
                StudentProgramHelpers.StudentBecameUnenrolledBeforeEndOfSchoolCalendar(context, Configuration))
            {
                foreach (var studentProgramAssociation in context.GeneratedStudentData.StudentProgramData.StudentProgramAssociations)
                {
                    studentProgramAssociation.EndDate = context.EnrollmentDateRange.EndDate;
                    studentProgramAssociation.EndDateSpecified = true;
                }
            }
        }

        private StudentProgramAssociation CreateStudentProgramAssociationEntity(StudentDataGeneratorContext context, Program program)
        {
            var student = context.Student;

            return new StudentProgramAssociation
            {
                StudentReference = student.GetStudentReference(),
                BeginDate = context.EnrollmentDateRange.StartDate,
                EndDateSpecified = false,
                EducationOrganizationReference = Configuration.GetEducationOrganizationReference(),
                ProgramReference = program.GetProgramReference()
            };
        }
    }
}
