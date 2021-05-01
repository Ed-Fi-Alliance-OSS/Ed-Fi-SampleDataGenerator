namespace EdFi.SampleDataGenerator.Core.DataGeneration.Common
{
    public class RandomOption<T>
    {
        public RandomOption(T value, double probability)
        {
            Value = value;
            Probability = probability;
        }

        public double Probability { get; }
        public T Value { get; }
    }
}
