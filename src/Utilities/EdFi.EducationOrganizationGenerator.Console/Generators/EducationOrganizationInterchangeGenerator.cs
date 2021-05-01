using System.Reflection;
using EdFi.EducationOrganizationGenerator.Console.Configuration;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common.Entity;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common.Interchanges;
using EdFi.SampleDataGenerator.Core.DataGeneration.InterchangeEntities;
using EdFi.SampleDataGenerator.Core.Serialization.Output.Interchanges;

namespace EdFi.EducationOrganizationGenerator.Console.Generators
{
    public class EducationOrganizationInterchangeGenerator : InterchangeDataGenerator<EducationOrganizationData, EducationOrganizationGeneratorConfig>
    {
        private static readonly GeneratorFactoryDelegate GeneratorFactory = rng => InterchangeEntityGeneratorFactory.DefaultGeneratorFactory<EducationOrganizationData, EducationOrganizationGeneratorConfig, EducationOrganizationEntityGenerator>(rng, Assembly.GetExecutingAssembly());

        public EducationOrganizationInterchangeGenerator() : this(new RandomNumberGenerator(), GeneratorFactory)
        {
        }

        public EducationOrganizationInterchangeGenerator(IRandomNumberGenerator randomNumberGenerator, GeneratorFactoryDelegate generatorFactory) : base(randomNumberGenerator, generatorFactory)
        {
        }

        public override InterchangeEntity InterchangeEntity => InterchangeEntity.EducationOrganization;
    }
}
