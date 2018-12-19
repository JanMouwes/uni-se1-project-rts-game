using kbs2.utils;
using kbs2.World;
using kbs2.World.Structs;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class WorldPositionCalculatorTests
    {
        [TestCase(20, 20, 20, 1, 1)]
        public void Test_ShouldGiveCorrectCoords_WhenGivenTileSize(int drawCoordsX, int drawCoordsY, int tileSize,
            int expectedX, int expectedY)
        {
            //    Arrange
            Coords drawCoords = new Coords { x = drawCoordsX, y = drawCoordsY };

            //    Act
            Coords actual = WorldPositionCalculator.DrawCoordsToCellCoords(drawCoords, tileSize);

            //    Assert
            Coords expectedCoords = new Coords { x = expectedX, y = expectedY };
            Assert.AreEqual(expectedCoords, actual);
        }

        [Test]
        [TestCase(20, 20, 1, 1)]
        public void Test_ShouldGiveChunkCoords_WhenGivenCellCoords(int tileX, int tileY, int expectedChunkX,
            int expectedChunkY)
        {
            //    Arrange
            FloatCoords cellCoords = new FloatCoords { x = tileX, y = tileY };

            //    Act
            Coords actual = WorldPositionCalculator.ChunkCoordsOfCellCoords(cellCoords);

            //    Assert
            Coords expectedCoords = new Coords { x = expectedChunkX, y = expectedChunkY };
            Assert.AreEqual(expectedCoords, actual);
        }
    }
}