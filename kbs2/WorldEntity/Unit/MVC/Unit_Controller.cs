using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using kbs2.Faction.FactionMVC;
using kbs2.Unit.Model;
using kbs2.WorldEntity.Battle;
using kbs2.WorldEntity.Health;
using kbs2.WorldEntity.Location;
using kbs2.WorldEntity.XP.XPMVC;

namespace kbs2.WorldEntity.Unit.MVC
{
	public class Unit_Controller
	{
		public HP_Controller HPController;
		public XP_Controller XPController;
		public Battle_Controller BattleController;
		public Location_Controller LocationController;
		public Unit_Model UnitModel;

		public Unit_Controller()
		{

		}
        // Create a new unit and add it to a faction
        public void CreateUnit(Faction_Model faction)
        {
            UnitModel = new Unit_Model();
            faction.Units.Add(UnitModel);
        }
	}
}
