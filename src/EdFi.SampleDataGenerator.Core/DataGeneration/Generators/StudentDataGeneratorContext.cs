using EdFi.SampleDataGenerator.Core.Config.SeedData;
using EdFi.SampleDataGenerator.Core.Date;
using EdFi.SampleDataGenerator.Core.Serialization.Output;

namespace EdFi.SampleDataGenerator.Core.DataGeneration.Generators
{
    public class StudentDataGeneratorContext : IHasSeedRecord
    {
        public int GlobalStudentNumber { get; set; }
        public GeneratedStudentData GeneratedStudentData { get; set; }
        public StudentPerformanceProfile StudentPerformanceProfile { get; set; }
        public StudentCharacteristics StudentCharacteristics { get; set; }

        public SeedRecord SeedRecord { get; set; }
        public bool HasSeedRecord => SeedRecord != null;

        public DateRange EnrollmentDateRange { get; set; }

        //reference shortcut since the student entity is used so frequently
        public Entities.Student Student
        {
            get { return GeneratedStudentData.StudentData.Student;}
            set { GeneratedStudentData.StudentData.Student = value; }
        }
    }
}