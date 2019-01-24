using System;
using kbs2.Actions.ActionTabActions;
using kbs2.Actions.GameActions;
using kbs2.Actions.Interfaces;
using kbs2.GamePackage;
using kbs2.WorldEntity.Interfaces;
using kbs2.WorldEntity.Unit;

namespace kbs2.Actions
{
    public class GameActionFactory
    {
        private readonly GameController game;

        public GameActionFactory(GameController game)
        {
            this.game = game;
        }

        public IGameAction CreateTrainAction(ITrainableDef trainableDef, ITrainingEntity trainingEntity)
        {
            IGameAction gameAction = new GameAction(trainableDef.IconData);

            gameAction.Clicked += () =>
            {
                UnitFactory unitFactory = new UnitFactory(trainingEntity.Faction, game);
                ITrainable spawnable = unitFactory.CreateNewTrainable(trainableDef);

                if (!trainingEntity.Faction.CanPurchase(spawnable.Def)) return;

                trainingEntity.Faction.Purchase(spawnable.Def);
                trainingEntity.TrainingQueue.Enqueue(spawnable);
            };

            return gameAction;
        }

        public SelectMapAction_GameAction CreateSelectAction(IMapAction mapAction)
        {
            SelectMapAction_GameAction tabItemAction = new SelectMapAction_GameAction(game.MapActionSelector, mapAction, mapAction.IconValues);

            return tabItemAction;
        }
    }
}