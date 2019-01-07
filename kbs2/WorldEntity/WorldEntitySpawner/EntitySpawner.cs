using System;
using kbs2.GamePackage;
using kbs2.GamePackage.EventArgs;
using kbs2.World;
using kbs2.World.Structs;
using kbs2.World.World;
using kbs2.WorldEntity.Interfaces;
using kbs2.WorldEntity.Structures;
using kbs2.WorldEntity.Structures.BuildingUnderConstructionMVC;
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

        /// <summary>
        /// Spawns Unit at coords, registers unit to faction, subscribes unit's Update to onTick
        /// </summary>
        /// <param name="unit">Unit to be spawned</param>
        /// <param name="coords">Location at which to spawn unit</param>
        public virtual void SpawnUnit(UnitController unit, Coords coords)
        {
            unit.LocationController.LocationModel.FloatCoords = (FloatCoords) coords;
            World.WorldModel.Units.Add(unit);
            unit.Faction.RegisterUnit(unit);
            Game.onTick += unit.Update;
        }

        /// <summary>
        /// Replaces one ConstructingBuilding with an IStructure.
        /// </summary>
        /// <param name="sender">ConstructingBuilding to be replaced</param>
        /// <param name="eventArgs">EventArgs containing new structure's definition</param>
        public void ReplaceBuilding(object sender, EventArgsWithPayload<IStructureDef> eventArgs)
        {
            if (!(sender is ConstructingBuildingController structure)) return;

            //    Remove old structure
            DespawnStructure(structure);

            //    Add new structure
            BuildingFactory factory = new BuildingFactory(structure.Faction, Game);

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

        public void SpawnWorldEntity(Coords location, IWorldEntity entity)
        {
            switch (entity)
            {
                case IStructure<IStructureDef> structure:
                    SpawnStructure(location, structure);
                    break;
                case UnitController unit:
                    SpawnUnit(unit, location);
                    break;
            }
        }
    }
}