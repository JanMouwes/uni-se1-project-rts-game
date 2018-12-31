using System;
using System.Collections.Generic;
using kbs2.Actions.GameActionDefs;
using kbs2.Faction.FactionMVC;
using kbs2.GamePackage;
using kbs2.World;
using kbs2.World.World;
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
        private ISpawnableDef SpawnableDef => ActionDef.SpawnableDef;

        private GameController gameController;
        private Faction_Controller factionController;

        public SpawnAction(SpawnActionDef actionDef, GameController gameController,
            Faction_Controller factionController) : base(actionDef)
        {
            this.gameController = gameController;
            this.factionController = factionController;
        }

        /// <summary>
        /// spawns a unit at a target location
        /// </summary>
        /// <param name="unit">unit to be spawned</param>
        /// <param name="targetable">The target</param>
        private void SpawnUnit(UnitController unit, ITargetable targetable)
        {
            gameController.Spawner.SpawnUnit(unit, (Coords)targetable.FloatCoords);
        }

        /// <summary>
        /// Spawns a building at a target location
        /// </summary>
        /// <param name="building">building to be spawned</param>
        /// <param name="spawntarget">The target</param>
        private void SpawnBuilding(IStructure building, ITargetable spawntarget)
        {
            ConstructingBuildingController constructingBuilding = ConstructingBuildingFactory.CreateNewBUCAt(building.Def, (Coords)spawntarget.FloatCoords, factionController);
            gameController.Spawner.SpawnStructure((Coords)spawntarget.FloatCoords, constructingBuilding);
        }

        /// <summary>
        /// Execute action on the selected target
        /// </summary>
        /// <param name="target">Selected target</param>
        public override void Execute(ITargetable target)
        {
            switch (SpawnableDef)
            {
                case BuildingDef buildingDef:
                    using (ConstructingBuildingFactory factory = new ConstructingBuildingFactory(factionController))
                    {
                        ConstructingBuildingController building = factory.CreateConstructingBuildingControllerOf(buildingDef);
                        SpawnBuilding(building, target);
                    }

                    break;
                case UnitDef unitDef:
                    using (UnitFactory factory = new UnitFactory(factionController))
                    {
                        UnitController unit = factory.CreateNewUnit(unitDef, new WorldModel());
                        SpawnUnit(unit, target);
                    }

                    break;

                default:
                    throw new NotImplementedException();
            }
        }
    }
}