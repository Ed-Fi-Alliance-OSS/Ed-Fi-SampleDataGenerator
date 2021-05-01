using EdFi.SampleDataGenerator.Core.DataGeneration.Common;
using EdFi.SampleDataGenerator.Core.DataGeneration.InterchangeEntities;

namespace EdFi.SampleDataGenerator.Core.DataGeneration.Generators.StudentEnrollment
{
    public class StudentEnrollmentInterchangeGenerator : StudentDataGenerator
    {
        public override InterchangeEntity InterchangeEntity => InterchangeEntity.StudentEnrollment;
        protected static GeneratorFactoryDelegate GeneratorFactory = randomNumberGenerator => DefaultGeneratorFactory<StudentEnrollmentEntityGenerator>(randomNumberGenerator);

        public StudentEnrollmentInterchangeGenerator() : this(new RandomNumberGenerator())
        {
        }

        public StudentEnrollmentInterchangeGenerator(IRandomNumberGenerator randomNumberGenerator) : base(randomNumberGenerator, GeneratorFactory)
        {
        }
    }
}
