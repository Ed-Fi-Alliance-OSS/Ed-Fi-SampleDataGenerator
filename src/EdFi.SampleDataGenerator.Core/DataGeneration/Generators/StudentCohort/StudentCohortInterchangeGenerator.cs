using EdFi.SampleDataGenerator.Core.DataGeneration.Common;
using EdFi.SampleDataGenerator.Core.DataGeneration.InterchangeEntities;

namespace EdFi.SampleDataGenerator.Core.DataGeneration.Generators.StudentCohort
{
    public class StudentCohortInterchangeGenerator : StudentDataGenerator
    {
        public override InterchangeEntity InterchangeEntity => InterchangeEntity.StudentCohort;
        protected static GeneratorFactoryDelegate GeneratorFactory = randomNumberGenerator => DefaultGeneratorFactory<StudentCohortEntityGenerator>(randomNumberGenerator);

        public StudentCohortInterchangeGenerator(IRandomNumberGenerator randomNumberGenerator) : base(randomNumberGenerator, GeneratorFactory)
        {
        }
    }

    public class StudentCohortInterchangeGlobalDataGenerator : GlobalDataGenerator
    {
        public override InterchangeEntity InterchangeEntity => InterchangeEntity.StudentCohort;
        protected static GeneratorFactoryDelegate GeneratorFactory = randomNumberGenerator => DefaultGeneratorFactory<StudentCohortEntityGlobalGenerator>(randomNumberGenerator);

        public StudentCohortInterchangeGlobalDataGenerator(IRandomNumberGenerator randomNumberGenerator) : base(randomNumberGenerator, GeneratorFactory)
        {
        }
    }
}
