using System.Collections.Generic;
using kbs2.World;
using kbs2.World.Chunk;
using kbs2.WorldEntity.Building;

namespace kbs2.Desktop.World.World
{

	public class WorldModel
	{
    
		//	Grid of chunks. Dictionary because it's expandable
		public Dictionary<Coords, WorldChunkController> ChunkGrid { get; set; }
        public List<Building_Controller> buildings { get; set; }

        public WorldModel()
        {
            buildings = new List<Building_Controller>();
        }
    }

}