using System;
using kbs2.Faction.FactionMVC;
using kbs2.Unit.Unit;
using kbs2.WorldEntity.Battle;
using kbs2.WorldEntity.Health;
using kbs2.WorldEntity.Location;
using kbs2.WorldEntity.XP.XPMVC;

namespace kbs2.Unit.Model
{
    public class Unit_Model
    {
		public Faction_Model FactionModel;
		public UnitDef UnitDef;
		public HP_Model HPModel;
		public XP_Model XPModel;
		public Battle_Model BattleModel;
		public Location_Model LocationModel;

		public Unit_Model()
        {
			
			
        }
    }
}
