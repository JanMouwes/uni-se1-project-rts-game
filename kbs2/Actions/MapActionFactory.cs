using System.Collections.Generic;
using kbs2.Actions.GameActionDefs;
using kbs2.Actions.GameActions;
using kbs2.Actions.Interfaces;
using kbs2.Faction.FactionMVC;
using kbs2.GamePackage;
using kbs2.WorldEntity.Interfaces;

namespace kbs2.Actions
{
    public class MapActionFactory
    {
        private readonly Faction_Controller faction;
        private readonly GameController game;

        public MapActionFactory(Faction_Controller faction, GameController game)
        {
            this.faction = faction;
            this.game = game;
        }

        /// <summary>
        /// Temp function
        /// </summary>
        /// <returns>returns a list of game actions</returns>
        public List<IMapAction> DefaultGameActions()
        {
            List<IMapAction> returnList = new List<IMapAction>
            {
                CreateSpawnAction(SpawnActionDef.Raichu),
                CreateSpawnAction(SpawnActionDef.Pikachu)
            };

            return returnList;
        }

        /// <summary>
        /// Creates a spawnaction from the specified def
        /// </summary>
        /// <param name="def">The spawnaction dev</param>
        /// <returns>Returns the specified spawnaction of the selected def</returns>
        public SpawnAction CreateSpawnAction(SpawnActionDef def)
        {
            return new SpawnAction(def, game, faction);
        }
    }
}