using Headspring;

namespace EdFi.SampleDataGenerator.Core.DataGeneration.Common
{
    public sealed class Salutation : Enumeration<Salutation>
    {
        private Salutation(int value, string title) : base(value, title)
        {
        }
        public static Salutation Mr = new Salutation(0, "Mr.");
        public static Salutation Ms = new Salutation(1, "Ms.");
        public static Salutation Mrs = new Salutation(2, "Mrs.");
    }
}
