namespace EdFi.SampleDataGenerator.Core.DataGeneration.Common
{
    public interface IRandomNumberGenerator
    {
        /// <summary>
        /// Re-seeds the random number generator with a new initialization value
        /// </summary>
        /// <param name="seed">Seed value to use for initializing the random number generation algorithm</param>
        void Reseed(int seed);
        /// <summary>
        /// Returns a non-negative random integer.
        /// </summary>
        /// <returns>Returns a non-negative random integer.</returns>
        int Generate();
        /// <summary>
        /// Returns random integer value greater than or equal to 0 and less than <paramref name="maxValue"/>
        /// </summary>
        /// <param name="maxValue">The exclusive upper bound of the random number to be generated. <paramref name="maxValue"/> must be greater than or equal to 0</param>
        /// <returns>Returns random integer value greater than or equal to 0 and less than <paramref name="maxValue"/></returns>
        int Generate(int maxValue);
        /// <summary>
        /// Returns random integer value greather than or equal to <paramref name="minValue"/> and less than <paramref name="maxValue"/>
        /// </summary>
        /// <param name="minValue">The inclusive lower bound of the random number to be generated. <paramref name="minValue"/> must be greater than or equal to 0</param>
        /// <param name="maxValue">The exclusive upper bound of the random number to be generated. <paramref name="maxValue"/> must be greater than or equal to 0</param>
        /// <returns>Returns random integer value greather than or equal to <paramref name="minValue"/> and less than <paramref name="maxValue"/></returns>
        int Generate(int minValue, int maxValue);
        /// <summary>
        /// Returns a random floating-point number that is greater than or equal to <paramref name="minValue"/> and less than <paramref name="maxValue"/>.
        /// </summary>
        /// <param name="minValue">The inclusive lower bound of the random number to be generated. <paramref name="minValue"/> must be greater than or equal to 0.0</param>
        /// <param name="maxValue">The exclusive upper bound of the random number to be generated. <paramref name="maxValue"/> must be greater than or equal to 0.0</param>
        /// <returns>Returns a random floating-point number that is greater than or equal to <paramref name="minValue"/> and less than <paramref name="maxValue"/>.</returns>
        double GenerateDouble(double minValue, double maxValue);
        /// <summary>
        /// Returns a random floating-point number that is greater than or equal to 0.0, and less than 1.0.
        /// </summary>
        /// <returns>Returns a random floating-point number that is greater than or equal to 0.0, and less than 1.0.</returns>
        double GenerateDouble();
    }
}