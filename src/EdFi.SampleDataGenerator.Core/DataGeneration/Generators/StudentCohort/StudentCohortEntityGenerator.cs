using EdFi.SampleDataGenerator.Core.DataGeneration.Common;

namespace EdFi.SampleDataGenerator.Core.DataGeneration.Generators.StudentCohort
{
    public abstract class StudentCohortEntityGenerator : StudentDataInterchangeEntityGenerator
    {
        protected StudentCohortEntityGenerator(IRandomNumberGenerator randomNumberGenerator) : base(randomNumberGenerator)
        {
        }
    }

    public abstract class StudentCohortEntityGlobalGenerator : GlobalDataInterchangeEntityGenerator
    {
        protected StudentCohortEntityGlobalGenerator(IRandomNumberGenerator randomNumberGenerator) : base(randomNumberGenerator)
        {
        }
    }
}
