using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Castle.Core.Internal;
using EdFi.SampleDataGenerator.Core.DataGeneration.InterchangeEntities;
using NUnit.Framework;
using Shouldly;

namespace EdFi.SampleDataGenerator.Core.UnitTests.DataGeneration.Common.Entity
{
    using Core.DataGeneration.Common.Entity;

    [TestFixture]
    public class EntityTests
    {
        [Test, TestCaseSource(nameof(ConcreteEntityClasses))]
        public void EntityClassesShouldHavePrivateConstructors(Type entityClass)
        {
            entityClass.GetConstructors().ForEach(x => x.IsPrivate.ShouldBeTrue($"Constructors on type {entityClass.FullName} should be private."));
        }

        [Test, TestCaseSource(nameof(ConcreteEntityClasses))]
        public void EntityClassesShouldBeSealed(Type entityClass)
        {
            entityClass.IsSealed.ShouldBeTrue($"{entityClass.FullName} should be marked as sealed.");
        }

        [Test, TestCaseSource(nameof(ConcreteEntityClasses))]
        public void EntityClassesShouldHaveReadOnlyStaticPropertiesOfTheirOwnType(Type entityClass)
        {
            entityClass.GetProperties(BindingFlags.Static | BindingFlags.Public)
                .ForEach(x =>
                {
                    x.PropertyType.ShouldBe(entityClass, $"Public static property {entityClass.FullName}.{x.Name} was expected to have type {entityClass.FullName}.");
                    x.CanWrite.ShouldBeFalse($"Public static property {entityClass.FullName}.{x.Name} should not have a setter.");
                });

            entityClass.GetFields(BindingFlags.Static | BindingFlags.Public)
                .ForEach(x =>
                {
                    x.FieldType.ShouldBe(entityClass, $"Public static field {entityClass.FullName}.{x.Name} was expected to have type {entityClass.FullName}.");
                    x.IsInitOnly.ShouldBeTrue($"Public static fields {entityClass.FullName}.{x.Name} should be marked as readonly.");
                });
        }

        private static IEnumerable<Type> ConcreteEntityClasses() => typeof(Entity)
            .Assembly
            .GetTypes()
            .Where(t => typeof(Entity).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract)
            .Concat(new[] {typeof(InterchangeEntity)});
    }
}
