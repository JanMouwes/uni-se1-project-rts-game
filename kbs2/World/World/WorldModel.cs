using System;
using System.Collections.Generic;
using System.Linq;
using kbs2.World.Chunk;
using kbs2.WorldEntity.Building.BuildingUnderConstructionMVC;
using kbs2.WorldEntity.Interfaces;
using kbs2.WorldEntity.Structures.BuildingUnderConstructionMVC;
using kbs2.WorldEntity.Structures.ResourceFactory;
using kbs2.WorldEntity.Unit.MVC;

namespace kbs2.World.World
{
    public class WorldModel
    {
        //	Grid of chunks. Dictionary because it's expandable
        public Dictionary<Coords, WorldChunkController> ChunkGrid { get; set; }

        public List<IStructure<IStructureDef>> Structures { get; }

        public IEnumerable<ConstructingBuildingController> UnderConstruction => from structure in Structures
            where structure is ConstructingBuildingController
            select (ConstructingBuildingController) structure;

        public IEnumerable<ResourceFactoryController> ResourceBuildings => from structure in Structures
            where structure is ResourceFactoryController
            select (ResourceFactoryController) structure;

        public List<UnitController> Units { get; }

        public static readonly int Seed = new Random().Next(0, 9999);

        public WorldModel()
        {
            Structures = new List<IStructure<IStructureDef>>();
            Units = new List<UnitController>();
        }
    }
}