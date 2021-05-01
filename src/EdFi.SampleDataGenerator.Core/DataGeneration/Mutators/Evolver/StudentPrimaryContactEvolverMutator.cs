using System.Collections.Generic;
using System.Linq;
using CloneExtensions;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common.Attributes;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common.Entity;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common.Mutators;
using EdFi.SampleDataGenerator.Core.DataGeneration.Generators;
using EdFi.SampleDataGenerator.Core.DataGeneration.Generators.Parent;
using EdFi.SampleDataGenerator.Core.DataGeneration.InterchangeEntities;
using EdFi.SampleDataGenerator.Core.Entities;
using EdFi.SampleDataGenerator.Core.Serialization.Output.Interchanges;

namespace EdFi.SampleDataGenerator.Core.DataGeneration.Mutators.Evolver
{
    public class StudentPrimaryContactEvolverMutator : StudentMutator
    {
        public override InterchangeEntity InterchangeEntity => InterchangeEntity.Parent;
        public override IEntity Entity => ParentEntity.StudentParentAssociation;
        public override IEntityField EntityField => null;
        public override string Name => "ChangeStudentPrimaryContactParent";
        public override MutationType MutationType => MutationType.Evolution;

        public StudentPrimaryContactEvolverMutator(IRandomNumberGenerator randomNumberGenerator) : base(randomNumberGenerator)
        {
        }

        protected override MutationResult MutateCore(StudentDataGeneratorContext context)
        {
            var parentData = context.GeneratedStudentData.ParentData;
            var originalValue = new ParentData
            {
                Parent1 = parentData.Parent1,
                Parent2 = parentData.Parent2,
                StudentParentAssociations = parentData.StudentParentAssociations.Select(a =>a.GetClone()).ToList(),
                ParentProfile = parentData.ParentProfile
            };

            if (parentData.StudentParentAssociations.Count == 2 &&
                parentData.StudentParentAssociations[0].IsParent()
                && parentData.StudentParentAssociations[1].IsParent())
            {
                var parentToBePrimaryContact =
                    parentData.StudentParentAssociations.First(a => !a.PrimaryContactStatus);

                var secondParent = parentData.StudentParentAssociations.First(a => a.PrimaryContactStatus);
                parentToBePrimaryContact.MakePrimaryContact();
                secondParent.MakeSecondaryContact();
                return MutationResult.NewMutation(originalValue, context.GeneratedStudentData.ParentData);
            }

            if (parentData.StudentParentAssociations.Count == 1 &&
                     !parentData.StudentParentAssociations[0].IsParent())
            {
                var student = context.Student;
                var parentProfile = ParentGeneratorHelpers.GenerateParentProfile(context, RandomNumberGenerator, Configuration.StudentConfig);
                var parentAssociations = student.GenerateStudentParentAssociations(parentProfile);
                context.GeneratedStudentData.ParentData = new ParentData
                {
                    Parent1 = parentProfile.Parent1.Entity,
                    Parent2 = parentProfile.Parent2?.Entity,
                    StudentParentAssociations = new List<StudentParentAssociation>(parentAssociations)
                };
                return MutationResult.NewMutation(originalValue, context.GeneratedStudentData.ParentData);
            }

            return MutationResult.NoMutation;
        }
    }
}
