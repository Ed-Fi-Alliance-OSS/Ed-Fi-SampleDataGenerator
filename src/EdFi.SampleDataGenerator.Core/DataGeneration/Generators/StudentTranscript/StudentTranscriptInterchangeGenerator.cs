using EdFi.SampleDataGenerator.Core.DataGeneration.Common;
using EdFi.SampleDataGenerator.Core.DataGeneration.InterchangeEntities;

namespace EdFi.SampleDataGenerator.Core.DataGeneration.Generators.StudentTranscript
{
    public class StudentTranscriptInterchangeGenerator : StudentDataGenerator
    {
        protected static GeneratorFactoryDelegate GeneratorFactory = randomNumberGenerator => DefaultGeneratorFactory<StudentTranscriptEntityGenerator>(randomNumberGenerator);

        public override InterchangeEntity InterchangeEntity => InterchangeEntity.StudentTranscript;

        public StudentTranscriptInterchangeGenerator() : this(new RandomNumberGenerator())
        {
        }

        public StudentTranscriptInterchangeGenerator(IRandomNumberGenerator randomNumberGenerator) : base(randomNumberGenerator, GeneratorFactory)
        {
        }
    }
}
