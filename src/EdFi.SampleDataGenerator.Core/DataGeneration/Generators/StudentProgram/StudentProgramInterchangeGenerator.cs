using System;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common;
using EdFi.SampleDataGenerator.Core.DataGeneration.InterchangeEntities;
using EdFi.SampleDataGenerator.Core.Serialization.Output;

namespace EdFi.SampleDataGenerator.Core.DataGeneration.Generators.StudentProgram
{
    public class StudentProgramInterchangeGenerator : StudentDataGenerator
    {
        public override InterchangeEntity InterchangeEntity => InterchangeEntity.StudentProgram;
        protected static GeneratorFactoryDelegate GeneratorFactory = randomNumberGenerator => DefaultGeneratorFactory<StudentProgramEntityGenerator>(randomNumberGenerator);

        public StudentProgramInterchangeGenerator(IRandomNumberGenerator randomNumberGenerator) : base(randomNumberGenerator, GeneratorFactory)
        {
        }
    }
}
