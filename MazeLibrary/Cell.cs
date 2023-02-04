using MazeLibrary;

namespace MazeLibrary
{
    public class Cell
    {
        public Dictionary<CompassPoint, bool> Walls { get; private set; }

        public Cell()
            : this(new Dictionary<CompassPoint, bool>
            {
                { CompassPoint.North, true },
                { CompassPoint.East, true },
                { CompassPoint.South, true },
                { CompassPoint.West, true },
            })
        {
        }

        public Cell(Dictionary<CompassPoint, bool> walls)
        {
            Walls = walls;
        }
    }
}
