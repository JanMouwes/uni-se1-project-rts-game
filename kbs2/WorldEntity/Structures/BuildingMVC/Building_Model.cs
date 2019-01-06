using System.Collections.Generic;
using kbs2.Actions.GameActionDefs;
using kbs2.Actions.GameActions;
using kbs2.World;
using kbs2.World.Cell;
using kbs2.WorldEntity.Building;
using kbs2.WorldEntity.Interfaces;

namespace kbs2.WorldEntity.Structures.BuildingMVC
{
    public class BuildingModel : BuildingModel<BuildingDef>
    {
    }

    public class BuildingModel<TStructureDef> where TStructureDef : IStructureDef
    {
        /// <summary>
        /// Building's definition
        /// </summary>
        public TStructureDef Def;

        /// <summary>
        /// Starting-coords from which the building originates.
        /// </summary>
        public Coords TopLeft { get; set; }

        /// <summary>
        /// Visibility to player.
        /// </summary>
        public ViewMode ViewMode;

        public List<MapAction<MapActionDef>> Actions { get; }

        //    FIXME change to WorldCellController. Never access a Model directly
        /// <summary>
        /// The cells the building occupies
        /// </summary>
        public List<WorldCellModel> LocationCells { get; }

        /// <summary>
        /// Constructor. Initialises LocationCells & Actions
        /// </summary>
        public BuildingModel()
        {
            LocationCells = new List<WorldCellModel>();
            Actions = new List<MapAction<MapActionDef>>();
        }
    }
}