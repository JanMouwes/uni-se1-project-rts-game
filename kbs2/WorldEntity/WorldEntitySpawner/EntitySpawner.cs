using System;
using kbs2.Desktop.World.World;
using kbs2.Faction.FactionMVC;
using kbs2.GamePackage;
using kbs2.WorldEntity.Unit.MVC;

namespace kbs2.WorldEntity.WorldEntitySpawner
{
    public class EntitySpawner
    {
        public WorldController World { get; set; }
        public event OnTick OnTick;

        public EntitySpawner(WorldController world, ref OnTick onTick)
        {
            World = world;
            OnTick = onTick;
        }

        public void SpawnUnit(Unit_Controller unit, Faction_Controller faction)
        {
            World.WorldModel.Units.Add(unit);
            faction.AddUnitToFaction(unit);
            //OnTick += unit.LocationController.Ontick; todo fix deze zooi
        }



    }
}
