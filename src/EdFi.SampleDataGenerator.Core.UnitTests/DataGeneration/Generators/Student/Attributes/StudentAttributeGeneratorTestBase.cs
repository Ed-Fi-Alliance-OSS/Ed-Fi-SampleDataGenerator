using System.Collections.Generic;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common;
using EdFi.SampleDataGenerator.Core.DataGeneration.Generators;
using EdFi.SampleDataGenerator.Core.Entities;
using EdFi.SampleDataGenerator.Core.Serialization.Output;
using EdFi.SampleDataGenerator.Core.Serialization.Output.Interchanges;
using EdFi.SampleDataGenerator.Core.UnitTests.Config;

namespace EdFi.SampleDataGenerator.Core.UnitTests.DataGeneration.Generators.Student.Attributes
{
    public abstract class StudentAttributeGeneratorTestBase
    {
        public StudentDataGeneratorContext DefaultStudentGenerationContext => 
            new StudentDataGeneratorContext
            {
                GlobalStudentNumber = 1
            };

        public StudentDataGeneratorContext DefaultStudentEntityAttributeGenerationContext => BuildContext(new Entities.Student());

        public static StudentDataGeneratorContext BuildContext(Entities.Student student)
        {
            return new StudentDataGeneratorContext
            {
                GeneratedStudentData = new GeneratedStudentData
                {
                    StudentData = new StudentData
                    {
                        Student = student
                    }, StudentEnrollmentData = new StudentEnrollmentData
                    {
                        StudentEducationOrganizationAssociation = new List<StudentEducationOrganizationAssociation>
                        {
                            new StudentEducationOrganizationAssociation()
                            {
                                 StudentCharacteristic = new StudentCharacteristic[0],
                                 StudentIndicator = new StudentIndicator[0]
                            }
                        }
                    }
                },
                GlobalStudentNumber = 1,
                StudentCharacteristics = new StudentCharacteristics()
            };
        }

        public StudentDataGeneratorConfig DefaultStudentDataGeneratorConfig => new StudentDataGeneratorConfig
        {
            GlobalConfig = new TestSampleDataGeneratorConfig
            {
                OutputMode = OutputMode.Standard
            }
        };
    }
}
