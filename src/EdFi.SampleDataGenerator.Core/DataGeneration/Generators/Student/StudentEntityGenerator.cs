using EdFi.SampleDataGenerator.Core.DataGeneration.Common;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common.Dependencies;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common.Entity;
using EdFi.SampleDataGenerator.Core.DataGeneration.InterchangeEntities;

namespace EdFi.SampleDataGenerator.Core.DataGeneration.Generators.Student
{
    public sealed class StudentEntityGenerator : StudentDataInterchangeEntityGenerator
    {
        public StudentEntityGenerator(IRandomNumberGenerator randomNumberGenerator) : base(randomNumberGenerator)
        {
        }

        public override IEntity GeneratesEntity => StudentEntity.Student;
        public override IEntity[] DependsOnEntities => EntityDependencies.None;

        protected override void GenerateFromSeedData(StudentDataGeneratorContext context)
        {
            context.StudentPerformanceProfile.PerformanceIndex = context.SeedRecord.PerformanceIndex;
        }

        protected override void GenerateCore(StudentDataGeneratorContext context)
        {
            GenerateStudentPerformanceProfile(context.StudentPerformanceProfile);
        }

        private void GenerateStudentPerformanceProfile(StudentPerformanceProfile profile)
        {
            profile.PerformanceIndex = StudentPerformanceProfileDistribution.GenerateStudentPerformanceProfile(RandomNumberGenerator);
        }
    }
}
