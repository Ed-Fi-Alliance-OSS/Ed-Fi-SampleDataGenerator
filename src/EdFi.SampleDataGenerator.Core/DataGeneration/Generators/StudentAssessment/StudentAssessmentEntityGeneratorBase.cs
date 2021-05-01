using EdFi.SampleDataGenerator.Core.DataGeneration.Common;

namespace EdFi.SampleDataGenerator.Core.DataGeneration.Generators.StudentAssessment
{
    public abstract class StudentAssessmentEntityGeneratorBase : StudentDataInterchangeEntityGenerator
    {
        protected StudentAssessmentEntityGeneratorBase(IRandomNumberGenerator randomNumberGenerator) : base(randomNumberGenerator)
        {
        }
    }
}
