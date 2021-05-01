using System;
using System.Collections.Generic;
using System.Linq;
using EdFi.SampleDataGenerator.Core.Entities;
using EdFi.SampleDataGenerator.Core.Serialization.CsvHelper;
using NUnit.Framework;
using Shouldly;

namespace EdFi.SampleDataGenerator.Core.UnitTests.Serialization.CsvHelper
{
    [TestFixture]
    public class DescriptorTypeCsvClassMapFactoryTester
    {
        [Test]
        public void ShouldRunAndReturnResults()
        {
            var result = DescriptorTypeCsvClassMapFactory.GetCsvClassMaps();
            result.Count.ShouldBeGreaterThan(0);
        }

        [Test]
        public void ManuallyMappedTypesShouldNotBeEmpty()
        {
            var manuallyMappedTyeps = DescriptorTypeCsvClassMapFactory.ManualDescriptorTypeCsvClassMapMappings;
            manuallyMappedTyeps.ShouldNotBeEmpty();
        }

        [Test, TestCaseSource(nameof(GetManualCsvClassMaps))]
        public void ShouldNotGenerateMappingsForManuallyMappedTypes(Type descriptorType)
        {
            var mappings = DescriptorTypeCsvClassMapFactory.GetCsvClassMaps();
            mappings.ContainsKey(descriptorType).ShouldBeFalse();
        }

        [Test, TestCaseSource(nameof(GetAllDescriptorTypes))]
        public void ShouldMapAllProperties(Type type)
        {
            var map = DescriptorTypeCsvClassMapFactory.GetCsvClassMapFor(type);
            var propertiesNotMapped = DescriptorTypeCsvClassMapFactory.GetUnmappedPropertyNames(type);

            var publicProperties = type.GetProperties().Where(p => !propertiesNotMapped.Contains(p.Name));

            foreach (var publicProperty in publicProperties)
            {
                map.PropertyMaps.Any(m => m.Data.Names.Contains(publicProperty.Name)).ShouldBeTrue($"CsvClassMap for '{type.Name}' does not map property '{publicProperty.Name}'.  This class should be mapped manually by creating a new class that inherits from CsvClassMap<{type.Name}>");
            }
        }

        [Test]
        public void ShouldNotBuildClassMapForInvalidType()
        {
            var testType = typeof (DescriptorTypeCsvClassMapFactory);
            var map = DescriptorTypeCsvClassMapFactory.GetCsvClassMapFor(testType);

            map.ShouldBeNull();
        }

        private static IEnumerable<Type> GetAllDescriptorTypes()
        {
            return DescriptorTypeCsvClassMapFactory.GetAutoMappedDescriptorTypes();
        }

        private static IEnumerable<Type> GetManualCsvClassMaps()
        {
            return DescriptorTypeCsvClassMapFactory.ManualDescriptorTypeCsvClassMapMappings;
        } 
    }
}
