using System;

namespace EdFi.SampleDataGenerator.Core.Statistics
{
    public static class NormalDistribution
    {
        public static double Normalize(double x, double mu, double sigma)
        {
            if (sigma <= 0.0)
            {
                throw new ArgumentOutOfRangeException(nameof(sigma));
            }

            return (x - mu) / sigma;
        }

        public static double Transform(double z, double mu, double sigma)
        {
            if (sigma <= 0.0)
            {
                throw new ArgumentOutOfRangeException(nameof(sigma));
            }

            return (z * sigma) + mu;
        }

        /// <summary>
        /// Calculates CDF of a standard normal variable
        /// </summary>
        /// <param name="x">Standard, normally distributed variable</param>
        /// <returns>Cumulative probability of values in distribution that are less than or equal x</returns>
        public static double Phi(double x)
        {
            //adapted from https://www.johndcook.com/blog/csharp_phi/
            const double a1 = 0.254829592;
            const double a2 = -0.284496736;
            const double a3 = 1.421413741;
            const double a4 = -1.453152027;
            const double a5 = 1.061405429;
            const double p = 0.3275911;

            var sign = x < 0 
                ? -1
                : 1;

            x = Math.Abs(x) / Math.Sqrt(2.0);

            var t = 1.0 / (1.0 + p * x);
            var y = 1.0 - (((((a5 * t + a4) * t) + a3) * t + a2) * t + a1) * t * Math.Exp(-x * x);

            return 0.5 * (1.0 + sign * y);
        }

        /// <summary>
        /// Calculates inverse of CDF of a standard normal variable
        /// </summary>
        /// <param name="p">Percentile value, in interval (0.0, 1.0)</param>
        /// <returns>Value of x where y% of normal distribution is less than or equal to x</returns>
        public static double PhiInverse(double p)
        {
            //adapted from https://www.johndcook.com/blog/csharp_phi_inverse/
            if (p <= 0.0 || p >= 1.0)
            {
                throw new ArgumentOutOfRangeException(nameof(p));
            }

            // See article above for explanation of this section.
            if (p < 0.5)
            {
                // F^-1(p) = - G^-1(p)
                return -RationalApproximation(Math.Sqrt(-2.0 * Math.Log(p)));
            }
            
            // F^-1(p) = G^-1(1-p)
            return RationalApproximation(Math.Sqrt(-2.0 * Math.Log(1.0 - p)));
        }

        private static double RationalApproximation(double t)
        {
            // Abramowitz and Stegun formula 26.2.23.
            // The absolute value of the error should be less than 4.5 e-4.
            double[] c = { 2.515517, 0.802853, 0.010328 };
            double[] d = { 1.432788, 0.189269, 0.001308 };
            return t - ((c[2] * t + c[1]) * t + c[0]) / (((d[2] * t + d[1]) * t + d[0]) * t + 1.0);
        }
    }
}