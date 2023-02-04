namespace MazeLibrary
{
    public interface ISolverAlgorithmFactory
    {
        ISolverAlgorithm Create(Grid<Cell> map);
    }
}
