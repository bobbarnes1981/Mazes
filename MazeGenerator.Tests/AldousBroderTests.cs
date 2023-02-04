using MazeLibrary;
using NUnit.Framework;

namespace MazeGenerator.Tests
{
    public class AldousBroderTests
    {
        private MazeConfiguration _configuration;

        [SetUp]
        public void Setup()
        {
            _configuration = new MazeConfiguration
            {
                MazeWidth = 2,
                MazeHeight = 2,
            };
        }

        [Test]
        public void CheckMapIsGeneratedByConstructor()
        {
            var generator = new AldousBroder(new PredictableRandom(), _configuration);

            Assert.That(generator.Map, Is.Not.Null);
            Assert.That(generator.Map.Width, Is.EqualTo(2));
            Assert.That(generator.Map.Height, Is.EqualTo(2));
        }

        [Test]
        public void CheckMapIsNotCompleted()
        {
            var generator = new AldousBroder(new PredictableRandom(), _configuration);

            Assert.That(generator.Complete, Is.False);
        }

        [Test]
        public void CheckGeneratesMap()
        {
            var generator = new AldousBroder(new PredictableRandom(), _configuration);

            Assert.That(generator.CurrentCoordinates.X, Is.EqualTo(0));
            Assert.That(generator.CurrentCoordinates.Y, Is.EqualTo(1));

            generator.Step();

            Assert.That(generator.CurrentCoordinates.X, Is.EqualTo(0));
            Assert.That(generator.CurrentCoordinates.Y, Is.EqualTo(0));

            generator.Step();

            Assert.That(generator.CurrentCoordinates.X, Is.EqualTo(1));
            Assert.That(generator.CurrentCoordinates.Y, Is.EqualTo(0));

            generator.Step();

            Assert.That(generator.CurrentCoordinates.X, Is.EqualTo(1));
            Assert.That(generator.CurrentCoordinates.Y, Is.EqualTo(1));

            Assert.That(generator.Complete, Is.True);
        }
    }
}