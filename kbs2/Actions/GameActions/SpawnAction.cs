using System;
using System.Collections.Generic;
using kbs2.Actions.GameActionDefs;
using kbs2.Faction.FactionMVC;
using kbs2.GamePackage;
using kbs2.World;
using kbs2.WorldEntity.Building;
using kbs2.WorldEntity.Building.BuildingMVC;
using kbs2.WorldEntity.Building.BuildingUnderConstructionMVC;
using kbs2.WorldEntity.Interfaces;
using kbs2.WorldEntity.Unit;
using kbs2.WorldEntity.Unit.MVC;

namespace kbs2.Actions.GameActions
{
    //    TODO Generic ISpawnableDef (type of spawn-action, building vs unit vs ...?)
    public class SpawnAction : GameAction<SpawnActionDef>
    {
        public ISpawnableDef Type { get; set; }

        private GameController gameController;
        private Faction_Controller factionController;

        public SpawnAction(SpawnActionDef actionDef, GameController gameController,
            Faction_Controller factionController) : base(actionDef)
        {
            this.gameController = gameController;
            this.factionController = factionController;
        }

        public override void Execute(ITargetable target)
        {
            Action<ISpawnable, ITargetable> spawnUnit = (spawnable, targetable) =>
            {
                UnitController unit = UnitFactory.CreateNewUnit((UnitDef) ActionDef.SpawnableDef,
                    target.FloatCoords, gameController.GameModel.World.WorldModel);
                gameController.Spawner.SpawnUnit(unit, factionController);
            };
            Action<ISpawnable, ITargetable> spawnBuilding = (spawnable, spawntarget) =>
            {
                IStructure building = (IStructure) (spawnable);
                
                ConstructingBuildingController constructingBuilding = ConstructingBuildingFactory.CreateNewBUCAt(
                    building.Def,
                    (Coords) spawntarget.FloatCoords,
                    factionController
                );
                gameController.Spawner.SpawnConstructingBuilding(constructingBuilding, 20); //FIXME
            };

            Dictionary<Type, Action<ISpawnable, ITargetable>> dictionary =
                new Dictionary<Type, Action<ISpawnable, ITargetable>>()
                {
                    {typeof(UnitDef), spawnUnit},
                    {typeof(BuildingDef), spawnBuilding}
                };
        }
    }
}