using EdFi.SampleDataGenerator.Core.DataGeneration.Common;
using EdFi.SampleDataGenerator.Core.DataGeneration.InterchangeEntities;

namespace EdFi.SampleDataGenerator.Core.DataGeneration.Generators.StudentDiscipline
{
    public class StudentDisciplineInterchangeGenerator : StudentDataGenerator
    {
        protected static GeneratorFactoryDelegate GeneratorFactory = randomNumberGenerator => DefaultGeneratorFactory<StudentDisciplineEntityGenerator>(randomNumberGenerator);

        public override InterchangeEntity InterchangeEntity => InterchangeEntity.StudentDiscipline;

        public StudentDisciplineInterchangeGenerator(IRandomNumberGenerator randomNumberGenerator) : base(randomNumberGenerator, GeneratorFactory)
        {
        }
    }
}
