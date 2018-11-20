using kbs2.World;
using kbs2.World.Chunk;
using System.Collections.Generic;

namespace kbs2.Desktop.World.World
{
    public class WorldModel
    {
        //public WorldChunkController[,] ChunkGrid { get; set; }
        public Dictionary<Coords, WorldChunkController> ChunkGrid { get; set;}
        public int ChunkSize { get; set; }
    }
}