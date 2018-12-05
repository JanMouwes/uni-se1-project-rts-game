using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using kbs2.Desktop.World.World;
using kbs2.Faction.FactionMVC;
using kbs2.Unit.Model;
using kbs2.WorldEntity.Battle;
using kbs2.WorldEntity.Health;
using kbs2.WorldEntity.Interfaces;
using kbs2.WorldEntity.Location;
using kbs2.WorldEntity.XP.XPMVC;
using Microsoft.Xna.Framework;
using MonoGame.Extended;

namespace kbs2.WorldEntity.Unit.MVC
{
	public class Unit_Controller : ISelectable
	{
		public HP_Controller HPController;
		public XP_Controller XPController;
		public Battle_Controller BattleController;
		public Location_Controller LocationController;
		public Unit_Model UnitModel;
        public Unit_View UnitView;

		public Unit_Controller(WorldModel worldModel, string imageSrc, float height, float width, float lx, float ly)
		{
            UnitModel = new Unit_Model(height, width);
            UnitView = new Unit_View(imageSrc);
            LocationController = new Location_Controller(worldModel, lx, ly);
            UnitModel.UnitDef = new kbs2.Unit.Unit.UnitDef();
            UnitModel.UnitDef.Speed = 0.04f;
            LocationController.LocationModel.parrent = this;
        }
        // Create a new unit and add it to a faction
        public void CreateUnit(Faction_Model faction)
        {
            faction.Units.Add(this);
        }

        public RectangleF CalcClickBox()
        {
            return new RectangleF(LocationController.LocationModel.floatCoords.x - UnitModel.Width/2, LocationController.LocationModel.floatCoords.y - UnitModel.Height/2, UnitModel.Width, UnitModel.Height);
        }
	}
}
