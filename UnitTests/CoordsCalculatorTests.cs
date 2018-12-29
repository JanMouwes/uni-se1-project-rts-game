using System.Collections.Generic;
using System.Linq;
using kbs2.utils;
using kbs2.World;
using kbs2.World.Structs;
using NUnit.Framework;

namespace UnitTests
{
    [TestFixture]
    public class CoordsCalculatorTests
    {
        [TestCase(0, 2, 2, 2, 2)]
        [TestCase(2, 0, 2, 2, 2)]
        [TestCase(0, 0, 3, 4, 5)]
        public void Test_ShouldReturnCorrectDistance_WhenGivenTwoPoints(int x1, int y1, int x2, int y2, double expectedDistance)
        {
            //    Arrange
            FloatCoords floatCoords = new FloatCoords()
            {
                x = x1,
                y = y1
            };
            FloatCoords target = new FloatCoords()
            {
                x = x2,
                y = y2
            };
            CoordsCalculator coordsCalculator = new CoordsCalculator(floatCoords);

            //    Act
            double actual = coordsCalculator.DistanceToFloatCoords(target);

            //    Assert
            Assert.AreEqual(expectedDistance, actual);
        }

        [TestCase(0, 0, 2, 2, 1, 1)]
        [TestCase(0, 0, 0, 2, -1, 1)]
        [TestCase(0, 0, 0, 2, 0, 1)]
        [TestCase(0, 0, 0, 2, 1, 1)]
        public void Test_ShouldReturnCorrectNeighbour_WhenGivenTwoPoints(int x1, int y1, int x2, int y2, int x3, int y3)
        {
            //    Arrange
            FloatCoords floatCoords = new FloatCoords()
            {
                x = x1,
                y = y1
            };
            FloatCoords target = new FloatCoords()
            {
                x = x2,
                y = y2
            };
            FloatCoords expected = new FloatCoords()
            {
                x = x3,
                y = y3
            };
            CoordsCalculator coordsCalculator = new CoordsCalculator(floatCoords);

            //    Act
            List<FloatCoords> actual = coordsCalculator.GetCommonNeighboursWith((Coords) target);

            //    Assert
            Assert.IsTrue(actual.Any(neighbour => neighbour == expected));
        }
    }
}