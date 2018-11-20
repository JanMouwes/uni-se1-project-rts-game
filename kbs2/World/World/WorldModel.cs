using System.Collections.Generic;
using kbs2.World;
using kbs2.World.Chunk;

namespace kbs2.Desktop.World.World
{
	public class WorldModel
	{
		public Dictionary<Coords, WorldChunkController> ChunkGrid { get; set; }
    }
}