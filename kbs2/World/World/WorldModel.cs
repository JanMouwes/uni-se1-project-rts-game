using System;
using System.Collections.Generic;
using kbs2.World;
using kbs2.World.Chunk;
using kbs2.WorldEntity.Building;
using kbs2.WorldEntity.Building.BuildingUnderConstructionMVC;
using kbs2.WorldEntity.Unit.MVC;

namespace kbs2.Desktop.World.World
{

	public class WorldModel
	{
    
		//	Grid of chunks. Dictionary because it's expandable
		public Dictionary<Coords, WorldChunkController> ChunkGrid { get; set; }
        public List<Building_Controller> buildings { get; set; }
        public List<BUCController> UnderConstruction { get; set; }
        public List<Unit_Controller> Units { get; set; }

        public readonly int seed = new Random().Next(0, 10000);

        public WorldModel()
        {
            buildings = new List<Building_Controller>();
            UnderConstruction = new List<BUCController>();
            Units = new List<Unit_Controller>();
        }
    }

}