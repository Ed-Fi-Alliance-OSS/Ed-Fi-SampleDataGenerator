using EdFi.SampleDataGenerator.Core.Config;

namespace EdFi.SampleDataGenerator.Core.Helpers
{
    public static class SchoolProfileHelpers
    {
        public static string GetSchoolEntityId(this ISchoolProfile schoolProfile)
        {
            return $"SCOL_{schoolProfile.SchoolId}";
        }
    }
}
