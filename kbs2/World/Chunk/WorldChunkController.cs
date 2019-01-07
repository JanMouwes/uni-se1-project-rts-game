namespace kbs2.World.Chunk
{
    public class WorldChunkController
    {
        public WorldChunkModel WorldChunkModel { get; set; } = new WorldChunkModel();

        public void Load()
        {
        }

        public void UnLoad()
        {
        }

        private void LoadFromFile(string fileName)
        {
        }

        public WorldChunkController(Coords coords)
        {
            WorldChunkModel.ChunkCoords = coords;
        }
    }
}