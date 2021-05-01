using System;

namespace EdFi.SampleDataGenerator.Core.DataGeneration.Common
{
    public class RandomNumberGenerator : IRandomNumberGenerator
    {
        private Random _rng;

        public RandomNumberGenerator()
        {
            _rng = new Random();
        }

        public RandomNumberGenerator(int seed)
        {
            Reseed(seed);
        }

        public void Reseed(int seed)
        {
            _rng = new Random(seed);
        }

        public int Generate()
        {
            return _rng.Next();
        }

        public int Generate(int maxValue)
        {
            return _rng.Next(maxValue);
        }

        public int Generate(int minValue, int maxValue)
        {
            return _rng.Next(minValue, maxValue);
        }

        public double GenerateDouble(double minValue, double maxValue)
        {
            return minValue + (_rng.NextDouble() * (maxValue - minValue));
        }

        public double GenerateDouble()
        {
            return _rng.NextDouble();
        }
    }
}