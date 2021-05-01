using System.Collections.Generic;
using System.Linq;
using EdFi.SampleDataGenerator.Core.DataGeneration.Generators.MasterSchedule;
using NUnit.Framework;
using Shouldly;

namespace EdFi.SampleDataGenerator.Core.UnitTests.DataGeneration.Generators.MasterSchedule
{
    [TestFixture]
    public class SectionFieldTester
    {
        private static readonly List<string> AllSectionPropertyNames = GetAllSectionPropertyNames();

        [Test, TestCaseSource(nameof(GetAllSectionFields))]
        public void ShouldMapToAnAttributeOnTheSectionEntity(SectionField mapping)
        {
            AllSectionPropertyNames.Contains(mapping.FieldName).ShouldBeTrue($"'{mapping.FieldName}' is not a valid property of Entities.Section");
        }

        private static IEnumerable<SectionField> GetAllSectionFields()
        {
            return SectionField.GetAll();
        }

        private static List<string> GetAllSectionPropertyNames()
        {
            var members = typeof(Entities.Section)
                    .GetProperties();

            return members.Select(m => m.Name).ToList();
        }
    }
}
