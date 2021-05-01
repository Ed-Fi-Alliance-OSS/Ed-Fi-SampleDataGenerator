using EdFi.SampleDataGenerator.Core.DataGeneration.Common;
using EdFi.SampleDataGenerator.Core.DataGeneration.InterchangeEntities;

namespace EdFi.SampleDataGenerator.Core.DataGeneration.Generators.StaffAssociation
{
    public class StaffAssociationInterchangeGenerator : GlobalDataGenerator
    {
        public static GeneratorFactoryDelegate GeneratorFactory => DefaultGeneratorFactory<StaffAssociationEntityGenerator>;

        public override InterchangeEntity InterchangeEntity => InterchangeEntity.StaffAssociation;

        public StaffAssociationInterchangeGenerator(IRandomNumberGenerator randomNumberGenerator) : base(randomNumberGenerator, GeneratorFactory)
        {
        }
    }
}
