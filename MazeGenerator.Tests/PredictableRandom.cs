using MazeLibrary;

namespace MazeGenerator.Tests
{
    internal class PredictableRandom : IRandom
    {
        private int _current = -1;

        public int Next(int minValue, int maxValue)
        {
            _current++;

            if (_current < minValue)
            {
                _current = minValue;
            }
            else if (_current >= maxValue)
            {
                _current = minValue;
            }

            return _current;
        }
    }
}
