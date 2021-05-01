using System;

namespace EdFi.SampleDataGenerator.Core.Helpers
{
    public static class MathHelpers
    {
        public static bool AdditionWillOverflowInteger(int lhs, int rhs)
        {
            try
            {
                checked
                {
                    var result = lhs + rhs;
                    return false;
                }
            }
            catch (OverflowException)
            {
                return true;
            }
        }

        public static bool IsEqualWithinTolerance(this double lhs, double rhs)
        {
            return Math.Abs(lhs - rhs) < Constants.FloatingPointMath.Epsilon;
        }
    }
}
