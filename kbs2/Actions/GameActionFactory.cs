using System.Collections.Generic;
using kbs2.Actions.GameActionDefs;
using kbs2.Actions.GameActions;
using kbs2.Actions.Interfaces;
using kbs2.Faction.FactionMVC;
using kbs2.GamePackage;
using kbs2.WorldEntity.Interfaces;

namespace kbs2.Actions
{
    public class GameActionFactory
    {
        private Faction_Controller faction;
        private GameController gameController;

        public GameActionFactory(Faction_Controller faction, GameController gameController)
        {
            this.faction = faction;
            this.gameController = gameController;
        }

        /// <summary>
        /// Temp function
        /// </summary>
        /// <returns>returns a list of game actions</returns>
        public List<IGameAction> DefaultGameActions()
        {
            List<IGameAction> returnList = new List<IGameAction>
            {
                CreateSpawnAction(SpawnActionDef.Raichu),
                CreateSpawnAction(SpawnActionDef.Pikachu)
            };

            return returnList;
        }

        /// <summary>
        /// Creates a spawnaction from the specified dev
        /// </summary>
        /// <param name="def">The spawnaction dev</param>
        /// <returns>Returns the specified spawnaction of the selected def</returns>
        public SpawnAction CreateSpawnAction(SpawnActionDef def)
        {
            return new SpawnAction(def, gameController, faction);
        }
    }
}