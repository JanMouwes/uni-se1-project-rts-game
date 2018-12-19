using System.Collections.Generic;
using kbs2.World;
using kbs2.WorldEntity.Interfaces;
using kbs2.WorldEntity.Structs;

namespace kbs2.WorldEntity.Building.BuildingUnderConstructionMVC
{
    public class ConstructingBuildingDef : IStructureDef
    {
        public ConstructingBuildingDef(IStructureDef completedBuildingDef, int constructionTime)
        {
            CompletedBuildingDef = completedBuildingDef;
            ConstructionTime = constructionTime;
        }

        public ViewValues ViewValues { get; set; }
        public List<Coords> BuildingShape { get; set; }
        public int ConstructionTime { get; set; }
        public IStructureDef CompletedBuildingDef { get; }
        public double Cost { get { return 0; } set => throw new System.NotImplementedException(); }
        public double UpkeepCost { get { return 0; } set => throw new System.NotImplementedException(); }
    }
}