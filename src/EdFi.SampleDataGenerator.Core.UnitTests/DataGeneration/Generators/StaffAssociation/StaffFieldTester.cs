using System.Collections.Generic;
using System.Linq;
using EdFi.SampleDataGenerator.Core.DataGeneration.Generators.StaffAssociation;
using NUnit.Framework;
using Shouldly;

namespace EdFi.SampleDataGenerator.Core.UnitTests.DataGeneration.Generators.StaffAssociation
{
    [TestFixture]
    public class StaffFieldTester
    {
        private static readonly List<string> AllStaffPropertyNames = GetAllStaffPropertyNames();

        [Test, TestCaseSource(nameof(GetAllStaffFields))]
        public void ShouldMapToAnAttributeOnTheStaffEntity(StaffField mapping)
        {
            AllStaffPropertyNames.Contains(mapping.FieldName).ShouldBeTrue($"'{mapping.FieldName}' is not a valid property of Entities.Staff");
        }

        private static IEnumerable<StaffField> GetAllStaffFields()
        {
            return StaffField.GetAll();
        }

        private static List<string> GetAllStaffPropertyNames()
        {
            var members = typeof(Entities.Staff)
                    .GetProperties();

            return members.Select(m => m.Name).ToList();
        }
    }
}
