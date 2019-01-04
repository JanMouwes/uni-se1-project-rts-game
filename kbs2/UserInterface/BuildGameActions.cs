using kbs2.Actions.ActionModels;
using kbs2.Actions.ActionMVC;
using kbs2.GamePackage;
using kbs2.WorldEntity.Interfaces;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using kbs2.Actions;
using kbs2.Actions.GameActionDefs;
using kbs2.Actions.GameActions;
using kbs2.Actions.Interfaces;
using kbs2.WorldEntity.Building;

namespace kbs2.UserInterface
{
    public class BuildGameActions : IHasGameActions
    {
        public List<IGameAction> GameActions { get; set; }

        // generate test actionlist
        public BuildGameActions(GameController controller)
        {
            BuildingDef buildingDef = DBController.GetBuildingDef<BuildingDef>(1); //FIXME

            GameActions = new List<IGameAction>();
        }
    }
}