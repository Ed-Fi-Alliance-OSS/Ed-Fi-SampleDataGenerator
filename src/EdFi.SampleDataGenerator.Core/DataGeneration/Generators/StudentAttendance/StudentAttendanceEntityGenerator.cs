using EdFi.SampleDataGenerator.Core.DataGeneration.Common;

namespace EdFi.SampleDataGenerator.Core.DataGeneration.Generators.StudentAttendance
{
    public abstract class StudentAttendanceEntityGenerator : StudentDataInterchangeEntityGenerator
    {
        protected StudentAttendanceEntityGenerator(IRandomNumberGenerator randomNumberGenerator) : base(randomNumberGenerator)
        {
        }
    }
}