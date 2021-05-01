using EdFi.SampleDataGenerator.Core.Entities;

namespace EdFi.SampleDataGenerator.Core.Helpers
{
    public static class NameHelpers
    {
        public static string AsString(this Name name)
        {
            return $"{name.FirstName} {name.LastSurname}";
        }
    }
}
