using EdFi.SampleDataGenerator.Core.DataGeneration.Common;
using EdFi.SampleDataGenerator.Core.DataGeneration.InterchangeEntities;

namespace EdFi.SampleDataGenerator.Core.DataGeneration.Generators.StudentGrade
{
    public class StudentGradeInterchangeGenerator : StudentDataGenerator
    {
        public override InterchangeEntity InterchangeEntity => InterchangeEntity.StudentGrade;
        protected static GeneratorFactoryDelegate GeneratorFactory = randomNumberGenerator => DefaultGeneratorFactory<StudentGradeBaseEntityGenerator>(randomNumberGenerator);
        
        public StudentGradeInterchangeGenerator(IRandomNumberGenerator randomNumberGenerator) : base(randomNumberGenerator, GeneratorFactory)
        {
        }
    }
}
