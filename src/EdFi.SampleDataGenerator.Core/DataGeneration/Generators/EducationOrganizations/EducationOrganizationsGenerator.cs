using EdFi.SampleDataGenerator.Core.DataGeneration.Common;
using EdFi.SampleDataGenerator.Core.DataGeneration.InterchangeEntities;

namespace EdFi.SampleDataGenerator.Core.DataGeneration.Generators.EducationOrganizations
{
    public class EducationOrganizationsGenerator : GlobalDataGenerator
    {
        public override InterchangeEntity InterchangeEntity => InterchangeEntity.EducationOrganization;

        public EducationOrganizationsGenerator(IRandomNumberGenerator randomNumberGenerator) : base(randomNumberGenerator, EmptyGeneratorFactory)
        {
        }

        public override void Generate(GlobalDataGeneratorContext context)
        {
            context.GlobalData.EducationOrganizationData = Configuration.EducationOrganizationData;
        }
    }
}
