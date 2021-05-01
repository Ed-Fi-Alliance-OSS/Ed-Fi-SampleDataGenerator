using EdFi.SampleDataGenerator.Core.DataGeneration.Common;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common.Dependencies;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common.Entity;
using EdFi.SampleDataGenerator.Core.DataGeneration.InterchangeEntities;

namespace EdFi.SampleDataGenerator.Core.DataGeneration.Generators.Parent
{
    public sealed class ParentEntityGenerator : ParentInterchangeEntityGenerator
    {
        public override IEntity GeneratesEntity => ParentEntity.Parent;
        public override IEntity[] DependsOnEntities => EntityDependencies.None;

        public ParentEntityGenerator(IRandomNumberGenerator randomNumberGenerator) : base(randomNumberGenerator)
        {
        }

        protected override void GenerateCore(StudentDataGeneratorContext context)
        {
            var parentProfile = ParentGeneratorHelpers.GenerateParentProfile(context, RandomNumberGenerator, Configuration);

            context.GeneratedStudentData.ParentData.Parent1 = parentProfile.Parent1.Entity;
            context.GeneratedStudentData.ParentData.Parent2 = parentProfile.Parent2?.Entity;
            context.GeneratedStudentData.ParentData.ParentProfile = parentProfile;
        }
    }
}
