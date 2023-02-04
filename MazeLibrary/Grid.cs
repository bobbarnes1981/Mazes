using System.Collections;

namespace MazeLibrary
{
    public class Grid<T> : IEnumerable<T> where T : new()
    {
        public int Width { get; private set; }
        public int Height { get; private set; }

        private T[] _cells;

        private int getIndex(int x, int y)
        {
            return x + (Width * y);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return ((IEnumerable<T>)_cells).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _cells.GetEnumerator();
        }

        public Grid(int width, int height)
        {
            Width = width;
            Height = height;
            _cells = new T[Width * Height];
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    _cells[getIndex(x, y)] = new T();
                }
            }
        }

        public T this[int x, int y]
        {
            get
            {
                return _cells[getIndex(x, y)];
            }
            set
            {
                _cells[getIndex(x, y)] = value;
            }
        }
    }
}