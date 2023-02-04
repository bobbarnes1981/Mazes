namespace MazeLibrary
{
    public interface ISolverAlgorithm
    {
        public Coordinates CurrentCoordinates { get; }

        public bool Complete { get; }

        public Grid<Cell> Map { get; }

        public void Step();
    }
}
