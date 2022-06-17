using MazeLibrary;

namespace MazeGenerator
{
    public interface IGenerationAlgorithm
    {
        public bool IsRunning { get; }

        public int CurrentX { get; }
        public int CurrentY { get; }

        public Map Map { get; }

        public void Generate();

        public void Stop();

        public bool Visited(int x, int y);
    }
}
