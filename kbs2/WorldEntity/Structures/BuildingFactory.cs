using System;
using kbs2.Actions;
using kbs2.Actions.Interfaces;
using kbs2.Faction.FactionMVC;
using kbs2.GamePackage;
using kbs2.WorldEntity.Interfaces;
using kbs2.WorldEntity.Structures.BuildingMVC;
using kbs2.WorldEntity.Structures.Defs;
using kbs2.WorldEntity.Structures.ResourceFactory;
using kbs2.WorldEntity.Structures.TrainingStructure;

namespace kbs2.WorldEntity.Structures
{
    public class BuildingFactory : IDisposable
    {
        private Faction_Controller faction;
        private GameController game;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="faction">Factory's faction</param>
        /// <exception cref="ArgumentNullException">Thrown when faction is null</exception>
        public BuildingFactory(Faction_Controller faction, GameController game)
        {
            this.faction = faction ?? throw new ArgumentNullException(nameof(faction));
            this.game = game;
        }

        /// <summary>
        /// Creates new structure according to def belonging to factory's faction
        /// </summary>
        /// <param name="def">Definition of structure to be created</param>
        /// <returns>Structure defined by Def belonging to factory's faction</returns>
        public IStructure<IStructureDef> CreateNewBuilding(IStructureDef def)
        {
            IStructure<IStructureDef> buildingController;

            switch (def)
            {
                case ResourceFactoryDef resourceFactoryDef:
                    buildingController = new ResourceFactoryController(resourceFactoryDef, faction);
                    break;
                case TrainingStructureDef trainingStructureDef:
                    TrainingStructureController structureController = new TrainingStructureController(trainingStructureDef, game.Spawner);
                    structureController.Faction = faction;

                    GameActionFactory gameActionFactory = new GameActionFactory(game);

                    foreach (ITrainableDef trainableDef in trainingStructureDef.TrainableDefs)
                    {
                        IGameAction gameAction = gameActionFactory.CreateTrainAction(trainableDef, structureController);
                        structureController.GameActions.Add(gameAction);
                    }

                    buildingController = structureController;

                    break;
                default:
                    buildingController = new BuildingController((BuildingDef) def);
                    buildingController.Faction = faction;
                    break;
            }

            return buildingController;
        }

        public void Dispose()
        {
            faction = null;
        }
    }
}