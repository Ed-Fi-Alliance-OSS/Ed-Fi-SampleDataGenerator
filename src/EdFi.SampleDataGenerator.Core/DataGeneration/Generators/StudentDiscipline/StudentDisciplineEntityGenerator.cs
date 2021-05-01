using EdFi.SampleDataGenerator.Core.DataGeneration.Common;

namespace EdFi.SampleDataGenerator.Core.DataGeneration.Generators.StudentDiscipline
{
    public abstract class StudentDisciplineEntityGenerator : StudentDataInterchangeEntityGenerator
    {
        protected StudentDisciplineEntityGenerator(IRandomNumberGenerator randomNumberGenerator) : base(randomNumberGenerator)
        {
        }
    }
}
