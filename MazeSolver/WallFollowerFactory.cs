using MazeLibrary;

namespace MazeSolver
{
    public class WallFollowerFactory : ISolverAlgorithmFactory
    {
        public ISolverAlgorithm Create(Grid<Cell> map)
        {
            return new WallFollower(map);
        }
    }
}
