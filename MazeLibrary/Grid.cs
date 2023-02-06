using System.Collections;

namespace MazeLibrary
{
    public class Grid<T> : IEnumerable<T> where T : new()
    {
        public int Width { get; private set; }
        public int Height { get; private set; }

        private static Coordinates[] relativeVonNeumannNeighbours = new Coordinates[] {
            new Coordinates(0, -1),
            new Coordinates(-1, 0),
            new Coordinates(1, 0),
            new Coordinates(0, 1)
        };

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

        public bool ValidCoordinates(int x, int y)
        {
            return x >= 0 && y >=0 && x < Width && y < Height;
        }

        public bool ValidCoordinates(Coordinates coordinates)
        {
            return ValidCoordinates(coordinates.X, coordinates.Y);
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

        public T this[Coordinates coordinates]
        {
            get
            {
                return this[coordinates.X, coordinates.Y];
            }
            set
            {
                this[coordinates.X, coordinates.Y] = value;
            }
        }

        public Coordinates[] VonNeumannNeighbourhood(int x, int y)
        {
            List<Coordinates> neighbours = new List<Coordinates>();
            foreach (var coordinates in relativeVonNeumannNeighbours)
            {
                var neighbour = new Coordinates(x + coordinates.X, y + coordinates.Y);
                if (ValidCoordinates(neighbour.X, neighbour.Y))
                {
                    neighbours.Add(neighbour);
                }
            }
            return neighbours.ToArray();
        }

        public Coordinates[] VonNeumannNeighbourhood(Coordinates coordinates)
        {
            return VonNeumannNeighbourhood(coordinates.X, coordinates.Y);
        }
    }
}