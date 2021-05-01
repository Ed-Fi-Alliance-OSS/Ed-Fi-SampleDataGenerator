using System;
using System.Linq;
using EdFi.SampleDataGenerator.Core.Entities;

namespace EdFi.SampleDataGenerator.Core.Helpers
{
    public static class SchoolYearHelpers
    {
        public static SchoolYearType AddYears(this SchoolYearType schoolYear, int yearsToAdd)
        {
            try
            {
                var yearString = schoolYear.ToCodeValue();
                var years = yearString.Split('-').Select(y => int.Parse(y) + yearsToAdd);
                var newYearString = string.Join("-", years);
                return EnumHelpers.Parse<SchoolYearType>(newYearString);
            }
            catch (Exception)
            {
                throw new ArgumentOutOfRangeException(nameof(yearsToAdd), $"Can't create a valid SchoolYearType by adding {yearsToAdd} years to the {schoolYear.ToCodeValue()} school year");
            }
        }
    }
}
