using System.Collections.Generic;
using kbs2.Actions.Interfaces;
using kbs2.Faction.FactionMVC;
using kbs2.World;
using kbs2.World.Cell;
using kbs2.WorldEntity.Structures.Defs;

namespace kbs2.WorldEntity.Structures.BuildingUnderConstructionMVC
{
    public class ConstructingBuildingModel
    {
        public ConstructingBuildingDef BuildingDef { get; set; }
        public Faction_Controller FactionController { get; set; }

        public int FinishTime { get; set; }
        public List<IGameAction> GameActions { get; }

        public List<WorldCellModel> LocationCells { get; }
        public Coords StartCoords { get; set; }

        public ConstructingBuildingModel()
        {
            GameActions = new List<IGameAction>();
            LocationCells = new List<WorldCellModel>();
        }
    }
}