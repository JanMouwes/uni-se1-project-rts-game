using kbs2.Faction.FactionMVC;
using kbs2.GamePackage.AIPackage.Enums;
using kbs2.Unit.Interfaces;
using kbs2.World;
using kbs2.World.Structs;
using kbs2.WorldEntity.Building;
using kbs2.WorldEntity.Unit.MVC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kbs2.GamePackage.AIPackage.Interfaces
{
    public interface IAI
    {
        Dictionary<Unit_Controller, FloatCoords> MoveOrders { get; set; }

        void Update(List<Faction_Controller> factionList);
        void AttackTarget(IHasPersonalSpace target);
        void CheckTriggerRadius(List<Faction_Controller> List);
    }
}
