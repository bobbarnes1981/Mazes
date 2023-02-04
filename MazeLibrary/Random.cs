namespace MazeLibrary
{
    /// <summary>
    /// Represents a pseudo-random number generator, which is an algorithm that produces a sequence of numbers that meet certain statistical requirements for randomness.
    /// </summary>
    public class Random : IRandom
    {
        private readonly System.Random _random;

        /// <summary>
        /// Initialises a new instance of the Random class using a default seed value.
        /// </summary>
        public Random()
        {
            _random = new System.Random();
        }

        /// <summary>
        /// Returns a random integer that is within a specified range.
        /// </summary>
        /// <param name="minValue">The inclusive lower bound of the random number returned.</param>
        /// <param name="maxValue">The exclusive upper bound of the random number returned. maxValue must be greater than or equal to minValue.</param>
        /// <returns>A 32-bit signed integer greater than or equal to minValue and less than maxValue; that is, the range of return values includes minValue but not maxValue. If minValue equals maxValue, minValue is returned.</returns>
        /// <exception cref="ArgumentOutOfRangeException">minValue is greater than maxValue.</exception>
        public int Next(int minValue, int maxValue)
        {
            return _random.Next(minValue, maxValue);
        }
    }
}
