using System;
using kbs2.Desktop.World.World;
using kbs2.Faction.FactionMVC;
using kbs2.GamePackage;
using kbs2.WorldEntity.Building;
using kbs2.WorldEntity.Building.BuildingUnderConstructionMVC;
using kbs2.WorldEntity.Unit.MVC;

namespace kbs2.WorldEntity.WorldEntitySpawner
{
    public class EntitySpawner
    {
        public WorldController World { get; set; }
        public GameController Game { get; set; } 

        public EntitySpawner(WorldController world,GameController game)
        {
            World = world;
            Game = game;
        }

        public void SpawnUnit(Unit_Controller unit, Faction_Controller faction)
        {
            World.WorldModel.Units.Add(unit);
            faction.AddUnitToFaction(unit);
            Game.onTick += unit.LocationController.Ontick;
        }

        public void SpawnBUC(BUCController BUC, BuildingDef buildingDef, int ConstructionTime, Faction_Controller faction)
        {
            World.AddBuildingUnderCunstruction(buildingDef, BUC);
            BUC.World = BUC.World;
            BUC.gameController = Game;
            BUC.BUCModel.Time = ConstructionTime + Game.gametime
            Game.onTick += BUC.Update;
        }



    }
}
