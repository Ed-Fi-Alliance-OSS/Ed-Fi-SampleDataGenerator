using EdFi.SampleDataGenerator.Core.DataGeneration.Common;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common.Dependencies;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common.Entity;
using EdFi.SampleDataGenerator.Core.DataGeneration.InterchangeEntities;
using EdFi.SampleDataGenerator.Core.Entities;
using EdFi.SampleDataGenerator.Core.Helpers;
 
namespace EdFi.SampleDataGenerator.Core.DataGeneration.Generators.StudentEnrollment
{
    public class StudentTransportationEntityGenerator : StudentEnrollmentEntityGenerator
    {
        public StudentTransportationEntityGenerator(IRandomNumberGenerator randomNumberGenerator) : base(randomNumberGenerator)
        {
        }
 
        private const double RidesBusChance = 0.55;
 
        public override IEntity GeneratesEntity => StudentEnrollmentEntity.StudentTransportation;
        public override IEntity[] DependsOnEntities => EntityDependencies.Create(StudentEntity.Student);
 
        protected override void GenerateCore(StudentDataGeneratorContext context)
        {
            var ridesBus = RandomNumberGenerator.GetValueWithProbability(RidesBusChance, true, false);
 
            if (ridesBus)
            {
                context
                    .GeneratedStudentData
                    .StudentEnrollmentData
                    .StudentTransportations
                    .Add(new EXTENSIONStudentTransportation
                    {
                        StudentReference = context.Student.GetStudentReference(),
                        SchoolReference = Configuration.SchoolProfile.GetSchoolReference(),
                        AMBusNumber = GenerateBusNumber(),
                        PMBusNumber = GenerateBusNumber(),
                        EstimatedMilesFromSchool = GenerateEstimatedMiles()
                    });
            }
        }
 
        private string GenerateBusNumber()
        {
            return RandomNumberGenerator.Generate(100, 999999).ToString();
        }
 
        private decimal GenerateEstimatedMiles()
        {
            return decimal.Parse(
                RandomNumberGenerator.Generate(0, 900)
                + "." +
                RandomNumberGenerator.Generate(10, 99));
        }
    }
}