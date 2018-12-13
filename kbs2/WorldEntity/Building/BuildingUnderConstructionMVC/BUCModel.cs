using System;
using System.Collections.Generic;
using kbs2.Actions;
using kbs2.Faction.FactionMVC;
using kbs2.World;
using kbs2.World.Cell;

namespace kbs2.WorldEntity.Building.BuildingUnderConstructionMVC
{
    public class BUCModel
    {
        public BUCModel BucModel { get; set; }
        public int Time { get; set; }
        public BuildingDef BuildingDef { get; set; }
        public Coords TopLeft { get; set; }
        public List<WorldCellModel> LocationCells { get; set; }
        public Faction_Controller faction_Controller { get; set; }
        public List<ActionController> actions{ get; set; }


        public BUCModel(Coords topLeft)
        {
            TopLeft = topLeft;
            LocationCells = new List<WorldCellModel>();
        }
    }
}
