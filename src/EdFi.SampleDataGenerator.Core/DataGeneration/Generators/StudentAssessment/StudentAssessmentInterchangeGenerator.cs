using EdFi.SampleDataGenerator.Core.DataGeneration.Common;
using EdFi.SampleDataGenerator.Core.DataGeneration.InterchangeEntities;

namespace EdFi.SampleDataGenerator.Core.DataGeneration.Generators.StudentAssessment
{
    public class StudentAssessmentInterchangeGenerator : StudentDataGenerator
    {
        public override InterchangeEntity InterchangeEntity => InterchangeEntity.StudentAssessment;

        private static readonly GeneratorFactoryDelegate GeneratorFactory = rng => DefaultGeneratorFactory<StudentAssessmentEntityGeneratorBase>(rng);

        public StudentAssessmentInterchangeGenerator(IRandomNumberGenerator randomNumberGenerator) : base(randomNumberGenerator, GeneratorFactory)
        {
        }
    }
}
