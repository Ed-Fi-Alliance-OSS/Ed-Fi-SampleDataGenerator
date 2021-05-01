using System;
using EdFi.SampleDataGenerator.Core.Statistics;

namespace EdFi.SampleDataGenerator.Core.DataGeneration.Common
{
    public static class StudentPerformanceProfileDistribution
    {
        public const double Mu = 0.5; //centered at 0.5
        public const double Sigma = 1.0 / 6.0; //tuned so that 3 sigma (99.7% of distribution) falls between 0 and 1

        //These constants calculated using Mu = 0.5 / Sigma = (1/6) -- they MUST be recalculated if Mu or Sigma change
        public const double BottomQuartile = 0.3875;
        public const double SecondQuartile = Mu;
        public const double ThirdQuartile = 1 - BottomQuartile;

        public const double FiftiethPercentile = Mu;

        public static double GetStudentPerformanceProfileFromPercentile(double percentile)
        {
            var z = NormalDistribution.PhiInverse(percentile);
            var x = NormalDistribution.Transform(z, Mu, Sigma);

            return x;
        }

        public static double GetStudentPercentileFromPerformanceProfile(double performanceProfileIndex)
        {
            var x = NormalDistribution.Normalize(performanceProfileIndex, Mu, Sigma);
            var p = NormalDistribution.Phi(x);

            return p;
        }

        public static double GenerateStudentPerformanceProfile(IRandomNumberGenerator randomNumberGenerator)
        {
            //we'll cut off at [0,1] to ensure the performance index always falls in that interval
            var rawPerformanceIndex = randomNumberGenerator.GenerateGaussian(Mu, Sigma);
            return Math.Min(Math.Max(0, rawPerformanceIndex), 1);
        }
    }
}
