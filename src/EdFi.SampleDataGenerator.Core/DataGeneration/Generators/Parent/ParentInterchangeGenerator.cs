using EdFi.SampleDataGenerator.Core.DataGeneration.Common;
using EdFi.SampleDataGenerator.Core.DataGeneration.InterchangeEntities;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common.Dependencies;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common.Entity;

namespace EdFi.SampleDataGenerator.Core.DataGeneration.Generators.Parent
{
    public class ParentInterchangeGenerator : StudentDataGenerator
    {
        public override InterchangeEntity InterchangeEntity => InterchangeEntity.Parent;
        public override IEntity[] DependsOnEntities => EntityDependencies.Create(StudentEntity.Student);

        protected static GeneratorFactoryDelegate GeneratorFactory = DefaultGeneratorFactory<ParentInterchangeEntityGenerator>;

        public ParentInterchangeGenerator(IRandomNumberGenerator randomNumberGenerator) : base(randomNumberGenerator, GeneratorFactory)
        {
        }
    }
}
