using EdFi.EducationOrganizationGenerator.Console.Configuration;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common.Entity;
using EdFi.SampleDataGenerator.Core.Serialization.Output.Interchanges;

namespace EdFi.EducationOrganizationGenerator.Console.Generators
{
    public abstract class EducationOrganizationEntityGenerator : InterchangeEntityGenerator<EducationOrganizationData, EducationOrganizationGeneratorConfig>
    {
        protected EducationOrganizationEntityGenerator(IRandomNumberGenerator randomNumberGenerator) : base(randomNumberGenerator)
        {
        }

        //Prevent inheritors from interfering with the base implementation.
        public sealed override void Generate(EducationOrganizationData context)
        {
            base.Generate(context);
        }
    }
}
