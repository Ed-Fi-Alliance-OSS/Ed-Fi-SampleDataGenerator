using EdFi.SampleDataGenerator.Core.DataGeneration.Common;

namespace EdFi.SampleDataGenerator.Core.DataGeneration.Generators.StudentTranscript
{
    public abstract class StudentTranscriptEntityGenerator : StudentDataInterchangeEntityGenerator
    {
        protected StudentTranscriptEntityGenerator(IRandomNumberGenerator randomNumberGenerator) : base(randomNumberGenerator)
        {
        }
    }
}
