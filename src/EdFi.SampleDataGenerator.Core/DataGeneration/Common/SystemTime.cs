using System;

namespace EdFi.SampleDataGenerator.Core.DataGeneration.Common
{
    public static class SystemTime
    {
        private static readonly Func<DateTime> NowCore = () => DateTime.Now;

        private static Func<DateTime> _now = NowCore;

        public static DateTime Now => _now();
        public static DateTime Today => Now.Date;

        public static void Set(DateTime now)
        {
            _now = () => now;
        }

        public static void ResetToDefault()
        {
            _now = NowCore;
        }

        public static void Stub(DateTime now, Action action)
        {
            Set(now);
            action();
            ResetToDefault();
        }
    }
}
