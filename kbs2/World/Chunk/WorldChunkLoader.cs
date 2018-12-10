using static kbs2.World.Chunk.WorldChunkFactory;

namespace kbs2.World.Chunk
{
    public static class WorldChunkLoader
    {
        public static ChunkGenerationDelegate ChunkGenerator = ChunkOfDefaultTerrain;
    }
}