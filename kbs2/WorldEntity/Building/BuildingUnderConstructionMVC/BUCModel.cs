using System;
using System.Collections.Generic;
using kbs2.World;
using kbs2.World.Cell;

namespace kbs2.WorldEntity.Building.BuildingUnderConstructionMVC
{
    public class BUCModel
    {
        public int Time { get; set; }
        public BuildingDef BuildingDef { get; set; }
        public Coords TopLeft { get; set; }
        public List<WorldCellModel> LocationCells { get; set; }


        public BUCModel()
        {
        }
    }
}
