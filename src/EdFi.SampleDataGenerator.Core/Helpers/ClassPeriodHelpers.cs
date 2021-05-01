using System.Linq;
using EdFi.SampleDataGenerator.Core.Entities;

namespace EdFi.SampleDataGenerator.Core.Helpers
{
    public static class ClassPeriodHelpers
    {
        private const int HomeRoomClassPeriod = 2;

        public static int GetNumericClassPeriod(this ClassPeriod classPeriod)
        {
            return GetNumericClassPeriod(classPeriod.ClassPeriodName);
        }

        public static int GetNumericClassPeriod(string classPeriodName)
        {
            return int.Parse(classPeriodName.ExtractLeadingDigits());
        }

        public static bool HasValidClassPeriodName(this ClassPeriod classPeriod)
        {
            return HasValidClassPeriodName(classPeriod.ClassPeriodName);
        }

        public static bool HasValidClassPeriodName(string classPeriodName)
        {
            int cp;
            return int.TryParse(classPeriodName.ExtractLeadingDigits(), out cp);
        }

        private static string ExtractLeadingDigits(this string classPeriodName)
        {
            return new string(classPeriodName.Trim().TakeWhile(char.IsDigit).ToArray());
        }

        public static bool IsHomeRoom(this ClassPeriod classPeriod)
        {
            return classPeriod.GetNumericClassPeriod() == HomeRoomClassPeriod;
        }
    }
}
