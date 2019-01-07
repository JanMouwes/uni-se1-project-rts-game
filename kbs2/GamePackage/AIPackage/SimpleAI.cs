using kbs2.Faction.FactionMVC;
using kbs2.GamePackage.AIPackage.Enums;
using kbs2.GamePackage.AIPackage.Interfaces;
using kbs2.GamePackage.CPU;
using kbs2.Unit.Interfaces;
using kbs2.utils;
using kbs2.World;
using kbs2.World.Structs;
using kbs2.WorldEntity.Building;
using kbs2.WorldEntity.Unit.MVC;
using MonoGame.Extended;
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
        public Dictionary<Unit_Controller, FloatCoords> MoveOrders { get; set; }

        /// <summary>
        /// The Update method checks if a unit has an order and executes that order, if not it will assign a random move order to that unit
        /// </summary>
        /// <param name="List">List of all the Factions in the game</param>
        public void Update(List<Faction_Controller> List)
        {
            // Check if there are any units in the trigger radius of any of the faction's units
            CheckTriggerRadius(List);

            // Loop through the faction's units and check which current order the unit has
            foreach (Unit_Controller unit in Faction.FactionModel.Units)
            {
                if(unit.Order == Command.AttackEntity)
                    AttackTarget(unit.Target);

                // Check if the unit has a Move command
                if (unit.Order == Command.Move)
                {
                    Console.WriteLine($"{Math.Round(unit.LocationController.LocationModel.floatCoords.x)}|{Math.Round(unit.LocationController.LocationModel.floatCoords.y)}, {Math.Round(MoveOrders[unit].x)}|{Math.Round(MoveOrders[unit].y)}");
                    
                    // Float so that unit doesnt have to reach the exact location, but near it.
                    float Left = unit.LocationController.LocationModel.floatCoords.x - 1;
                    float Top = unit.LocationController.LocationModel.floatCoords.y - 1;
                    float Right = unit.LocationController.LocationModel.floatCoords.x + 1;
                    float Bottom = unit.LocationController.LocationModel.floatCoords.y + 1;

                    // Unit has reached its final destination
                    if (MoveOrders[unit].x > Left && MoveOrders[unit].y > Top && MoveOrders[unit].x < Right && MoveOrders[unit].y < Bottom)
                        RemoveOrder(unit);

                    // Unit moves toward the desired location if order is not finished
                    if(unit.FinishedOrder == false)
                        MoveToLocation(unit, MoveOrders[unit]);
                }
                if (unit.Order == Command.Idle)
                {
                    Random random = new Random();

                    if (random.Next(1, 1000) < 5)
                    {
                        MoveRandom(unit);
                    }
                }
            }
        }

        public void AttackTarget(IHasPersonalSpace target)
        {
            // If target == building or unit
        }

        public void CheckTriggerRadius(List<Faction_Controller> FactionsList)
        {
            List<Faction_Controller> HostileFactions = (from Fac in FactionsList
                                                        where Fac != Faction
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
                    double distance = DistanceCalculator.getDistance2d(HostileUnit.LocationController.LocationModel.floatCoords, PlayerUnit.LocationController.LocationModel.floatCoords);
                    

                }
            }

            //TODO: check if triggerradius is smaller than the double returned from the DistanceCalculator. 
            //If it is, then that means there is a unit inside the triggerradius
        }
        /// <summary>
        /// Sets a MoveTo command to the given unit with a specific coordinate
        /// </summary>
        /// <param name="unit">The unit that has to receive the command</param>
        /// <param name="coords">The specific coords that the unit has to move to</param>
        private void MoveToLocation(Unit_Controller unit, FloatCoords coords)
        {
            unit.MoveTo(coords, false);
        }
        /// <summary>
        /// Sets a MoveTo command to the given unit with a random coordinate
        /// </summary>
        /// <param name="unit">The unit that has to receive the command</param>
        private void MoveRandom(Unit_Controller unit)
        {
            // Generate a random movement pattern according to the units current coordinate (No more than X amount of steps)
            Random random = new Random();

            const int walkRange = 3;

            float curPosX = unit.LocationController.LocationModel.floatCoords.x;
            float curPosY = unit.LocationController.LocationModel.floatCoords.y;

            FloatCoords positive = new FloatCoords() { x = (curPosX + walkRange), y = (curPosY + walkRange) };
            FloatCoords negative = new FloatCoords() { x = (curPosX - walkRange), y = (curPosY - walkRange) };

            MoveOrders.Add(unit, new FloatCoords() { x = random.Next((int)negative.x, (int)positive.x), y = random.Next((int)negative.y, (int)positive.y) });
            unit.Order = Command.Move;
            unit.FinishedOrder = false;
        }

        private void MoveSpecific(Unit_Controller unit, FloatCoords coords)
        {
            MoveOrders.Add(unit, coords);
            unit.Order = Command.Move;
        }

        private void RemoveOrder(Unit_Controller unit)
        {
            unit.FinishedOrder = true;
            MoveOrders.Remove(unit);
            unit.Order = Command.Idle;
        }
    }
}
