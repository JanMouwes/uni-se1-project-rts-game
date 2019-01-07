using kbs2.Faction.FactionMVC;
using kbs2.GamePackage.AIPackage.Enums;
using kbs2.World;
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
        bool IsHostile { get; set; }
        bool IsCommandFinished { get; set; }
        Command CurrentCommand { get; set; }
        Unit_Controller TargetUnit { get; set; }
        Building_Controller TargetBuilding { get; set; }

        void Update();
        void AttackUnit(Unit_Controller target);
        void AttackBuilding(Building_Controller target);
        void CheckUnitTriggerRadius(List<Faction_Controller> unitList);
        void CheckBuildingTriggerRadius(List<Building_Controller> buildingList);
        
    }
}
