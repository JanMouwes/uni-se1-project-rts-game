using kbs2.World;
using kbs2.World.Cell;
using kbs2.World.Chunk;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class WorldControllerTests
    {
        [TestCase(1, 1, 3, 4, 23, 24)]
        [TestCase(1, 2, 3, 4, 23, 44)]
        public void Test_Chunk_Generates_CorrectCellCoords(int chunkX, int chunkY, int relativeCellX, int relativeCellY,
            int expectedCellX, int expectedCellY)
        {
            //    Arrange
            WorldChunkController chunk;
            Coords chunkCoords = new Coords() {x = chunkX, y = chunkY};
            Coords relativeCellCoords = new Coords() {x = relativeCellX, y = relativeCellY};
            Coords expectedCellCoords = new Coords() {x = expectedCellX, y = expectedCellY};

            //    Act
            chunk = WorldChunkFactory.ChunkOfTerrainType(chunkCoords, TerrainType.Soil);


            //    Assert
            WorldCellController cell = chunk.WorldChunkModel.grid[relativeCellCoords.x, relativeCellCoords.y];

            Assert.AreEqual(cell.worldCellModel.RealCoords, expectedCellCoords);
        }
    }
}