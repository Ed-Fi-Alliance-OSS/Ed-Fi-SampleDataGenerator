using EdFi.SampleDataGenerator.Core.DataGeneration.Common;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common.Entity;
using EdFi.SampleDataGenerator.Core.DataGeneration.InterchangeEntities;
using EdFi.SampleDataGenerator.Core.Serialization.Output;
using EdFi.SampleDataGenerator.Core.Serialization.Output.Interchanges;

namespace EdFi.EducationOrganizationGenerator.Console.Generators
{
    public class AccountabilityRatingEntityGenerator : EducationOrganizationEntityGenerator
    {
        public AccountabilityRatingEntityGenerator(IRandomNumberGenerator randomNumberGenerator) : base(randomNumberGenerator)
        {
        }

        public override IEntity GeneratesEntity => EducationOrganizationEntity.AccountabilityRating;
        public override IEntity[] DependsOnEntities => new IEntity[] { EducationOrganizationEntity.LocalEducationAgency, EducationOrganizationEntity.School };

        protected override void GenerateCore(EducationOrganizationData context)
        {
        }
    }
}
