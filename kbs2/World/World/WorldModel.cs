using System.Collections.Generic;
using kbs2.World;
using kbs2.World.Chunk;
using System.Collections.Generic;

namespace kbs2.Desktop.World.World
{

	public class WorldModel
	{
    
		public Dictionary<Coords, WorldChunkController> ChunkGrid { get; set; }
		//old chunkGrid property. not able to extend bounderies because its an array. remove if not used.
        // public WorldChunkController[,] ChunkGrid { get; set; }
		public int ChunkSize { get; set; }
    
   }
}