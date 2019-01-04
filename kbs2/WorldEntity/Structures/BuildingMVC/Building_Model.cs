using System.Collections.Generic;
using kbs2.Actions.GameActionDefs;
using kbs2.Actions.GameActions;
using kbs2.World;
using kbs2.World.Cell;
using kbs2.WorldEntity.Interfaces;

namespace kbs2.WorldEntity.Structures.BuildingMVC
{
    public class BuildingModel : BuildingModel<IStructureDef>
    {
    }

    public class BuildingModel<TStructureDef> where TStructureDef : IStructureDef
    {
        public TStructureDef Def;

        public Coords TopLeft { get; set; }

        public ViewMode ViewMode;

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