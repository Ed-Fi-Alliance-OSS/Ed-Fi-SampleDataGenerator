using EdFi.SampleDataGenerator.Core.DataGeneration.Common;
using EdFi.SampleDataGenerator.Core.DataGeneration.InterchangeEntities;

namespace EdFi.SampleDataGenerator.Core.DataGeneration.Generators.StudentAttendance
{
    public class StudentAttendanceInterchangeGenerator : StudentDataGenerator
    {
        public override InterchangeEntity InterchangeEntity => InterchangeEntity.StudentAttendance;

        private static readonly GeneratorFactoryDelegate GeneratorFactory = rng => DefaultGeneratorFactory<StudentAttendanceEntityGenerator>(rng);

        public StudentAttendanceInterchangeGenerator(IRandomNumberGenerator randomNumberGenerator) : base(randomNumberGenerator, GeneratorFactory)
        {
        }
    }
}
