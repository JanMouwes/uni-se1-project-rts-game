using kbs2.utils;
using kbs2.World;
using kbs2.World.Structs;
using NUnit.Framework;

namespace UnitTests
{
    [TestFixture]
    public class DistanceCalculatorTests
    {
        [TestCase(0, 0, 1, 1, 45)]
        [TestCase(0, 0, -1, 1, 135)]
        [TestCase(0, 0, -1, -1, 225)]
        [TestCase(0, 0, 1, -1, 315)]
        [TestCase(0, 0, 1, 0, 0)]
        [TestCase(0, 0, -1, 0, 180)]
        public void Test_ShouldReturnCorrectDegrees_WhenGivenCoords(int x1, int y1, int x2, int y2, double expected)
        {
            //    Arrange
            Coords firstCoords = new Coords() {x = x1, y = y1};
            Coords secondCoords = new Coords() {x = x2, y = y2};

            //    Act
            double actual = DistanceCalculator.DegreesFromCoords((FloatCoords) firstCoords, (FloatCoords) secondCoords);

            //    Assert
            Assert.AreEqual(expected, actual);
        }
    }
}