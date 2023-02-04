namespace MazeLibrary
{
    public interface IGenerationAlgorithm
    {
        public Coordinates CurrentCoordinates { get; }

        public bool Complete { get; }

        public Grid<Cell> Map { get; }

        public void Step();
    }
}
