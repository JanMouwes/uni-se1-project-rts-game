using System.Collections.Generic;
using kbs2.Actions.GameActionDefs;
using kbs2.Actions.GameActions;
using kbs2.Actions.Interfaces;
using kbs2.Actions.MapActionDefs;
using kbs2.Actions.MapActions;
using kbs2.Faction.FactionMVC;
using kbs2.GamePackage;
using kbs2.GamePackage.Interfaces;
using kbs2.WorldEntity.Structs;
using kbs2.WorldEntity.Unit.MVC;

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
            SpawnAction spawnAction = new SpawnAction(def, game, faction);
            game.onTick += spawnAction.Update;
            return spawnAction;
        }

        /// <summary>
        /// Creates a spawnaction from the specified def
        /// </summary>
        /// <param name="def">The spawnaction dev</param>
        /// <param name="unit">Sender</param>
        /// <returns>Returns the specified spawnaction of the selected def</returns>
        public AttackAction CreateAttackAction(AttackActionDef def, UnitController unit)
        {
            AttackAction attackAction = new AttackAction(def, def.Icon, unit);
            attackAction.MapActionExecuted += (sender, eventArgs) =>
            {
                UnitController unitSender = sender;

                //    Animation
                List<MapActionAnimationItem> animationItems = attackAction.ActionDef.GetAnimationItems(unitSender.Center, eventArgs.Value);
                game.AnimationController.AddAnimation_ByFrames(new List<IViewItem>(animationItems), 20);

                //    Cooldown-reset
                attackAction.CurrentCooldown = attackAction.ActionDef.CooldownTime;
            };

            game.onTick += attackAction.Update;
            return attackAction;
        }
    }
}