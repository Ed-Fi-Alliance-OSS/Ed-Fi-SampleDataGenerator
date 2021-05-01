using EdFi.SampleDataGenerator.Core.DataGeneration.Common;

namespace EdFi.SampleDataGenerator.Core.UnitTests.DataGeneration.Common
{
    public class TestRandomNumberGenerator : IRandomNumberGenerator
    {
        private int _nextIntSequenceIndex = 0;
        private int _nextDoubleSequenceIndex = 0;

        private int[] _randomIntSequence;
        public int[] RandomIntSequence
        {
            get { return _randomIntSequence; }
            set
            {
                _randomIntSequence = value;
                _nextIntSequenceIndex = 0;
            }
        }

        private double[] _randomDoubleSequence;
        public double[] RandomDoubleSequence
        {
            get { return _randomDoubleSequence; }
            set
            {
                _randomDoubleSequence = value;
                _nextDoubleSequenceIndex = 0;
            }
        }

        public void Reseed(int seed)
        {
        }

        public int Generate()
        {
            var index = _nextIntSequenceIndex;
            _nextIntSequenceIndex = (_nextIntSequenceIndex + 1) % RandomIntSequence.Length;

            return RandomIntSequence[index];
        }

        public int Generate(int maxValue)
        {
            return Generate();
        }

        public int Generate(int minValue, int maxValue)
        {
            return Generate();
        }

        public double GenerateDouble(double minValue, double maxValue)
        {
            return GenerateDouble();
        }

        public double GenerateDouble()
        {
            var index = _nextDoubleSequenceIndex;
            _nextDoubleSequenceIndex = (_nextDoubleSequenceIndex + 1) % RandomDoubleSequence.Length;

            return RandomDoubleSequence[index];
        }

        public static TestRandomNumberGenerator Create(params int[] values)
        {
            return new TestRandomNumberGenerator {RandomIntSequence = values};
        }
    }
}
