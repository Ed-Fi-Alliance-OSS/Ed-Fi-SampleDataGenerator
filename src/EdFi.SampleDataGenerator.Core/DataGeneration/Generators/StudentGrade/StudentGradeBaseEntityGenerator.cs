using EdFi.SampleDataGenerator.Core.DataGeneration.Common;

namespace EdFi.SampleDataGenerator.Core.DataGeneration.Generators.StudentGrade
{
    public abstract class StudentGradeBaseEntityGenerator : StudentDataInterchangeEntityGenerator
    {
        protected StudentGradeBaseEntityGenerator(IRandomNumberGenerator randomNumberGenerator) : base(randomNumberGenerator)
        {
        }
    }
}
