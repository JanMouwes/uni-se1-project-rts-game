using kbs2.GamePackage;
using kbs2.WorldEntity.Interfaces;
using System.Collections.Generic;
using kbs2.Actions.Interfaces;
using kbs2.WorldEntity.Structures;

namespace kbs2.UserInterface
{
    public class BuildGameActionHolder : IGameActionHolder
    {
        public List<IGameAction> GameActions { get; set; }

        // generate test actionlist
        public BuildGameActionHolder(GameController controller)
        {
            BuildingDef buildingDef = DBController.GetBuildingDef<BuildingDef>(1); //FIXME

            GameActions = new List<IGameAction>();
        }
    }
}