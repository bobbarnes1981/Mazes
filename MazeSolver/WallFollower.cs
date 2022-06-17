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

        public void Solve(Map map, int sleep)
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
            var checking = CompassPoint.West;

            Locations.Add(new Coordinates(CurrentX, CurrentY));
            while ((CurrentX != endX || CurrentY != endY) && IsRunning)
            {
                var x = 0;
                var y = 0;
                Console.WriteLine($"facing: {facing}");
                while (x == 0 && y == 0)
                {
                    switch (facing)
                    {
                        case CompassPoint.North:
                            checking = CompassPoint.West;
                            Console.WriteLine($"checking: {checking}");
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
                            Console.WriteLine($"checking: {checking}");
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
                            Console.WriteLine($"checking: {checking}");
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
                            Console.WriteLine($"checking: {checking}");
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
                    Console.WriteLine($"turning to face: {facing}");
                }
                Console.WriteLine($"moving: {x},{y}");
                CurrentX += x;
                CurrentY += y;
                Locations.Add(new Coordinates(CurrentX, CurrentY));
                Console.WriteLine($"Steps: {Locations.Count()}");

                if (sleep > 0)
                {
                    Thread.Sleep(sleep);
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
