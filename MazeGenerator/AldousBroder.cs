using MazeLibrary;

namespace MazeGenerator
{
    public class AldousBroder : IGenerationAlgorithm
    {
        private readonly IRandom _random;

        private Grid<bool> _visited;

        public Coordinates CurrentCoordinates { get; private set; }

        public bool Complete
        {
            get
            {
                return _visited.All(v => v == true);
            }
        }

        public Grid<Cell> Map { get; private set;}

        public AldousBroder(IRandom random, MazeConfiguration configuration)
        {
            Map = new Grid<Cell>(configuration.MazeWidth, configuration.MazeHeight);

            _random = random;
            _visited = new Grid<bool>(configuration.MazeWidth, configuration.MazeHeight);

            // pick the random starting cell
            CurrentCoordinates = new Coordinates(_random.Next(0, Map.Width), _random.Next(0, Map.Height));
            _visited[CurrentCoordinates.X, CurrentCoordinates.Y] = true;
        }

        public void Step()
        {
            // While there are unvisited cells:
            if (_visited.Any(v => v == false))
            {
                int x;
                int y;
                CompassPoint wallFront;
                CompassPoint wallBehind;

                // Pick a random neighbour within the grid
                do
                {
                    x = CurrentCoordinates.X;
                    y = CurrentCoordinates.Y;
                    wallFront = (CompassPoint)_random.Next(0, 4);
                    switch (wallFront)
                    {
                        case CompassPoint.North:
                            y--;
                            wallBehind = CompassPoint.South;
                            break;
                        case CompassPoint.East:
                            x++;
                            wallBehind = CompassPoint.West;
                            break;
                        case CompassPoint.South:
                            y++;
                            wallBehind = CompassPoint.North;
                            break;
                        case CompassPoint.West:
                            x--;
                            wallBehind = CompassPoint.East;
                            break;
                        default:
                            throw new Exception($"Invalid CompassPoint {wallFront}");
                    }
                } while (x < 0 || x >= Map.Width || y < 0 || y >= Map.Height);

                // If the chosen neighbour has not been visited:
                if (_visited[x, y] == false)
                {
                    // Remove the wall between the current cell and the chosen neighbour.
                    Map[CurrentCoordinates.X, CurrentCoordinates.Y].Walls[wallFront] = false;
                    Map[x, y].Walls[wallBehind] = false;
                    // Mark the chosen neighbour as visited.
                    _visited[x, y] = true;
                }
                // Make the chosen neighbour the current cell.
                CurrentCoordinates = new Coordinates(x, y);
            }
        }
    }
}
