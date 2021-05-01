using EdFi.SampleDataGenerator.Core.DataGeneration.Common;

namespace EdFi.SampleDataGenerator.Core.DataGeneration.Generators.StudentProgram
{
    public abstract class StudentProgramEntityGenerator : StudentDataInterchangeEntityGenerator
    {
        protected StudentProgramEntityGenerator(IRandomNumberGenerator randomNumberGenerator) : base(randomNumberGenerator)
        {
        }
    }
}
