using System;
using System.Collections.Generic;
using kbs2.Actions.GameActionDefs;
using kbs2.Actions.MapActions;
using kbs2.Faction.FactionMVC;
using kbs2.GamePackage;
using kbs2.World;
using kbs2.World.Cell;
using kbs2.World.Structs;
using kbs2.WorldEntity.Interfaces;
using kbs2.WorldEntity.Structs;
using kbs2.WorldEntity.Structures;
using kbs2.WorldEntity.Structures.BuildingUnderConstructionMVC;
using kbs2.WorldEntity.Unit;

namespace kbs2.Actions.GameActions
{
    //    TODO Generic ISpawnableDef (type of spawn-action, building vs unit vs ...?)
    public class SpawnAction : MapAction<IWorldEntity, SpawnActionDef>
    {
        private ISpawnableDef SpawnableDef => ActionDef.SpawnableDef;

        private GameController gameController;
        private Faction_Controller factionController;

        private ViewValues IconValues => SpawnableDef.ViewValues;
        public override List<MapActionAnimationItem> GetAnimationItems(FloatCoords @from, FloatCoords to) => new List<MapActionAnimationItem>();

        public SpawnAction(SpawnActionDef actionDef, GameController gameController, Faction_Controller factionController) : base(actionDef, actionDef.SpawnableDef.ViewValues)
        {
            this.gameController = gameController;
            this.factionController = factionController;
        }

        public override bool TryExecute(ITargetable target)
        {
            IWorldEntity worldEntity;
            switch (SpawnableDef)
            {
                case BuildingDef buildingDef:
                    using (ConstructingBuildingFactory factory = new ConstructingBuildingFactory(factionController))
                    {
                        worldEntity = factory.CreateConstructingBuildingControllerOf(buildingDef);
                    }

                    break;
                case UnitDef unitDef:
                    using (UnitFactory factory = new UnitFactory(factionController, gameController))
                    {
                        worldEntity = factory.CreateNewUnit(unitDef);
                    }

                    break;

                default:
                    return false;
            }

            gameController.Spawner.SpawnWorldEntity((Coords) target.FloatCoords, worldEntity);

            return true;
        }

        public override bool IsValidTarget(ITargetable targetable) => targetable is WorldCellController;
    }
}