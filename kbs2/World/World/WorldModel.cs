using System;
using System.Collections.Generic;
using kbs2.World.Chunk;
using kbs2.WorldEntity.Building.BuildingUnderConstructionMVC;
using kbs2.WorldEntity.Interfaces;
using kbs2.WorldEntity.Unit.MVC;

namespace kbs2.World.World
{

	public class WorldModel
	{
    
		//	Grid of chunks. Dictionary because it's expandable
		public Dictionary<Coords, WorldChunkController> ChunkGrid { get; set; }
        public List<IStructure> Structures { get; set; }
        public List<ConstructingBuildingController> UnderConstruction { get; set; } = new List<ConstructingBuildingController>();
        public List<UnitController> Units { get; set; }

        public int seed { get; }

        public WorldModel(int seed)
        {
            Structures = new List<IStructure>();
            Units = new List<UnitController>();
            this.seed = seed;
        }
    }

}