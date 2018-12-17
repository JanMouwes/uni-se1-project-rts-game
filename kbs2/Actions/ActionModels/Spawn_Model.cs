using kbs2.Desktop.World.World;
using kbs2.Faction.FactionMVC;
using kbs2.GamePackage;
using kbs2.WorldEntity.WorldEntitySpawner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kbs2.Actions.ActionModels
{
    class Spawn_Model : IActionModel
    {
        public int Index { get; set; }
        public EntitySpawner spawner { get; set; }
        public WorldController World { get; set; }
        public Faction_Controller faction { get; set; }
        public int ConstructionTime { get; set; }
        public int CoolDown { get; set ; }
        public int CurrentCoolDown { get; set; }
    }
}
