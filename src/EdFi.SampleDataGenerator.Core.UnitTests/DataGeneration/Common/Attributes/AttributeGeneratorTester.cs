using System.Collections.Generic;
using System.Linq;
using EdFi.SampleDataGenerator.Core.DataGeneration.Common.Attributes;
using EdFi.SampleDataGenerator.Core.DataGeneration.Generators;
using NUnit.Framework;
using Shouldly;

namespace EdFi.SampleDataGenerator.Core.UnitTests.DataGeneration.Common.Attributes
{
    [TestFixture]
    public class AttributeGeneratorTester
    {
        public static readonly IEnumerable<IEntityAttributeGenerator<GlobalDataGeneratorContext, GlobalDataGeneratorConfig>> GlobalAttributeGenerators = GetAttributeGenerators<GlobalDataGeneratorContext, GlobalDataGeneratorConfig>();
        public static readonly IEnumerable<IEntityAttributeGenerator<StudentDataGeneratorContext, StudentDataGeneratorConfig>> StudentAttributeGenerators = GetAttributeGenerators<StudentDataGeneratorContext, StudentDataGeneratorConfig>();

        [Test, TestCaseSource(nameof(GlobalAttributeGenerators))]
        public void GlobalAttributeGeneratorsShouldNotDependOnThemselves(IEntityAttributeGenerator<GlobalDataGeneratorContext, GlobalDataGeneratorConfig> instance)
        {
            instance.DependsOnFields?.Any(f => f.FullyQualifiedFieldName == instance.FullyQualifiedFieldName).ShouldBeFalse();
        }

        [Test, TestCaseSource(nameof(StudentAttributeGenerators))]
        public void StudentAttributeGeneratorsShouldNotDependOnThemselves(IEntityAttributeGenerator<StudentDataGeneratorContext, StudentDataGeneratorConfig> instance)
        {
            instance.DependsOnFields?.Any(f => f.FullyQualifiedFieldName == instance.FullyQualifiedFieldName).ShouldBeFalse();
        }

        [Test]
        public void StudentAttributeGeneratorsShouldNotDependOnGlobalAttributeGenerators()
        {
            var fieldsGeneratedByGlobalAttributeGenerators =
                new HashSet<string>(
                    GlobalAttributeGenerators
                        .Select(x => x.GeneratesField.FullyQualifiedFieldName));

            var fieldsStudentAttributeGeneratorsDependOn =
                new HashSet<string>(
                    StudentAttributeGenerators
                        .SelectMany(x => x.DependsOnFields.Select(y => y.FullyQualifiedFieldName)));

            fieldsGeneratedByGlobalAttributeGenerators
                .Intersect(fieldsStudentAttributeGeneratorsDependOn)
                .ShouldBeEmpty();
        }

        [Test]
        public void GlobalAttributeGeneratorsShouldNotDependOnStudentAttributeGenerators()
        {
            var fieldsGeneratedByStudentAttributeGenerators =
                new HashSet<string>(
                    StudentAttributeGenerators
                        .Select(x => x.GeneratesField.FullyQualifiedFieldName));

            var fieldsGlobalAttributeGeneratorsDependOn =
                new HashSet<string>(
                    GlobalAttributeGenerators
                        .SelectMany(x => x.DependsOnFields.Select(y => y.FullyQualifiedFieldName)));

            fieldsGeneratedByStudentAttributeGenerators
                .Intersect(fieldsGlobalAttributeGeneratorsDependOn)
                .ShouldBeEmpty();
        }

        [Test, TestCaseSource(nameof(GlobalAttributeGenerators))]
        public void GlobalAttributeGeneratorsShouldNotHaveNullDependencyList(IEntityAttributeGenerator<GlobalDataGeneratorContext, GlobalDataGeneratorConfig> instance)
        {
            instance.DependsOnFields.ShouldNotBeNull();
        }

        [Test, TestCaseSource(nameof(StudentAttributeGenerators))]
        public void StudentAttributeGeneratorsShouldNotHaveNullDependencyList(IEntityAttributeGenerator<StudentDataGeneratorContext, StudentDataGeneratorConfig> instance)
        {
            instance.DependsOnFields.ShouldNotBeNull();
        }

        private static IEnumerable<IEntityAttributeGenerator<TContext, TConfig>> GetAttributeGenerators<TContext, TConfig>()
        {
            var random = new TestRandomNumberGenerator();

            return EntityAttributeGeneratorFactory
                .BuildAllAttributeGenerators<
                    TContext,
                    TConfig>(random).ToArray();
        }
    }
}
