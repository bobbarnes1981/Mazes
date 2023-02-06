using MazeLibrary;

namespace MazeGenerator
{
    public class RandomisedDepthFirstSearch : IGenerationAlgorithm
    {
        private readonly IRandom _random;

        private Stack<Coordinates> _stack;

        private Grid<bool> _visited;

        public Coordinates CurrentCoordinates { get; private set; }

        public bool Complete { get; private set; }

        public Grid<Cell> Map { get; private set; }

        public RandomisedDepthFirstSearch(IRandom random, MazeConfiguration configuration)
        {
            Map = new Grid<Cell>(configuration.MazeWidth, configuration.MazeHeight);

            _random = random;
            _stack = new Stack<Coordinates>();
            _visited = new Grid<bool>(configuration.MazeWidth, configuration.MazeHeight);
            Complete = false;

            // pick the random starting cell
            CurrentCoordinates = new Coordinates(_random.Next(0, Map.Width), _random.Next(0, Map.Height));
            _visited[CurrentCoordinates.X, CurrentCoordinates.Y] = true;
            _stack.Push(CurrentCoordinates);
        }

        private bool currentHasUnvisitedNeighbours()
        {
            return Map.VonNeumannNeighbourhood(CurrentCoordinates.X, CurrentCoordinates.Y)
                .Any(coord => _visited[coord.X, coord.Y] == false);
        }

        private Coordinates chooseUnvisitedNeighbour()
        {
            var choices = Map.VonNeumannNeighbourhood(CurrentCoordinates.X, CurrentCoordinates.Y)
                .Where(coord => _visited[coord.X, coord.Y] == false).ToArray();
            return choices[_random.Next(0, choices.Length)];
        }

        private void removeWallsBetweenCells(Coordinates a, Coordinates b)
        {
            if (a.X < b.X && a.Y == b.Y)
            {
                Map[a.X, a.Y].Walls[CompassPoint.East] = false;
                Map[b.X, b.Y].Walls[CompassPoint.West] = false;
            }
            else if (a.X > b.X && a.Y == b.Y)
            {
                Map[a.X, a.Y].Walls[CompassPoint.West] = false;
                Map[b.X, b.Y].Walls[CompassPoint.East] = false;
            }
            else if (a.Y < b.Y && a.X == b.X)
            {
                Map[a.X, a.Y].Walls[CompassPoint.South] = false;
                Map[b.X, b.Y].Walls[CompassPoint.North] = false;
            }
            else if (a.Y > b.Y && a.X == b.X)
            {
                Map[a.X, a.Y].Walls[CompassPoint.North] = false;
                Map[b.X, b.Y].Walls[CompassPoint.South] = false;
            }
        }

        public void Step()
        {
            if (_stack.Count > 0)
            {
                CurrentCoordinates = _stack.Pop();
                if (currentHasUnvisitedNeighbours())
                {
                    _stack.Push(CurrentCoordinates);
                    var chosen = chooseUnvisitedNeighbour();
                    removeWallsBetweenCells(CurrentCoordinates, chosen);
                    _visited[chosen.X, chosen.Y] = true;
                    _stack.Push(chosen);
                }
            }
            else
            {
                Complete = true;
            }
        }
    }
}
