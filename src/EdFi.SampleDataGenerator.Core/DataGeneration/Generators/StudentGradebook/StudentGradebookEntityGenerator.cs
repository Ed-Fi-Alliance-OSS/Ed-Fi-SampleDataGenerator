using EdFi.SampleDataGenerator.Core.DataGeneration.Common;

namespace EdFi.SampleDataGenerator.Core.DataGeneration.Generators.StudentGradebook
{
    public abstract class StudentGradebookEntityGenerator : StudentDataInterchangeEntityGenerator
    {
        protected StudentGradebookEntityGenerator(IRandomNumberGenerator randomNumberGenerator) : base(randomNumberGenerator)
        {
        }
    }

    public abstract class StudentGradebookEntityGlobalGenerator : GlobalDataInterchangeEntityGenerator
    {
        protected StudentGradebookEntityGlobalGenerator(IRandomNumberGenerator randomNumberGenerator) : base(randomNumberGenerator)
        {
        }
    }
}
