using System;
using kbs2.Faction.FactionMVC;
using kbs2.GamePackage;
using kbs2.GamePackage.EventArgs;
using kbs2.World;
using kbs2.World.Cell;
using kbs2.World.Structs;
using kbs2.World.World;
using kbs2.WorldEntity.Building;
using kbs2.WorldEntity.Building.BuildingMVC;
using kbs2.WorldEntity.Building.BuildingUnderConstructionMVC;
using kbs2.WorldEntity.Interfaces;
using kbs2.WorldEntity.Unit.MVC;

namespace kbs2.WorldEntity.WorldEntitySpawner
{
    public class EntitySpawner
    {
        private WorldController World => Game.GameModel.World;
        private GameController Game { get; }

        public EntitySpawner(GameController game)
        {
            Game = game;
        }

        public virtual void SpawnUnit(UnitController unit, Coords coords)
        {
            unit.LocationController.LocationModel.FloatCoords = (FloatCoords) coords;
            World.WorldModel.Units.Add(unit);
            unit.Faction.AddUnitToFaction(unit);
            Game.onTick += unit.Update;
        }

        // replace buc with building
        private void ReplaceBuilding(object sender, EventArgsWithPayload<IStructure> eventArgs)
        {
            IStructure<ConstructingBuildingDef> buildingConstruction = (ConstructingBuildingController) sender;
            World.RemoveStructure((IStructure) buildingConstruction);

            // Remove references to this from cells
            buildingConstruction.OccupiedCells.ForEach(cellModel => cellModel.BuildingOnTop = null);

            // make building
            BuildingController building = BuildingFactory.CreateNewBuilding((BuildingDef) buildingConstruction.Def.CompletedBuildingDef);
            SpawnStructure(buildingConstruction.StartCoords, building);
            buildingConstruction.Faction.AddBuildingToFaction(building);

            // unsub from ontick event
            Game.onTick -= buildingConstruction.Update;
        }

        /// <summary>
        /// Spawns IStructure according to underlying subclass 
        /// </summary>
        /// <param name="spawnCoords">Where to spawn, 'StartCoords'</param>
        /// <param name="structure">What to spawn</param>
        /// <exception cref="NotImplementedException">When structure isn't implemented yet</exception>
        public virtual void SpawnStructure(Coords spawnCoords, IStructure structure)
        {
            structure.StartCoords = spawnCoords;
            switch (structure)
            {
                case ConstructingBuildingController building:
                    SpawnConstructingBuilding(building, building.Def.ConstructionTime);
                    break;
                case BuildingController building:
                    SpawnBuilding(building);
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        public void SpawnBuilding(BuildingController buildingController)
        {
            World.AddBuilding(buildingController);

            Game.onTick += buildingController.Update;
        }

        public void SpawnConstructingBuilding(ConstructingBuildingController constructingBuilding, int constructionTime)
        {
            World.AddBuildingUnderConstruction(constructingBuilding.Def, constructingBuilding);
            constructingBuilding.ConstructingBuildingModel.FinishTime = (int) (constructionTime + Game.LastUpdateGameTime.TotalGameTime.TotalSeconds);

            Game.onTick += constructingBuilding.Update;
        }

        public void SpawnWorldEntity(IWorldEntity entity)
        {
            throw new NotImplementedException();
            //    FIXME Unfinished
            World.WorldModel.Units.Add(entity as UnitController);
        }
    }
}