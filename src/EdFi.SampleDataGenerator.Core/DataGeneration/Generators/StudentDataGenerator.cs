using EdFi.SampleDataGenerator.Core.Config;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common.Interchanges;

namespace EdFi.SampleDataGenerator.Core.DataGeneration.Generators
{
    public abstract class StudentDataGenerator : InterchangeDataGenerator<StudentDataGeneratorContext, StudentDataGeneratorConfig>
    {
        protected StudentDataGenerator(IRandomNumberGenerator randomNumberGenerator, GeneratorFactoryDelegate generatorFactory) : base(randomNumberGenerator, generatorFactory)
        {
        }

        public void GenerateAdditiveData(StudentDataGeneratorContext context, IDataPeriod dataPeriod)
        {
            foreach (var interchangeDataGenerator in Generators)
            {
                var additiveGenerator = interchangeDataGenerator as StudentDataInterchangeEntityGenerator;
                additiveGenerator?.GenerateAdditiveData(context, dataPeriod);
            }
        }
    }
}