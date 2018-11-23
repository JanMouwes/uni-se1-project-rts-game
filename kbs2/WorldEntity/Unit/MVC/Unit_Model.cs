using kbs2.World;
using kbs2.World.Structs;
using System;
using System.Collections.Generic;
using kbs2.Faction.FactionMVC;
using kbs2.Unit.Unit;
using kbs2.WorldEntity.Battle;
using kbs2.WorldEntity.Health;
using kbs2.WorldEntity.Location;
using kbs2.WorldEntity.XP.XPMVC;
using kbs2.Unit.Interfaces;
using kbs2.Unit.Abstract;

namespace kbs2.Unit.Model
{
    public class Unit_Model : IHasPersonalSpace, IPurchasable, IClickable, IElemental
    {
		public Faction_Model FactionModel;
		public UnitDef UnitDef;
		public HP_Model HPModel;
		public XP_Model XPModel;
		public Battle_Model BattleModel;
		public Location_Model LocationModel;

        public List<TerrainType> UnwalkableTerrain { get; set; }

        public List<ElementType> Elementtypes { get; set; }

        public CostValue CostValue { get; set; }

        public Hitbox Clickbox { get; set; }
        

        public Unit_Model() { }

        public void InsertUnitDef(int id)
        {
            DBController Con = new DBController();
            Con.OpenConnection("DefDex");

            UnitDef = Con.GetDefaultFromUnit(1);

            Con.CloseConnection();
        }

    }
}
