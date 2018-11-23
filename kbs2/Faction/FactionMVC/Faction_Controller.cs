using kbs2.Unit.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kbs2.Faction.FactionMVC
{
    public class Faction_Controller
    {
        static public Faction_Model FactionModel { get; set; }

        Action<Unit_Model> AddUnitToFaction = unit => FactionModel.Units.Add(unit);
    }
}
