using EdFi.SampleDataGenerator.Core.DataGeneration.Common;
using EdFi.SampleDataGenerator.Core.DataGeneration.InterchangeEntities;

namespace EdFi.SampleDataGenerator.Core.DataGeneration.Generators.Student
{
    public class StudentInterchangeGenerator : StudentDataGenerator
    {
        public static GeneratorFactoryDelegate GeneratorFactory => rng => new[] {new StudentEntityGenerator(rng)};

        public override InterchangeEntity InterchangeEntity => InterchangeEntity.Student;

        public StudentInterchangeGenerator(IRandomNumberGenerator randomNumberGenerator) : base(randomNumberGenerator, GeneratorFactory)
        {
        }
    }
}
