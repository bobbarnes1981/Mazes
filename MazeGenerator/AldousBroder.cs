using MazeLibrary;

namespace MazeGenerator
{
    public class AldousBroder : IGenerationAlgorithm
    {
        private bool[] _visited;
        private int _sleep;

        public bool IsRunning { get; private set; }
        public int CurrentX { get; private set; }
        public int CurrentY { get; private set; }

        public Map Map { get; private set;}

        public AldousBroder(int width, int height, int sleep)
        {
            Map = new Map(width, height);
            IsRunning = false;
            _visited = new bool[width * height];
            _sleep = sleep;
        }

        private int getIndex(int x, int y)
        {
            return x + (Map.Width * y);
        }

        public bool Visited(int x, int y)
        {
            return _visited[getIndex(x, y)];
        }

        public void Generate()
        {
            IsRunning = true;

            // Pick a random cell as the current cell and mark it as visited.
            var r = new Random();

            CurrentX = r.Next(0, Map.Width);
            CurrentY = r.Next(0, Map.Height);

            // While there are unvisited cells:
            while (_visited.Any(v => v == false) && IsRunning)
            {
                // Pick a random neighbour
                var _x = CurrentX;
                var _y = CurrentY;
                var point = r.Next(0, 4);
                var n = (CompassPoint)point;
                var _n = CompassPoint.North;

                switch (n)
                {
                    case CompassPoint.North:
                        _y--;
                        _n = CompassPoint.South;
                        break;
                    case CompassPoint.East:
                        _x++;
                        _n = CompassPoint.West;
                        break;
                    case CompassPoint.South:
                        _y++;
                        _n = CompassPoint.North;
                        break;
                    case CompassPoint.West:
                        _x--;
                        _n = CompassPoint.East;
                        break;
                }
                if (_x >= 0 && _x < Map.Width && _y >= 0 && _y < Map.Height)
                {
                    // If the chosen neighbour has not been visited:
                    if (_visited[getIndex(_x, _y)] == false)
                    {
                        // Remove the wall between the current cell and the chosen neighbour.
                        Map[CurrentX, CurrentY].Walls[n] = false;
                        Map[_x, _y].Walls[_n] = false;
                        // Mark the chosen neighbour as visited.
                        _visited[getIndex(_x, _y)] = true;
                    }
                    // Make the chosen neighbour the current cell.
                    CurrentX = _x;
                    CurrentY = _y;

                    if (_sleep > 0)
                    {
                        Thread.Sleep(_sleep);
                    }
                }
            }

            IsRunning = false;
        }

        public void Stop()
        {
            IsRunning = false;
        }
    }
}
