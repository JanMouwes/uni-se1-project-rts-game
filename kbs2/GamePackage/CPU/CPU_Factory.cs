using kbs2.Faction.FactionMVC;
using kbs2.GamePackage.AIPackage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kbs2.GamePackage.CPU
{
    public class CPU_Factory
    {
        public static CPU_Controller CreateSimpleCpu(Faction_Controller faction)
        {
            CPU_Controller Cpu_Controller = new CPU_Controller()
            {
                CpuModel = new CPU_Model()
                {
                    AI = new SimpleAI()
                    {
                        Faction = faction,
                        MoveOrders = new Dictionary<WorldEntity.Unit.MVC.UnitController, World.Structs.FloatCoords>()
                    }

                }

            };

            return Cpu_Controller;
        }
    }
}