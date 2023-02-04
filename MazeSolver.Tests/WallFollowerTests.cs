using MazeLibrary;
using NUnit.Framework;
using System.Reflection.Emit;

namespace MazeSolver.Tests
{
    public class WallFollowerTests
    {
        private Grid<Cell> _map;

        [SetUp]
        public void Setup()
        {
            _map = new Grid<Cell>(2, 2);
            _map[0, 0] = new Cell(
                new Dictionary<CompassPoint, bool>
                {
                    { CompassPoint.North, true },
                    { CompassPoint.East, false },
                    { CompassPoint.South, false },
                    { CompassPoint.West, true },
                }
            );
            _map[1, 0] = new Cell(
                new Dictionary<CompassPoint, bool>
                {
                    { CompassPoint.North, true },
                    { CompassPoint.East, true },
                    { CompassPoint.South, false },
                    { CompassPoint.West, false },
                }
            );
            _map[0, 1] = new Cell(
                new Dictionary<CompassPoint, bool>
                {
                    { CompassPoint.North, false },
                    { CompassPoint.East, true },
                    { CompassPoint.South, true },
                    { CompassPoint.West, true },
                }
            );
            _map[1, 1] = new Cell(
                new Dictionary<CompassPoint, bool>
                {
                    { CompassPoint.North, false },
                    { CompassPoint.East, true },
                    { CompassPoint.South, true },
                    { CompassPoint.West, true },
                }
            );
        }

        [Test]
        public void CheckMapIsLoadedByConstructor()
        {
            var solver = new WallFollower(_map);

            Assert.That(solver.Map, Is.Not.Null);
            Assert.That(solver.Map.Width, Is.EqualTo(2));
            Assert.That(solver.Map.Height, Is.EqualTo(2));
        }

        [Test]
        public void CheckMapIsNotCompleted()
        {
            var solver = new WallFollower(_map);

            Assert.That(solver.Complete, Is.False);
        }

        [Test]
        public void CheckSolvesMap()
        {
            var solver = new WallFollower(_map);

            Assert.That(solver.CurrentCoordinates.X, Is.EqualTo(0));
            Assert.That(solver.CurrentCoordinates.Y, Is.EqualTo(0));

            solver.Step();

            Assert.That(solver.CurrentCoordinates.X, Is.EqualTo(1));
            Assert.That(solver.CurrentCoordinates.Y, Is.EqualTo(0));

            solver.Step();

            Assert.That(solver.CurrentCoordinates.X, Is.EqualTo(1));
            Assert.That(solver.CurrentCoordinates.Y, Is.EqualTo(1));

            Assert.That(solver.Complete, Is.True);
        }
    }
}