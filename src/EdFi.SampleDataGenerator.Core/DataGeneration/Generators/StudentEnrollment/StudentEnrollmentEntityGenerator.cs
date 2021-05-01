using EdFi.SampleDataGenerator.Core.DataGeneration.Common;

namespace EdFi.SampleDataGenerator.Core.DataGeneration.Generators.StudentEnrollment
{
    public abstract class StudentEnrollmentEntityGenerator : StudentDataInterchangeEntityGenerator
    {
        protected StudentEnrollmentEntityGenerator(IRandomNumberGenerator randomNumberGenerator) : base(randomNumberGenerator)
        {
        }
    }
}
