using System.Collections.Generic;
using kbs2.Actions.ActionMVC;
using kbs2.Actions.GameActionDefs;
using kbs2.Actions.GameActions;
using kbs2.World;
using kbs2.World.Cell;

namespace kbs2.WorldEntity.Building.BuildingMVC
{
    public class BuildingModel
    {
        public Coords TopLeft { get; set; }

        public List<GameAction<GameActionDef>> Actions { get; }

        //    FIXME change to WorldCellController. Never access a Model directly
        // all the cells the building is on
        public List<WorldCellModel> LocationCells { get; }

        public BuildingModel()
        {
            LocationCells = new List<WorldCellModel>();
            Actions = new List<GameAction<GameActionDef>>();
        }
    }
}