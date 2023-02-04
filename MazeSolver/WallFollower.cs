using MazeLibrary;

namespace MazeSolver
{
    public class WallFollower : ISolverAlgorithm
    {
        public bool IsRunning { get; private set; }

        public int CurrentX { get; private set; }

        public int CurrentY { get; private set; }

        public Map Map { get; private set; }

        public List<Coordinates> Locations { get; private set; }

        public WallFollower()
        {
            Locations = new List<Coordinates>();
        }

        public void Solve(Map map)
        {
            Map = map;

            Locations.Clear();

            IsRunning = true;

            var startX = 0;
            var startY = 0;
            var endX = map.Width - 1;
            var endY = map.Height - 1;

            CurrentX = startX;
            CurrentY = startY;

            var facing = CompassPoint.North;
            CompassPoint checking;

            Locations.Add(new Coordinates(CurrentX, CurrentY));
            while ((CurrentX != endX || CurrentY != endY) && IsRunning)
            {
                var x = 0;
                var y = 0;
                while (x == 0 && y == 0)
                {
                    switch (facing)
                    {
                        case CompassPoint.North:
                            checking = CompassPoint.West;
                            if (map[CurrentX, CurrentY].Walls[checking])
                            {
                                facing = CompassPoint.East;
                            }
                            else
                            {
                                facing = checking;
                                x = -1;
                            }
                            break;
                        case CompassPoint.East:
                            checking = CompassPoint.North;
                            if (map[CurrentX, CurrentY].Walls[checking])
                            {
                                facing = CompassPoint.South;
                            }
                            else
                            {
                                facing = checking;
                                y = -1;
                            }
                            break;
                        case CompassPoint.South:
                            checking = CompassPoint.East;
                            if (map[CurrentX, CurrentY].Walls[checking])
                            {
                                facing = CompassPoint.West;
                            }
                            else
                            {
                                facing = checking;
                                x = +1;
                            }
                            break;
                        case CompassPoint.West:
                            checking = CompassPoint.South;
                            if (map[CurrentX, CurrentY].Walls[checking])
                            {
                                facing = CompassPoint.North;
                            }
                            else
                            {
                                facing = checking;
                                y = +1;
                            }
                            break;
                    }
                }
                CurrentX += x;
                CurrentY += y;
                Locations.Add(new Coordinates(CurrentX, CurrentY));
            }

            IsRunning = false;
        }

        public void Stop()
        {
            IsRunning = false;
        }
    }
}
