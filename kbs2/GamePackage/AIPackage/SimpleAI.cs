using kbs2.Faction.FactionMVC;
using kbs2.GamePackage.AIPackage.Enums;
using kbs2.GamePackage.AIPackage.Interfaces;
using kbs2.GamePackage.CPU;
using kbs2.utils;
using kbs2.World;
using kbs2.WorldEntity.Building;
using kbs2.WorldEntity.Unit.MVC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kbs2.GamePackage.AIPackage
{
    public class SimpleAI : IAI
    {
        public Faction_Controller Faction { get; set; }
        public bool IsHostile { get; set; }
        public bool IsCommandFinished { get; set; }
        public Command CurrentCommand { get; set; }
        public Coords MoveTo { get; set; }
        public Unit_Controller TargetUnit { get; set; }
        public Building_Controller TargetBuilding { get; set; }

        public void Update(List<Faction_Controller> List)
        {
            if(CurrentCommand == Command.Idle)
            {
                CheckUnitTriggerRadius(List);
            }   
        }

        public void AttackBuilding(Building_Controller target)
        {
            TargetBuilding = target;
        }

        public void AttackUnit(Unit_Controller target)
        {
            TargetUnit = target;
        }

        public void CheckBuildingTriggerRadius(List<Building_Controller> buildingList)
        {
            
        }

        public void CheckUnitTriggerRadius(List<Faction_Controller> FactionsList)
        {
            List<Faction_Controller> HostileFactions = (from Fac in FactionsList
                                                               where Fac.IsHostileTo(Faction)
                                                               select Fac).ToList();

            List<Unit_Controller> HostileUnits = new List<Unit_Controller>();

            foreach (Faction_Controller faction in HostileFactions)
            {
                foreach(Unit_Controller unit in Faction.FactionModel.Units)
                {
                    HostileUnits.Add(unit);
                }
            }

            foreach(Unit_Controller HostileUnit in HostileUnits)
            {
                foreach(Unit_Controller PlayerUnit in Faction.FactionModel.Units)
                {
                    DistanceCalculator.getDistance2d(HostileUnit.LocationController.LocationModel.floatCoords, PlayerUnit.LocationController.LocationModel.floatCoords);
                }
            }

            //TODO: check if triggerradius is smaller than the double returned from the DistanceCalculator. 
            //If it is, then that means there is a unit inside the triggerradius
        }

        public void MoveToLocation(Coords coords)
        {

        }

        public void MoveRandom(Unit_Controller unit)
        {

        }
    }
}
