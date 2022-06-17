using MazeLibrary;

namespace MazeSolver
{
    public interface ISolverAlgorithm
    {
        public bool IsRunning { get; }

        public int CurrentX { get; }
        public int CurrentY { get; }

        public Map Map { get; }

        public List<Coordinates> Locations { get; }

        public void Solve(Map map, int sleep);

        public void Stop();
    }
}
