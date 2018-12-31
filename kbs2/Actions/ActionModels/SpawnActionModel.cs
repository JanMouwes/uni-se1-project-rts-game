using kbs2.Faction.FactionMVC;
using kbs2.GamePackage;
using kbs2.WorldEntity.WorldEntitySpawner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using kbs2.World.World;

namespace kbs2.Actions.ActionModels
{
    public class SpawnActionModel : IActionModel
    {
        public int Id { get; set; }
        public EntitySpawner Spawner { get; set; }
        public WorldController World { get; set; }
        public Faction_Controller Faction { get; set; }
        public int ConstructionTime { get; set; }
        public float CoolDown { get; set; }
        public float CurrentCoolDown { get; set; }
    }
}