using kbs2.World;
using kbs2.World.Chunk;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class WorldControllerTests
    {
        [TestCase()]
        public void Test_Chunk_Generates_CorrectCellCoords(Coords chunkCoords,
            Coords relativeCellCoords,
            Coords expectedCellCoords)
        {
            //    Arrange
            WorldChunkModel chunkModel = WorldChunkFactory.ChunkOfTerrainType()
            
            //    Act
            
            
            //    Assert
            
        }
    }
}