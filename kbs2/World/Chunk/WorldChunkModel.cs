using kbs2.World.Cell;

namespace kbs2.World.Chunk
{
    public class WorldChunkModel
    {
        public const int ChunkSize = 20;

        public WorldCellController[,] grid;
        public Coords ChunkCoords;
    }
}