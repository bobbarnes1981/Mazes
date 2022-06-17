namespace MazeLibrary
{
    public class Map
    {
        public int Width { get; private set; }
        public int Height { get; private set; }

        private Cell[] _cells;

        private int getIndex(int x, int y)
        {
            return x + (Width * y);
        }

        public Map(int width, int height)
        {
            Width = width;
            Height = height;
            _cells = new Cell[Width * Height];
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    _cells[getIndex(x, y)] = new Cell();
                }
            }
        }

        public Cell this[int x, int y]
        {
            get
            {
                return _cells[getIndex(x, y)];
            }
        }
    }
}