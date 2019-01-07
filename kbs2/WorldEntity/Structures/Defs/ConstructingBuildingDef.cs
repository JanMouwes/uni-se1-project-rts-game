using System.Collections.Generic;
using kbs2.World;
using kbs2.World.Enums;
using kbs2.WorldEntity.Interfaces;
using kbs2.WorldEntity.Structs;

namespace kbs2.WorldEntity.Structures.Defs
{
    public class ConstructingBuildingDef : IStructureDef
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="completedBuildingDef">Definition of the to-be-built structure</param>
        /// <param name="constructionTime">Time it takes to build structure</param>
        public ConstructingBuildingDef(IStructureDef completedBuildingDef, int constructionTime)
        {
            CompletedBuildingDef = completedBuildingDef;
            ConstructionTime = constructionTime;
        }

        public ViewValues ViewValues { get; set; }
        public int ViewRange { get; set; }
        public List<Coords> BuildingShape { get; set; }
        public int ConstructionTime { get; }
        public IStructureDef CompletedBuildingDef { get; }

        public double Cost
        {
            get => 0;
            set => throw new System.NotImplementedException();
        }

        public double UpkeepCost
        {
            get => 0;
            set => throw new System.NotImplementedException();
        }

        public List<TerrainType> LegalTerrain { get; set; }
    }
}