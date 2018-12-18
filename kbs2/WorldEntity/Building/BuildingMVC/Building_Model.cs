using System.Collections.Generic;
using kbs2.Actions.ActionMVC;
using kbs2.Actions.GameActionDefs;
using kbs2.Actions.GameActions;
using kbs2.World;
using kbs2.World.Cell;

namespace kbs2.WorldEntity.Building.BuildingMVC
{
    public class Building_Model
    {
        public Coords TopLeft { get; set; }

        public List<GameAction<GameActionDef>> actions { get; }

        // all the cells the building is on
        public List<WorldCellModel> LocationCells { get; }

        public Building_Model()
        {
            LocationCells = new List<WorldCellModel>();
            actions = new List<GameAction<GameActionDef>>();
        }
    }
}