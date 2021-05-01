using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace EdFi.SampleDataGenerator.Core.Helpers
{
    public static class StringExtensions
    {
        public static bool AsBoolean(this string text)
        {
            return bool.Parse(text);
        }

        public static bool IsValidBoolean(this string text)
        {
            bool value;
            return bool.TryParse(text, out value);
        }

        public static string ToUnescapedCSV(this IEnumerable<string> stringList)
        {
            return string.Join(",", stringList);
        }

        public static string LettersOnly(this string text)
        {
            return new Regex("[^a-zA-Z]").Replace(text,"");
        }
        public static string SwapCharacters(this string text, int position1, int position2)
        {
            if (string.IsNullOrEmpty(text)) throw new InvalidOperationException();
            if (position1 < 0 || position1 >= text.Length) throw new ArgumentOutOfRangeException(nameof(position1));
            if (position2 < 0 || position2 >= text.Length) throw new ArgumentOutOfRangeException(nameof(position2));

            var charArray = text.ToCharArray();
            var tmp = charArray[position1];
            charArray[position1] = charArray[position2];
            charArray[position2] = tmp;

            return new string(charArray);
        }
    }
}
