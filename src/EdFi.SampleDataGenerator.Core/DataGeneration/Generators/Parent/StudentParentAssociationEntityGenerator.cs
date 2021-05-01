using EdFi.SampleDataGenerator.Core.DataGeneration.Common;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common.Dependencies;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common.Entity;
using EdFi.SampleDataGenerator.Core.DataGeneration.InterchangeEntities;

namespace EdFi.SampleDataGenerator.Core.DataGeneration.Generators.Parent
{
    public sealed class StudentParentAssociationEntityGenerator : ParentInterchangeEntityGenerator
    {
        public override IEntity GeneratesEntity => ParentEntity.StudentParentAssociation;
        public override IEntity[] DependsOnEntities => EntityDependencies.Create(ParentEntity.Parent);

        public StudentParentAssociationEntityGenerator(IRandomNumberGenerator randomNumberGenerator) : base(randomNumberGenerator)
        {
        }

        protected override void GenerateCore(StudentDataGeneratorContext context)
        {
            var student = context.Student;
            var parentAssociations = student.GenerateStudentParentAssociations(context.GeneratedStudentData.ParentData.ParentProfile);
            context.GeneratedStudentData.ParentData.StudentParentAssociations.AddRange(parentAssociations);
        }
    }
}
