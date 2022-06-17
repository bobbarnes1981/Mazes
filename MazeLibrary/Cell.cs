namespace MazeLibrary
{
    public class Cell
    {
        public Dictionary<CompassPoint, bool> Walls { get; private set; }

        public Cell()
        {
            Walls = new Dictionary<CompassPoint, bool>
            {
                { CompassPoint.North, true },
                { CompassPoint.East, true },
                { CompassPoint.South, true },
                { CompassPoint.West, true },
            };
        }
    }
}
