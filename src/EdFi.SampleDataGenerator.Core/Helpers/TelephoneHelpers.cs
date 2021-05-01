namespace EdFi.SampleDataGenerator.Core.Helpers
{
    public static class TelephoneHelpers
    {
        public static string BuildNumber(int areaCode, int numberPrefix, int number)
        {
            return $"({areaCode:D3}) {numberPrefix:D3}-{number:D4}";
        }

        public static string BuildNumber(string areaCode, string numberPrefix, string number)
        {
            return $"({areaCode}) {numberPrefix}-{number}";
        }

        public static string[] ParseNumber(string phoneNumber)
        {
            var areaCode = phoneNumber.Substring(1, 3);
            var numberPrefix = phoneNumber.Substring(6, 3);
            var number = phoneNumber.Substring(10);

            return new[] {areaCode, numberPrefix, number};
        }
    }
}
