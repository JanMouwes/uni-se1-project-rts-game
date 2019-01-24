using System.Collections.Generic;
using kbs2.Actions;
using kbs2.Actions.GameActionDefs;
using kbs2.Actions.GameActions;
using kbs2.Actions.Interfaces;
using kbs2.Faction.FactionMVC;
using kbs2.WorldEntity.Interfaces;
using kbs2.WorldEntity.Unit.MVC;

namespace kbs2.GamePackage.GameScenario
{
    public class GameScenario
    {
        /// <summary>
        /// Entities the player starts with in scenario
        /// </summary>
        public IEnumerable<IWorldEntity> StartingEntities { get; } = new List<IWorldEntity>();

        /// <summary>
        /// Actions to be displayed when no entities selected
        /// </summary>
        public List<IGameAction> BaseActions { get; } = new List<IGameAction>();

        public List<IMapActionDef> BaseMapActionDefs { get; } = new List<IMapActionDef>();

        /// <summary>
        /// Currency that the player starts with
        /// </summary>
        public float StartingBalance { get; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="startingBalance">Currency that the player starts with</param>
        public GameScenario(float startingBalance)
        {
            StartingBalance = startingBalance;
        }

        public void Initialise(GameController game, Faction_Controller playerFaction)
        {
            MapActionFactory mapActionFactory = new MapActionFactory(playerFaction, game);
            GameActionFactory gameActionFactory = new GameActionFactory(game);

            foreach (IMapActionDef actionDef in BaseMapActionDefs)
            {
                //NOTE move to factory?
                IMapAction mapAction = mapActionFactory.CreateSpawnAction((SpawnActionDef) actionDef);
                IGameAction selectAction = gameActionFactory.CreateSelectAction(mapAction);
                BaseActions.Add(selectAction);
            }
        }

        public static GameScenario DefaultScenario => new GameScenario(500)
        {
            BaseMapActionDefs =
            {
                new SpawnActionDef(40, "TrainingCenter", DBController.GetBuildingDef(1)),
                new SpawnActionDef(40, "factory_mine", DBController.GetBuildingDef(3))
            }
        };
    }
}