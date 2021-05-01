using System;
using System.Collections.Generic;
using System.Linq;
using EdFi.SampleDataGenerator.Core.Helpers;
using EdFi.SampleDataGenerator.Core.Serialization.Output;
using EdFi.SampleDataGenerator.Core.Serialization.Output.Interchanges;
using NUnit.Framework;
using Shouldly;

namespace EdFi.SampleDataGenerator.Core.UnitTests.Serialization.Output.Interchanges
{
    [TestFixture]
    public class SdgEntityCollectionTester
    {
        [Test, TestCaseSource(nameof(ConcreteSdgEntityCollectionClasses))]
        public void AllSdgEntityCollectionsShouldHaveInterchangeOutputAttribute(Type collectionType)
        {
            collectionType.HasInterchangeOutputInfo().ShouldBeTrue();
        }

        private static IEnumerable<Type> ConcreteSdgEntityCollectionClasses() => typeof(ISdgEntityOutputCollection<>)
           .Assembly
           .GetTypes()
           .Where(t => !t.IsInterface && !t.IsAbstract && t.ImplementsGenericInterface(typeof(ISdgEntityOutputCollection<>)));
    }
}
