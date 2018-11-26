using System.Collections.Generic;
using kbs2.World;
using kbs2.World.Chunk;
using System.Collections.Generic;

namespace kbs2.Desktop.World.World
{

	public class WorldModel
	{
    
		//	Grid of chunks. Dictionary because it's expandable
		public Dictionary<Coords, WorldChunkController> ChunkGrid { get; set; }
  }

}