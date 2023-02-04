using MazeLibrary;

namespace MazeSolver
{
    public class WallFollower : ISolverAlgorithm
    {
        private Coordinates _endCoordinates;
        private CompassPoint _facing;

        public Coordinates CurrentCoordinates { get; private set; }

        public bool Complete
        {
            get
            {
                return CurrentCoordinates.X == _endCoordinates.X && CurrentCoordinates.Y == _endCoordinates.Y;
            }
        }

        public Grid<Cell> Map { get; private set; }

        public WallFollower(Grid<Cell> map)
        {
            Map = map;

            _facing = CompassPoint.North;
            CurrentCoordinates = new Coordinates(0, 0);
            _endCoordinates = new Coordinates(map.Width - 1, map.Height - 1);
        }

        public void Step()
        {
            CompassPoint checking;

            var x = 0;
            var y = 0;
            while (x == 0 && y == 0)
            {
                // Check for path left, if no path, turn 90 degrees right, otherwise go left
                switch (_facing)
                {
                    case CompassPoint.North:
                        checking = CompassPoint.West;
                        if (Map[CurrentCoordinates.X, CurrentCoordinates.Y].Walls[checking])
                        {
                            _facing = CompassPoint.East;
                        }
                        else
                        {
                            _facing = checking;
                            x = -1;
                        }
                        break;
                    case CompassPoint.East:
                        checking = CompassPoint.North;
                        if (Map[CurrentCoordinates.X, CurrentCoordinates.Y].Walls[checking])
                        {
                            _facing = CompassPoint.South;
                        }
                        else
                        {
                            _facing = checking;
                            y = -1;
                        }
                        break;
                    case CompassPoint.South:
                        checking = CompassPoint.East;
                        if (Map[CurrentCoordinates.X, CurrentCoordinates.Y].Walls[checking])
                        {
                            _facing = CompassPoint.West;
                        }
                        else
                        {
                            _facing = checking;
                            x = +1;
                        }
                        break;
                    case CompassPoint.West:
                        checking = CompassPoint.South;
                        if (Map[CurrentCoordinates.X, CurrentCoordinates.Y].Walls[checking])
                        {
                            _facing = CompassPoint.North;
                        }
                        else
                        {
                            _facing = checking;
                            y = +1;
                        }
                        break;
                }
            }

            CurrentCoordinates = new Coordinates(CurrentCoordinates.X + x, CurrentCoordinates.Y + y);
        }
    }
}
