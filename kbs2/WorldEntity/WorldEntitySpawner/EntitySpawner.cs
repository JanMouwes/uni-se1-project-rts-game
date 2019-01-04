using System;
using kbs2.Faction.FactionMVC;
using kbs2.GamePackage;
using kbs2.GamePackage.EventArgs;
using kbs2.World;
using kbs2.World.Cell;
using kbs2.World.Structs;
using kbs2.World.World;
using kbs2.WorldEntity.Building;
using kbs2.WorldEntity.Building.BuildingUnderConstructionMVC;
using kbs2.WorldEntity.Interfaces;
using kbs2.WorldEntity.Structures;
using kbs2.WorldEntity.Structures.BuildingMVC;
using kbs2.WorldEntity.Structures.BuildingUnderConstructionMVC;
using kbs2.WorldEntity.Structures.Defs;
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
        public void ReplaceBuilding(object sender, EventArgsWithPayload<IStructureDef> eventArgs)
        {
            if (!(sender is ConstructingBuildingController structure)) return;

            //    Remove old structure
            DespawnStructure(structure);

            //    Add new structure
            BuildingFactory factory = new BuildingFactory(structure.Faction);

            IStructure<IStructureDef> building = factory.CreateNewBuilding(structure.Def.CompletedBuildingDef);

            SpawnStructure(structure.StartCoords, building);
        }

        /// <summary>
        /// Spawns IStructure:
        /// <para>- Adds structure to world</para> 
        /// <para>- Registers building to faction</para> 
        /// <para>- Subscribes Update to Game's onTick</para> 
        /// <para>- If ConstructingBuilding, Registers finish-time</para> 
        /// </summary>
        /// <param name="spawnCoords">Where to spawn, 'StartCoords'</param>
        /// <param name="structure">What to spawn</param>
        public virtual void SpawnStructure(Coords spawnCoords, IStructure<IStructureDef> structure)
        {
            structure.StartCoords = spawnCoords;

            World.AddStructure(structure);

            structure.Faction.RegisterBuilding(structure);

            Game.onTick += structure.Update;

            if (!(structure is ConstructingBuildingController)) return;

            ConstructingBuildingController constructingBuilding = (ConstructingBuildingController) structure;
            constructingBuilding.ConstructingBuildingModel.FinishTime = (int) (constructingBuilding.Def.ConstructionTime + Game.LastUpdateGameTime.TotalGameTime.TotalSeconds);
        }

        /// <summary>
        /// Despawns IStructure:
        /// <para>- Removes structure from world</para> 
        /// <para>- Unregisters building to faction</para> 
        /// <para>- Unsubscribes Update from Game's onTick</para>  
        /// </summary>
        /// <param name="structure">What to spawn</param>
        /// <exception cref="NotImplementedException">When structure isn't implemented yet</exception>
        public virtual void DespawnStructure(IStructure<IStructureDef> structure)
        {
            World.RemoveStructure(structure);

            structure.Faction.UnregisterBuilding(structure);

            Game.onTick -= structure.Update;
        }

        public void SpawnWorldEntity(IWorldEntity entity)
        {
            throw new NotImplementedException();
            //    FIXME Unfinished
            World.WorldModel.Units.Add(entity as UnitController);
        }
    }
}